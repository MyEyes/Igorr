using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Content;
using IGORR.Client.Logic;

namespace IGORR.Client
{
    public class LightSource
    {
        public Vector2 position;
        public Color color;
        public float radius;
        public int id;
        public bool shadows;
        public float timeOutStart = 0;
        public float timeOut = 0;
        public float brightness=1;
    }

    public class LightMap
    {
        Map map;
        List<LightSource> _glows = new List<LightSource>();
        Mutex _lightMutex;

        const int lightMapDownSample = 1;

        static VertexBuffer clearVBuffer;
        static IndexBuffer clearIBuffer;
        public static Matrix rotateLeft = Matrix.Identity;
        public static Matrix rotateRight = Matrix.Identity;
        VertexBuffer shadowVB;
        IndexBuffer shadowIB;
        RenderTarget2D shadowTarget;
        RenderTarget2D shadowTarget2;
        BlendState blendShadow;
        BlendState generateShadow;
        BlendState generateShadow2;
        BlendState addLight;
        DepthStencilState generateShadowStencil;
        DepthStencilState drawShadowedLight;
        DepthStencilState disableShadows;
        DepthStencilState clearStencil;
        Effect shadowEffect;

        Texture2D _unusedTexture;

        GraphicsDevice device;

        const int maxX = 30;
        const int maxY = 30;

        public static LightMap LightReference = null;

        public LightMap(GraphicsDevice device)
        {
            this.device = device;
            LightReference = this;
            //Four triangles per shadow three shadows per block so 4*3*3*maxX*maxY
            shadowIB = new IndexBuffer(device, IndexElementSize.SixteenBits, 4 * 3 * 3 * maxX * maxY, BufferUsage.WriteOnly);
            short[] indices = new short[4 * 3 * 3 * maxX * maxY];
            for (int x = 0; x < 4 * 3 * 3 * maxX * maxY; x += 12)
            {
                short offset = (short)(6 * x / 12);
                indices[x] = (short)(offset);
                indices[x + 1] = (short)(offset + 1);
                indices[x + 2] = (short)(offset + 4);

                indices[x + 3] = (short)(offset + 1);
                indices[x + 4] = (short)(offset + 2);
                indices[x + 5] = (short)(offset + 4);

                indices[x + 6] = (short)(offset + 2);
                indices[x + 7] = (short)(offset + 5);
                indices[x + 8] = (short)(offset + 4);

                indices[x + 9] = (short)(offset + 2);
                indices[x + 10] = (short)(offset + 3);
                indices[x + 11] = (short)(offset + 5);
            }
            shadowIB.SetData<short>(indices);
            shadowVB = new VertexBuffer(device, VertexPositionColor.VertexDeclaration, 6 * 3 * maxX * maxY, BufferUsage.WriteOnly);
            shadowEffect = ContentInterface.LoadShader("ShadowEffect");
            shadowTarget = new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth/lightMapDownSample, device.PresentationParameters.BackBufferHeight/lightMapDownSample, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 1, RenderTargetUsage.DiscardContents);
            shadowTarget2 = new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth / lightMapDownSample, device.PresentationParameters.BackBufferHeight / lightMapDownSample, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 1, RenderTargetUsage.DiscardContents);
            blendShadow = new BlendState();
            blendShadow.ColorWriteChannels = ColorWriteChannels.All;
            blendShadow.ColorBlendFunction = BlendFunction.Add;
            blendShadow.ColorDestinationBlend = Blend.SourceColor;
            blendShadow.ColorSourceBlend = Blend.Zero;

            //Write to stencil buffer
            generateShadow = new BlendState();
            generateShadow.ColorSourceBlend = Blend.Zero;
            generateShadow.ColorDestinationBlend = Blend.One;
            generateShadow.ColorWriteChannels = ColorWriteChannels.None;

            generateShadow2 = new BlendState();
            generateShadow2.ColorBlendFunction = BlendFunction.Min;
            generateShadow2.ColorSourceBlend = Blend.One;
            generateShadow2.ColorDestinationBlend = Blend.One;
            generateShadow2.ColorWriteChannels = ColorWriteChannels.All;

            addLight = new BlendState();
            addLight.ColorBlendFunction = BlendFunction.Add;
            addLight.ColorSourceBlend = Blend.One;
            addLight.ColorDestinationBlend = Blend.One;
            addLight.ColorWriteChannels = ColorWriteChannels.All;

            generateShadowStencil = new DepthStencilState();
            generateShadowStencil.StencilEnable = true;
            generateShadowStencil.DepthBufferEnable = false;
            generateShadowStencil.ReferenceStencil = 1;
            generateShadowStencil.StencilFunction = CompareFunction.Equal;
            generateShadowStencil.StencilPass = StencilOperation.Increment;

            drawShadowedLight = new DepthStencilState();
            drawShadowedLight.DepthBufferEnable = false;
            drawShadowedLight.StencilEnable = true;
            drawShadowedLight.ReferenceStencil = 1;
            drawShadowedLight.StencilFunction = CompareFunction.GreaterEqual;

            disableShadows = new DepthStencilState();
            disableShadows.DepthBufferEnable = false;
            disableShadows.StencilEnable = true;
            disableShadows.StencilFunction = CompareFunction.Always;
            disableShadows.ReferenceStencil = 1;
            disableShadows.StencilPass = StencilOperation.Zero;

            clearStencil = new DepthStencilState();
            clearStencil.StencilEnable = true;
            clearStencil.StencilFunction = CompareFunction.Always;
            clearStencil.ReferenceStencil = 1;
            clearStencil.StencilPass = StencilOperation.Replace;

            if (clearVBuffer == null)
            {
                clearVBuffer = new VertexBuffer(device, VertexPositionColor.VertexDeclaration, 4, BufferUsage.None);
                clearIBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, 6, BufferUsage.None);
                clearIBuffer.SetData<short>(new short[] { 0, 1, 2, 2, 3, 0 });
                clearVBuffer.SetData<VertexPositionColor>(new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(-1,-1,0), new Color(new Vector3(0,0,0))),
                    new VertexPositionColor(new Vector3(1,-1,0), new Color(new Vector3(1,0,0))),
                    new VertexPositionColor(new Vector3(1,1,0), new Color(new Vector3(1,1,0))),
                    new VertexPositionColor(new Vector3(-1,1,0), new Color(new Vector3(0,1,0)))
                });
            }

            if (rotateLeft == Matrix.Identity)
            {
                rotateLeft = Matrix.CreateRotationZ(0 * MathHelper.Pi / 180);
                rotateRight = Matrix.CreateRotationZ(0 * MathHelper.Pi / 180);
            }
            _unusedTexture=new Texture2D(device, 1,1);

            shadowEffect.Parameters["xTexelDist"].SetValue(1.0f / (device.PresentationParameters.BackBufferWidth/lightMapDownSample));
            shadowEffect.Parameters["yTexelDist"].SetValue(1.0f / (device.PresentationParameters.BackBufferHeight/lightMapDownSample));

            _lightMutex = new Mutex();
        }
        public void SetGlow(int id, Vector2 position, Color color, float radius, bool shadows)
        {
            _lightMutex.WaitOne();
            for (int x = 0; x < _glows.Count; x++)
            {
                if (_glows[x].id == id)
                {
                    _glows[x].position = position;
                    _glows[x].radius = radius;
                    _glows[x].color = color;
                    if (radius < 0)
                        _glows.RemoveAt(x);
                    _lightMutex.ReleaseMutex();
                    return;
                }
            }
            LightSource glow = new LightSource();
            glow.id = id;
            glow.position = position;
            glow.radius = radius;
            glow.shadows = shadows;
            glow.color = color;
            _glows.Add(glow);
            _lightMutex.ReleaseMutex();
        }

        public void SetGlow(int id, Vector2 position, Color color, float radius, bool shadows, float timeout)
        {
            _lightMutex.WaitOne();
            for (int x = 0; x < _glows.Count; x++)
            {
                if (_glows[x].id == id)
                {
                    _glows[x].position = position;
                    _glows[x].radius = radius;
                    _glows[x].timeOut = timeout;
                    _glows[x].color = color;
                    if (radius < 0)
                        _glows.RemoveAt(x);
                    _lightMutex.ReleaseMutex();
                    return;
                }
            }
            LightSource glow = new LightSource();
            glow.id = id;
            glow.position = position;
            glow.radius = radius;
            glow.timeOut = timeout;
            glow.timeOutStart = timeout;
            glow.color = color;
            glow.shadows = shadows;
            _glows.Add(glow);
            _lightMutex.ReleaseMutex();
        }


        //Multiplicative blend lightmap over drawn scene
        public void ApplyLight(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.FrontToBack, blendShadow, SamplerState.LinearClamp, DepthStencilState.Default, null);
            batch.Draw(shadowTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, lightMapDownSample, SpriteEffects.None, 0.19f);
            batch.End();
        }

        //Draws the lightmap
        public void DrawLights(float intensity, Camera cam, SpriteBatch batch)
        {
            //Intensity is considered to be between 0 and 1 outside of this routine
            //but we only want values between 0 and 0.5 in here
            intensity /= 2;

            //Set appropriate rendering paramaeters
            shadowEffect.Parameters["View"].SetValue(cam.ViewMatrix);
            shadowEffect.Parameters["Projection"].SetValue(cam.ProjectionMatrix);

            shadowEffect.Parameters["shadowDarkness"].SetValue(0.9f-intensity);
            shadowEffect.Parameters["viewDistance"].SetValue((float)(1 / (0.001f + 0.999 * intensity)));

            //Set graphics device states and render target
            device.DepthStencilState = clearStencil;
            device.RasterizerState = RasterizerState.CullNone;
            device.BlendState = BlendState.Opaque;

            device.SetRenderTarget(shadowTarget);

            //Clear the backbuffer
            shadowEffect.CurrentTechnique = shadowEffect.Techniques["Clear"];
            device.SetVertexBuffer(clearVBuffer);
            device.Indices = clearIBuffer;
            shadowEffect.CurrentTechnique.Passes[0].Apply();
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);

            //Make sure no server messages can change the light around while drawing
            _lightMutex.WaitOne();
            for (int x = 0; x < _glows.Count; x++)
            {
                //Draws a single light, I am a lazy bum so I am assuming for lights with timeout that we run close enough to 60fps
                //to not notice the difference
                DrawLight(cam, _glows[x], map, batch);
                if (_glows[x].timeOut > 0)
                {
                    _glows[x].timeOut -= 16; //Previous comment referring to this
                    _glows[x].brightness = (_glows[x].timeOut / _glows[x].timeOutStart);
                    if (_glows[x].timeOut <= 0)
                    {
                        _glows.RemoveAt(x);
                        x--;
                    }
                }
            }
            _lightMutex.ReleaseMutex();

            //Blur the shadow edges to get softer shadows using 5x5 kernel gaussian blur
            BlurShadowTarget(batch);

            //Disable shadowing collision tiles
            //This must be done so that there are no weird shadows overlapping parts of geometry
            UnShadowTiles(cam,batch,map);

            //Unset rendertarget, we are done calculating the light map
            device.SetRenderTarget(null);
            device.DepthStencilState = DepthStencilState.None;
        }

        void BlurShadowTarget(SpriteBatch batch)
        {
            device.SetRenderTarget(shadowTarget2);
            shadowEffect.CurrentTechnique = shadowEffect.Techniques["ShadowBlurH"];
            batch.Begin(SpriteSortMode.Immediate, null, SamplerState.LinearClamp, DepthStencilState.None, null, shadowEffect);
            batch.Draw(shadowTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, lightMapDownSample, SpriteEffects.None, 0.19f);
            batch.End();
            
            device.SetRenderTarget(shadowTarget);
            shadowEffect.CurrentTechnique = shadowEffect.Techniques["ShadowBlurV"];
            batch.Begin(SpriteSortMode.Immediate, null, SamplerState.LinearClamp, DepthStencilState.None, null, shadowEffect);
            batch.Draw(shadowTarget2, Vector2.Zero, null, Color.White, 0, Vector2.Zero, lightMapDownSample, SpriteEffects.None, 0.19f);
            batch.End();
            
        }


        public void DrawLight(Camera cam, LightSource light, Map map, SpriteBatch batch)
        {
            //Clear stencil buffer
            device.Clear(ClearOptions.Stencil, Color.White, 0, 1);

            if (light.shadows)
            {
                //Build shadow geometry for the light
                int vertexCount = map.CreateShadowGeometry(light.position, new Rectangle((int)light.position.X - cam.ViewSpace.Width / 2, (int)light.position.Y - cam.ViewSpace.Height / 2, cam.ViewSpace.Width, cam.ViewSpace.Height), shadowVB);
                
                //Draw Shadows into stencil buffer
                device.BlendState = generateShadow;
                device.DepthStencilState = generateShadowStencil;
                shadowEffect.CurrentTechnique = shadowEffect.Techniques["Shadow"];
                device.SetVertexBuffer(shadowVB);
                device.Indices = shadowIB;
                shadowEffect.CurrentTechnique.Passes[0].Apply();

                //Check necessary because empty draw calls cause crashes
                if (vertexCount > 0)
                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexCount, 0, 4 * vertexCount / 6);
            }
            //Draw glow over everything that has not been stenciled out
            shadowEffect.CurrentTechnique = shadowEffect.Techniques["Glow"];
            batch.Begin(SpriteSortMode.Immediate, addLight, null, drawShadowedLight, RasterizerState.CullNone, shadowEffect);
            //I need to specify a texture to use this call, but since I use a custom shader it is never used, it might actually be null I am not sure
            batch.Draw(_unusedTexture, new Rectangle((int)(light.position.X - light.radius), (int)(light.position.Y - light.radius), (int)(light.radius * 2), (int)(light.radius * 2)), Color.Lerp(light.color, Color.Black, 1-light.brightness));
            batch.End();
        }

        public void DrawMapOutline(SpriteBatch batch, Map map)
        {

        }

        private void UnShadowTiles(Camera cam, SpriteBatch batch, Map map)
        {
            device.BlendState = generateShadow;
            shadowEffect.CurrentTechnique = shadowEffect.Techniques["Tile"];
            batch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, DepthStencilState.None, RasterizerState.CullNone, shadowEffect);
            map.DrawTileHighlight(cam, batch);
            batch.End();

            batch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, shadowEffect);
            map.DrawForegroundTileHighlight(cam, batch, shadowEffect);
            batch.End();
        }

        public void SetMap(Map map)
        {
            this.map = map;
        }

        public void Clear()
        {
            _lightMutex.WaitOne();
            _glows.Clear();
            _lightMutex.ReleaseMutex();
        }

        public void DrawShadows(Camera cam, SpriteBatch batch, float intensity)
        {
            shadowEffect.CurrentTechnique = shadowEffect.Techniques["Glow"];
            batch.Begin(SpriteSortMode.Immediate, addLight, null, DepthStencilState.None, RasterizerState.CullNone, shadowEffect);
            for (int x = 0; x < _glows.Count; x++)
            {
                batch.Draw(_unusedTexture, new Rectangle((int)(_glows[x].position.X - _glows[x].radius), (int)(_glows[x].position.Y - _glows[x].radius), (int)(_glows[x].radius * 2), (int)(_glows[x].radius * 2)), Color.Gray);
            }
            batch.End();

            device.SetRenderTarget(null);
        }
    }
}
