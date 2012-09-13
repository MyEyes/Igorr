using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Camera cam;
        ObjectManager objectManager;
        ParticleManager pm;
        KeyboardState prevKeyboard;
        GamePadState _prevGamePadState;
        SpriteFont font;
        Effect _spriteEffect;
        LightMap _lightMap;
        bool shadows = true;
        float shadowCountdown;
        bool shadowTurnOn=true;
        Texture2D expBorder;
        Texture2D bar;
        #if WINDOWS
        Player player;
        #elif XBOX
        Player[] players;
        #endif

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Window.Title = "IGORRRR";
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 61f);
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.IsFullScreen = true;
            WorldController.SendText("Start");
            Settings.LoadSettings();
        }

        /// <summary>
        /// Ermöglicht dem Spiel die Durchführung einer Initialisierung, die es benötigt, bevor es ausgeführt werden kann.
        /// Dort kann es erforderliche Dienste abfragen und nicht mit der Grafik
        /// verbundenen Content laden.  Bei Aufruf von base.Initialize werden alle Komponenten aufgezählt
        /// sowie initialisiert.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Fügen Sie Ihre Initialisierungslogik hier hinzu

            base.Initialize();
        }

        /// <summary>
        /// LoadContent wird einmal pro Spiel aufgerufen und ist der Platz, wo
        /// Ihr gesamter Content geladen wird.
        /// </summary>
        protected override void LoadContent()
        {
            // Erstellen Sie einen neuen SpriteBatch, der zum Zeichnen von Texturen verwendet werden kann.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cam = new Camera(new Vector2(520, 440), new Rectangle(0, 0, 800, 600));
            MapManager.LoadMaps(Content,GraphicsDevice);
            //map = MapManager.GetMapByID(0);
            objectManager = new ObjectManager(map, Content);
            _lightMap = new LightMap(GraphicsDevice, Content);
            objectManager.SetLight(_lightMap);
            pm = new ParticleManager(Content);
            TextManager.SetUp(Content.Load<SpriteFont>("font"));
            Player.font = Content.Load<SpriteFont>("font");
            font = Content.Load<SpriteFont>("font");
            bar = Content.Load<Texture2D>("White");
            expBorder = Content.Load<Texture2D>("ExpBar");
            _spriteEffect = Content.Load<Effect>("ShadowEffect");
            _spriteEffect.CurrentTechnique = _spriteEffect.Techniques["Sprite"];
            MusicPlayer.SetContent(Content);
            WorldController.SetObjectManager(objectManager);
            WorldController.SetContent(Content);
            WorldController.SetGame(this);
            WorldController.Start();
            // TODO: Verwenden Sie this.Content, um Ihren Spiel-Content hier zu laden
        }

        public void LoadMap(int id)
        {
            map = MapManager.GetMapByID(id);
            objectManager.SetMap(map);
            _lightMap.SetMap(map);
        }

        /// <summary>
        /// UnloadContent wird einmal pro Spiel aufgerufen und ist der Ort, wo
        /// Ihr gesamter Content entladen wird.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Entladen Sie jeglichen Nicht-ContentManager-Content hier
        }

        /// <summary>
        /// Ermöglicht dem Spiel die Ausführung der Logik, wie zum Beispiel Aktualisierung der Welt,
        /// Überprüfung auf Kollisionen, Erfassung von Eingaben und Abspielen von Ton.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Update(GameTime gameTime)
        {
            if (map != null)
            {
                // Ermöglicht ein Beenden des Spiels
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();

#if WINDOWS
                KeyboardState keyboard = Keyboard.GetState();

                MouseState mouse = Mouse.GetState();
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                player = objectManager.Player;
                if (player != null)
                {
                    if (keyboard.IsKeyDown(Keys.A))
                        player.Move(-1);
                    if (keyboard.IsKeyDown(Keys.D))
                        player.Move(1);
                    if (keyboard.IsKeyDown(Keys.Space) && !prevKeyboard.IsKeyDown(Keys.Space))
                        player.Jump();
                    if (keyboard.IsKeyDown(Keys.LeftControl) && !prevKeyboard.IsKeyDown(Keys.LeftControl))
                        WorldController.SendAttack(1, player.ID);

                    if (keyboard.IsKeyDown(Keys.Left))
                        player.Move(-1);
                    if (keyboard.IsKeyDown(Keys.Up) && !prevKeyboard.IsKeyDown(Keys.Up))
                        player.Jump();
                    if (keyboard.IsKeyDown(Keys.Right))
                        player.Move(1);
                    if (keyboard.IsKeyDown(Keys.Y) && !prevKeyboard.IsKeyDown(Keys.Y))
                        WorldController.SendAttack(1, player.ID);
                    if (keyboard.IsKeyDown(Keys.Z) && !prevKeyboard.IsKeyDown(Keys.Z))
                        WorldController.SendAttack(1, player.ID);
                    if (keyboard.IsKeyDown(Keys.X) && !prevKeyboard.IsKeyDown(Keys.X))
                        WorldController.SendAttack(2, player.ID);
                    if (keyboard.IsKeyDown(Keys.C) && !prevKeyboard.IsKeyDown(Keys.C))
                        WorldController.SendAttack(3, player.ID);
                    /*                
                                    if (keyboard.IsKeyDown(Keys.Q))
                                        cam.ZoomInOn(1.1f, new Vector2(mouse.X, mouse.Y));
                                    if (keyboard.IsKeyDown(Keys.E))
                                        cam.ZoomInOn(1 / 1.1f, new Vector2(mouse.X, mouse.Y));
                     */

                    player.Move(pad.ThumbSticks.Left.X);
                    if (pad.IsButtonDown(Buttons.DPadLeft))
                        player.Move(-1);
                    if (pad.IsButtonDown(Buttons.DPadRight))
                        player.Move(1);
                    if (pad.IsButtonDown(Buttons.A) && !_prevGamePadState.IsButtonDown(Buttons.A))
                        player.Jump();
                    if (pad.IsButtonDown(Buttons.X) && !_prevGamePadState.IsButtonDown(Buttons.X))
                        WorldController.SendAttack(1, player.ID);
                    if (pad.IsButtonDown(Buttons.B) && !_prevGamePadState.IsButtonDown(Buttons.B))
                        WorldController.SendAttack(2, player.ID);
                    if (pad.IsButtonDown(Buttons.Y) && !_prevGamePadState.IsButtonDown(Buttons.Y))
                        WorldController.SendAttack(3, player.ID);
                    //Test stuff
                    if (keyboard.IsKeyDown(Keys.T) && !prevKeyboard.IsKeyDown(Keys.T))
                        pm.Boom(Light,player.MidPosition);
                    WorldController.SendPosition(player);
                    objectManager.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
                    cam.SetPos(player.Position);
                    _lightMap.SetGlow(-1, player.MidPosition, 100, true);
                }

                if (shadowTurnOn)
                {
                    shadows = true;
                    if (shadowCountdown > 2)
                    {
                        shadowCountdown = 2;
                    }
                    else if(shadowCountdown<2)
                        shadowCountdown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    if (shadowCountdown < 0)
                    {
                        shadows = false;
                        shadowCountdown = 0;
                    }
                    else if(shadowCountdown>0)
                        shadowCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }


                map.Update(gameTime.ElapsedGameTime.Milliseconds, cam);
                if (player != null && player.HP <= 0)
                {
                    //Point startPos = map.getRandomSpawn();
                    //player = new Player(Content.Load<Texture2D>("blob"), new Rectangle(startPos.X, startPos.Y, 16, 15));
                }
                pm.Update(map, (float)gameTime.ElapsedGameTime.TotalSeconds);
#elif XBOX
            Vector2 center = Vector2.Zero;
            for(int x=0; x<players.Length; x++)
            {
                GamePadState gamepad = GamePad.GetState((PlayerIndex)x);
                players[x].Move(gamepad.ThumbSticks.Left.X);
                if (gamepad.IsButtonDown(Buttons.A))
                    players[x].Jump();
                center+=players[x].MidPosition;
                players[x].Update(map, (float)gameTime.ElapsedGameTime.TotalSeconds);
                if(players[x].HP<0)
                {
                    Point startPos = map.getRandomSpawn();
                    players[x] = new Player(Content.Load<Texture2D>("blob"), new Rectangle(startPos.X, startPos.Y, 16, 16));    
                }
            }
            center /= players.Length;
            cam.SetPos(center );
#endif

                TextManager.Update(gameTime.ElapsedGameTime.Milliseconds);
                prevKeyboard = keyboard;
                _prevGamePadState = pad;
                if (this.IsActive)
                    MusicPlayer.Update(gameTime.ElapsedGameTime.Milliseconds);
                // TODO: Fügen Sie Ihre Aktualisierungslogik hier hinzu
            }
            IGORRProtocol.Protocol.Update(gameTime.ElapsedGameTime.Milliseconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// Dies wird aufgerufen, wenn das Spiel selbst zeichnen soll.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (map != null)
            {
                if (shadows)
                    _lightMap.DrawLights(shadowCountdown, cam, spriteBatch);
                GraphicsDevice.Clear(Color.Black);
                _spriteEffect.Parameters["View"].SetValue(cam.ViewMatrix);
                _spriteEffect.Parameters["Projection"].SetValue(cam.ProjectionMatrix);
                _spriteEffect.CurrentTechnique = _spriteEffect.Techniques["Sprite"];
                spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, DepthStencilState.Default, null,_spriteEffect, cam.ViewMatrix);
                map.Draw(spriteBatch, cam);

                objectManager.Draw(spriteBatch);
                pm.Draw(spriteBatch);
                TextManager.Draw(spriteBatch);
                spriteBatch.End();
                if (shadows)
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
                    spriteBatch.DrawString(font, "Not connected", new Vector2(50, 50), Color.White);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        void DrawExpBar()
        {
            if (player == null)
                return;
            int sizeX = GraphicsDevice.Viewport.Width;
            int pixels = (int)(sizeX * (player.Exp - player.LastLevelExp) / ((float)(player.NextLevelExp - player.LastLevelExp)));
            spriteBatch.Draw(bar, new Rectangle(0, 600-16, pixels, 16), Color.Orange);
            spriteBatch.Draw(bar, new Rectangle(pixels, 600-16, sizeX - pixels, 16), Color.White);
            spriteBatch.Draw(expBorder, new Rectangle(0, 600-16, sizeX, 16), Color.White);
            if (font != null)
                spriteBatch.DrawString(font, (player.Exp - player.LastLevelExp).ToString() + "/" + (player.NextLevelExp - player.LastLevelExp).ToString(), new Vector2(sizeX/2-50, 600-16-20), Color.White, 0, Vector2.Zero, 0.45f, SpriteEffects.None, 0.5f);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            WorldController.Leave();
            WorldController.Exit();
            base.OnExiting(sender, args);
        }

        public Map Map
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
