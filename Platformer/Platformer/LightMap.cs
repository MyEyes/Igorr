﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    public class LightSource
    {
        public Vector2 position;
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

        static VertexBuffer clearVBuffer;
        static IndexBuffer clearIBuffer;
        public static Matrix rotateLeft = Matrix.Identity;
        public static Matrix rotateRight = Matrix.Identity;
        VertexBuffer shadowVB;
        IndexBuffer shadowIB;
        RenderTarget2D shadowTarget;
        BlendState blendShadow;
        BlendState generateShadow;
        BlendState generateShadow2;
        BlendState generateShadow3;
        DepthStencilState generateShadowStencil;
        DepthStencilState drawShadowedLight;
        DepthStencilState disableShadows;
        DepthStencilState clearStencil;
        Effect shadowEffect;

        Texture2D _unusedTexture;

        GraphicsDevice device;

        const int maxX = 30;
        const int maxY = 30;

        public LightMap(GraphicsDevice device, ContentManager content)
        {
            this.device = device;
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
            shadowEffect = content.Load<Effect>("ShadowEffect");
            shadowTarget = new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 1, RenderTargetUsage.DiscardContents);
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

            generateShadow3 = new BlendState();
            generateShadow3.ColorBlendFunction = BlendFunction.Add;
            generateShadow3.ColorSourceBlend = Blend.One;
            generateShadow3.ColorDestinationBlend = Blend.One;
            generateShadow3.ColorWriteChannels = ColorWriteChannels.All;

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

            _lightMutex = new Mutex();
        }
        public void SetGlow(int id, Vector2 position, float radius, bool shadows)
        {
            _lightMutex.WaitOne();
            for (int x = 0; x < _glows.Count; x++)
            {
                if (_glows[x].id == id)
                {
                    _glows[x].position = position;
                    _glows[x].radius = radius;
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
            _glows.Add(glow);
            _lightMutex.ReleaseMutex();
        }

        public void SetGlow(int id, Vector2 position, float radius,bool shadows, float timeout)
        {
            _lightMutex.WaitOne();
            for (int x = 0; x < _glows.Count; x++)
            {
                if (_glows[x].id == id)
                {
                    _glows[x].position = position;
                    _glows[x].radius = radius;
                    _glows[x].timeOut = timeout;
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
            glow.shadows = shadows;
            _glows.Add(glow);
            _lightMutex.ReleaseMutex();
        }

        public void ApplyLight(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.FrontToBack, blendShadow, null, DepthStencilState.Default, null);
            batch.Draw(shadowTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.19f);
            batch.End();
        }

        public void DrawLights(float intensity, Camera cam, SpriteBatch batch)
        {
            intensity /= 2;

            shadowEffect.Parameters["View"].SetValue(cam.ViewMatrix);
            shadowEffect.Parameters["Projection"].SetValue(cam.ProjectionMatrix);

            shadowEffect.Parameters["shadowDarkness"].SetValue(1-intensity);
            shadowEffect.Parameters["viewDistance"].SetValue((float)(1 / (0.001f + 0.999 * intensity)));

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
            _lightMutex.WaitOne();
            for (int x = 0; x < _glows.Count; x++)
            {
                DrawLight(cam, _glows[x], map, batch);
                if (_glows[x].timeOut > 0)
                {
                    _glows[x].timeOut -= 16;
                    _glows[x].brightness = (_glows[x].timeOut / _glows[x].timeOutStart);
                    if (_glows[x].timeOut <= 0)
                    {
                        _glows.RemoveAt(x);
                        x--;
                    }
                }
            }
            _lightMutex.ReleaseMutex();
            device.SetRenderTarget(null);
            device.DepthStencilState = DepthStencilState.None;
        }

        public void DrawLight(Camera cam, LightSource light, Map map, SpriteBatch batch)
        {
            //Clear stencil buffer
            device.Clear(ClearOptions.Stencil, Color.White, 0, 1);

            if (light.shadows)
            {
                //Build shadow geometry for the light
                int vertexCount = map.CreateShadowGeometry(light.position, cam.ViewSpace, shadowVB);
                
                //Disable shadowing collision tiles
                shadowEffect.CurrentTechnique = shadowEffect.Techniques["Tile"];
                shadowEffect.CurrentTechnique.Passes[0].Apply();
                batch.Begin(SpriteSortMode.Immediate, generateShadow, null, disableShadows, RasterizerState.CullNone, shadowEffect);
                map.DrawTileHighlight(cam, batch);
                batch.End();
                //device.Clear(ClearOptions.Stencil, Color.White, 0, 0);
                //Draw Shadows
                device.BlendState = generateShadow;
                device.DepthStencilState = generateShadowStencil;
                shadowEffect.CurrentTechnique = shadowEffect.Techniques["Shadow"];
                device.SetVertexBuffer(shadowVB);
                device.Indices = shadowIB;
                shadowEffect.CurrentTechnique.Passes[0].Apply();

                if (vertexCount > 0)
                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexCount, 0, 4 * vertexCount / 6);
            }
            shadowEffect.CurrentTechnique = shadowEffect.Techniques["Glow"];
            batch.Begin(SpriteSortMode.Immediate, generateShadow3, null, drawShadowedLight, RasterizerState.CullNone, shadowEffect);
            batch.Draw(_unusedTexture, new Rectangle((int)(light.position.X - light.radius), (int)(light.position.Y - light.radius), (int)(light.radius * 2), (int)(light.radius * 2)), Color.Lerp(Color.White, Color.Black, 1-light.brightness));
            batch.End();

        }

        public void DrawMapOutline(SpriteBatch batch, Map map)
        {

        }

        private void UnShadowTiles(Camera cam, SpriteBatch batch, Map map)
        {
            device.BlendState = generateShadow;
            shadowEffect.CurrentTechnique = shadowEffect.Techniques["Tile"];
            batch.Begin(SpriteSortMode.Immediate, generateShadow, null, DepthStencilState.None, RasterizerState.CullNone, shadowEffect);
            map.DrawTileHighlight(cam, batch);
            batch.End();

            batch.Begin(SpriteSortMode.Immediate, generateShadow, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, shadowEffect);
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
            batch.Begin(SpriteSortMode.Immediate, generateShadow3, null, DepthStencilState.None, RasterizerState.CullNone, shadowEffect);
            for (int x = 0; x < _glows.Count; x++)
            {
                batch.Draw(_unusedTexture, new Rectangle((int)(_glows[x].position.X - _glows[x].radius), (int)(_glows[x].position.Y - _glows[x].radius), (int)(_glows[x].radius * 2), (int)(_glows[x].radius * 2)), Color.Gray);
            }
            batch.End();

            device.SetRenderTarget(null);
        }
    }
}
