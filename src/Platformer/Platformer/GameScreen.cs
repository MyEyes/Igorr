using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using IGORR.Content;
using IGORR.Client.Logic;


//TODO: Hide mouse cursor after a while of inactivity
namespace IGORR.Client
{
    class GameScreen : IScreen
    {
        SpriteBatch spriteBatch;
        Map map;
        Camera cam;
        ObjectManager objectManager;
        ParticleManager pm;
        KeyboardState _prevKeyboard;
        GamePadState _prevGamePadState;
        MouseState _prevMouseState;
        SpriteFont font;
        Effect _spriteEffect;
        LightMap _lightMap;
        bool shadows = true;
        float shadowCountdown;
        bool shadowTurnOn = true;
        Texture2D expBorder;
        Texture2D bar;
        Texture2D crosshair;
        GraphicsDevice GraphicsDevice;

        Vector2 _mouseDir = Vector2.Zero;
        #if WINDOWS
        Player player;
        #elif XBOX
        Player[] players;
        #endif

        public void Initialize(GraphicsDevice Device, ScreenManager manager)
        {
            GraphicsDevice = Device;
            spriteBatch = new SpriteBatch(Device);
            cam = new Camera(new Vector2(520, 440), new Rectangle(0, 0, 800, 600));
            MapManager.LoadMaps(Device);
            //map = MapManager.GetMapByID(0);
            objectManager = new ObjectManager(map);
            _lightMap = new LightMap(Device);
            objectManager.SetLight(_lightMap);
            pm = new ParticleManager();
            TextManager.SetUp(ContentInterface.LoadFont("font"));
            Player.font = ContentInterface.LoadFont("font");
            font = ContentInterface.LoadFont("font");
            bar = ContentInterface.LoadTexture("White");
            expBorder = ContentInterface.LoadTexture("ExpBar");
            crosshair = ContentInterface.LoadTexture("Crosshair");
            _spriteEffect = ContentInterface.LoadShader("ShadowEffect");
            _spriteEffect.CurrentTechnique = _spriteEffect.Techniques["Sprite"];
            WorldController.SetObjectManager(objectManager);
            WorldController.SetGame(this);
            WorldController.Start();
        }

        public void LoadMap(int id)
        {
            map = MapManager.GetMapByID(id);
            objectManager.SetMap(map);
            _lightMap.SetMap(map);
            cam.JumpNext();
        }



        public void Draw(GameTime gameTime)
        {
            if (map != null && player!=null)
            {
                _lightMap.DrawLights(shadowCountdown, cam, spriteBatch);
                GraphicsDevice.Clear(Color.Black);
                _spriteEffect.Parameters["View"].SetValue(cam.ViewMatrix);
                _spriteEffect.Parameters["Projection"].SetValue(cam.ProjectionMatrix);
                _spriteEffect.CurrentTechnique = _spriteEffect.Techniques["Sprite"];
                spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, DepthStencilState.Default, null, _spriteEffect, cam.ViewMatrix);
                map.Draw(spriteBatch, cam);

                objectManager.Draw(spriteBatch);
                spriteBatch.Draw(crosshair, player.MidPosition, null, Color.White, (float)Math.Atan2(_mouseDir.Y, _mouseDir.X), new Vector2(-32, 16),0.5f, SpriteEffects.None, 0.1f);
                pm.Draw(spriteBatch);
                TextManager.Draw(spriteBatch);
                spriteBatch.End();
                _lightMap.ApplyLight(spriteBatch);

                spriteBatch.Begin();
                DrawExpBar();
                TextManager.DrawInfo(spriteBatch);
                spriteBatch.End();
                // TODO: Fügen Sie Ihren Zeichnungscode hier hinzu
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();
                if (font != null)
                    spriteBatch.DrawString(font, "Loading", new Vector2(50, 50), Color.White);
                spriteBatch.End();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (map != null)
            {

                KeyboardState keyboard = Keyboard.GetState();

                MouseState mouse = Mouse.GetState();
                if (player != null && (mouse.X != _prevMouseState.X || mouse.Y != _prevMouseState.Y))
                {
                    _mouseDir = Vector2.Zero;
                    _mouseDir = cam.ViewToWorldPosition(new Vector2(mouse.X, mouse.Y)) - player.MidPosition;
                }
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                player = objectManager.Player;
                if (player != null)
                {
                    if (keyboard.IsKeyDown(Keys.A))
                        player.Move(-1);
                    if (keyboard.IsKeyDown(Keys.D))
                        player.Move(1);
                    if (keyboard.IsKeyDown(Keys.Space) && !_prevKeyboard.IsKeyDown(Keys.Space))
                        player.Jump();
                    if (keyboard.IsKeyDown(Keys.LeftControl) && !_prevKeyboard.IsKeyDown(Keys.LeftControl))
                        WorldController.SendAttack(0,_mouseDir, player.ID);

                    if (keyboard.IsKeyDown(Keys.Left))
                        player.Move(-1);
                    if (keyboard.IsKeyDown(Keys.Up) && !_prevKeyboard.IsKeyDown(Keys.Up))
                        player.Jump();
                    if (keyboard.IsKeyDown(Keys.Right))
                        player.Move(1);
                    if (keyboard.IsKeyDown(Keys.Y) && !_prevKeyboard.IsKeyDown(Keys.Y))
                        WorldController.SendAttack(0, _mouseDir, player.ID);
                    if (keyboard.IsKeyDown(Keys.Z) && !_prevKeyboard.IsKeyDown(Keys.Z))
                        WorldController.SendAttack(0, _mouseDir, player.ID);
                    if (keyboard.IsKeyDown(Keys.X) && !_prevKeyboard.IsKeyDown(Keys.X))
                        WorldController.SendAttack(1, _mouseDir, player.ID);
                    if (keyboard.IsKeyDown(Keys.C) && !_prevKeyboard.IsKeyDown(Keys.C))
                        WorldController.SendAttack(2, _mouseDir, player.ID);
                    /*                
                                    if (keyboard.IsKeyDown(Keys.Q))
                                        cam.ZoomInOn(1.1f, new Vector2(mouse.X, mouse.Y));
                                    if (keyboard.IsKeyDown(Keys.E))
                                        cam.ZoomInOn(1 / 1.1f, new Vector2(mouse.X, mouse.Y));
                     */
                    if (pad.ThumbSticks.Right.LengthSquared() > 0.2f)
                    {
                        _mouseDir = pad.ThumbSticks.Right;
                        _mouseDir.Y = -_mouseDir.Y;
                    }

                    player.Move(pad.ThumbSticks.Left.X);
                    if (pad.IsButtonDown(Buttons.DPadLeft))
                        player.Move(-1);
                    if (pad.IsButtonDown(Buttons.DPadRight))
                        player.Move(1);
                    if (pad.IsButtonDown(Buttons.A) && !_prevGamePadState.IsButtonDown(Buttons.A))
                        player.Jump();
                    if (pad.IsButtonDown(Buttons.X) && !_prevGamePadState.IsButtonDown(Buttons.X))
                        WorldController.SendAttack(0, _mouseDir, player.ID);
                    if (pad.IsButtonDown(Buttons.B) && !_prevGamePadState.IsButtonDown(Buttons.B))
                        WorldController.SendAttack(1, _mouseDir, player.ID);
                    if (pad.IsButtonDown(Buttons.Y) && !_prevGamePadState.IsButtonDown(Buttons.Y))
                        WorldController.SendAttack(2, _mouseDir, player.ID);
                    //Test stuff
                    WorldController.SendPosition(player);
                    objectManager.Update((float)gameTime.ElapsedGameTime.Milliseconds);
                    cam.MoveTo(player.Position , 0.1f);
                    //cam.SetPos(player.Position);
                    _lightMap.SetGlow(-1, player.MidPosition, Color.White, shadowCountdown * 50, true);
                }

                if (shadowTurnOn)
                {
                    shadows = true;
                    if (shadowCountdown > 2)
                    {
                        shadowCountdown = 2;
                    }
                    else if (shadowCountdown < 2)
                        shadowCountdown += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                }
                else
                {
                    if (shadowCountdown < 0)
                    {
                        shadows = false;
                        shadowCountdown = 0;
                    }
                    else if (shadowCountdown > 0)
                        shadowCountdown -= gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                }


                map.Update(gameTime.ElapsedGameTime.Milliseconds, cam);
                if (player != null && player.HP <= 0)
                {
                    //Point startPos = map.getRandomSpawn();
                    //player = new Player(Content.Load<Texture2D>("blob"), new Rectangle(startPos.X, startPos.Y, 16, 15));
                }
                pm.Update(map, gameTime.ElapsedGameTime.Milliseconds / 1000.0f);


                TextManager.Update(gameTime.ElapsedGameTime.Milliseconds);
                _prevKeyboard = keyboard;
                _prevGamePadState = pad;
                _prevMouseState = mouse;
                // TODO: Fügen Sie Ihre Aktualisierungslogik hier hinzu
            }
            IGORR.Protocol.ProtocolHelper.Update((int)gameTime.ElapsedGameTime.Milliseconds);
        }

        void DrawExpBar()
        {
            if (player == null)
                return;
            int sizeX = GraphicsDevice.Viewport.Width;
            int pixels = (int)(sizeX * (player.Exp - player.LastLevelExp) / ((float)(player.NextLevelExp - player.LastLevelExp)));
            spriteBatch.Draw(bar, new Rectangle(0, 600 - 16, pixels, 16), Color.Orange);
            spriteBatch.Draw(bar, new Rectangle(pixels, 600 - 16, sizeX - pixels, 16), Color.White);
            spriteBatch.Draw(expBorder, new Rectangle(0, 600 - 16, sizeX, 16), Color.White);
            if (font != null)
                spriteBatch.DrawString(font, (player.Exp - player.LastLevelExp).ToString() + "/" + (player.NextLevelExp - player.LastLevelExp).ToString(), new Vector2(sizeX / 2 - 50, 600 - 16 - 20), Color.White, 0, Vector2.Zero, 0.45f, SpriteEffects.None, 0.5f);
        }

        public IMap Map
        {
            get { return map; }
        }

        public ParticleManager Particles
        {
            get { return pm; }
        }

        public Camera Cam
        {
            get { return cam; }
        }

        public bool Shadows
        {
            get { return shadows; }
            set
            {
                if (shadowTurnOn != value)
                {
                    shadowTurnOn = value;
                }
            }
        }

        public LightMap Light
        {
            get { return _lightMap; }
        }
    }
}
