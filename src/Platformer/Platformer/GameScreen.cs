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

        InputManager input;
        SpriteFont font;
        Effect _spriteEffect;
        LightMap _lightMap;
        bool shadows = true;
        float shadowCountdown;
        bool shadowTurnOn = true;
        bool mapChanged = false;
        Texture2D expBorder;
        Texture2D bar;
        Texture2D crosshair;
        GraphicsDevice GraphicsDevice;
        ScreenManager _manager;

        UI.GUIScreen _GUIOverlay;

        System.Threading.Mutex _mapMutex;

        Vector2 _mouseDir = Vector2.Zero;
        Player player;
        

        public void Initialize(GraphicsDevice Device, ScreenManager manager)
        {
            GraphicsDevice = Device;
            spriteBatch = new SpriteBatch(Device);
            cam = new Camera(new Vector2(520, 440), new Rectangle(0, 0, 800, 600));

            _lightMap = new LightMap(Device);

            MapManager.LoadMaps(Device);
            //map = MapManager.GetMapByID(0);
            objectManager = new ObjectManager(map);
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
            _manager = manager;

            _mapMutex = new System.Threading.Mutex();

            input = new InputManager();

            WorldController.SetObjectManager(objectManager);
            WorldController.SetGame(this);
            WorldController.Start();
            _GUIOverlay = new UI.GUIScreen();
            manager.AddScreen(_GUIOverlay);
        }

        public void LoadMap(int id)
        {
            _mapMutex.WaitOne();
            map = null;
            player = null;
            Map NewMap = MapManager.GetMapByID(id);
            objectManager.SetMap(NewMap);
            _lightMap.SetMap(NewMap);
            cam.JumpNext();
            map = NewMap;
            mapChanged = true;
            _mapMutex.ReleaseMutex();
        }

        private void DrawInteractMarker()
        {
            GameObject interactObject = objectManager.GetObjectInteract(player.MidPosition, 32);
            if (interactObject != null)
            {
                //spriteBatch.Draw();
            }
        }


        public void Draw(GameTime gameTime)
        {
            Camera.CurrentCam = cam;
            _mapMutex.WaitOne();
            if (map != null && player!=null)
            {
                if (!_lightMap.HasLightmap)
                    _lightMap.ComputeLightMap(spriteBatch, map, "Content\\gfx\\lightmaps\\"+map.Name+".png");
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

                spriteBatch.End();
                _lightMap.ApplyLight(spriteBatch);

                TextManager.Draw(spriteBatch, cam);
                spriteBatch.Begin();
                DrawExpBar();
                DrawCurrentAttacks();
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
            _mapMutex.ReleaseMutex();
        }

        public void Update(GameTime gameTime)
        {
            if (!WorldController.Connected)
            {
                _manager.RemoveScreen(this);
            }
            _mapMutex.WaitOne();
            if (map != null)
            {
                input.Update();
                KeyboardState keyboard = Keyboard.GetState();

                MouseState mouse = Mouse.GetState();
                if (player != null && (mouse.X != _prevMouseState.X || mouse.Y != _prevMouseState.Y))
                {
                    _mouseDir = Vector2.Zero;
                    _mouseDir = cam.ViewToWorldPosition(new Vector2(mouse.X, mouse.Y)) - player.MidPosition;
                }
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                Player = objectManager.Player;
                if (player != null)
                {
                    if (input.isActive(Actions.Inventory))
                        _GUIOverlay.ToggleInventoryWindow();
                    if (input.isActive(Actions.Character))
                        _GUIOverlay.ToggleCharacterWindow();
                    if (pad.ThumbSticks.Right.LengthSquared() > 0.2f)
                    {
                        _mouseDir = pad.ThumbSticks.Right;
                        _mouseDir.Y = -_mouseDir.Y;
                    }

                    if (input.isActive(Actions.Attack1))
                        WorldController.SendAttack(0, _mouseDir, player.ID);
                    if (input.isActive(Actions.Attack2))
                        WorldController.SendAttack(1, _mouseDir, player.ID);
                    if (input.isActive(Actions.Attack3))
                        WorldController.SendAttack(2, _mouseDir, player.ID);
                    if (input.isActive(Actions.Attack4))
                        WorldController.SendAttack(3, _mouseDir, player.ID);

                    if (input.Jump)
                        player.Jump();
                    player.Move(input.Direction, input.yDirection);

                    GameObject interactObject = objectManager.GetObjectInteract(player.MidPosition, 32);
                    if (interactObject != null && input.isActive(Actions.Interact))
                    {
                        IGORR.Protocol.Messages.InteractMessage im = (IGORR.Protocol.Messages.InteractMessage)WorldController.ProtocolHelper.NewMessage(Protocol.MessageTypes.Interact);
                        im.objectID = interactObject.ID;
                        im.info = 0;
                        im.sinfo = "";
                        im.action = Protocol.Messages.InteractAction.StartInteract;
                        im.Encode();
                        WorldController.SendReliable(im);
                    }
                    //Test stuff
                    if (mapChanged)
                        player.ChangedMovement = true;
                    WorldController.SendPosition(player);
                    objectManager.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
                    cam.MoveTo(player.Position , 1);
                    mapChanged = false;
                    //cam.SetPos(player.Position);
                    //_lightMap.SetGlow(-1, player.MidPosition, Color.White, 150, true);
                }

                if (shadowTurnOn)
                {
                    shadows = true;
                    if (shadowCountdown > 2)
                    {
                        shadowCountdown = 2;
                    }
                    else if (shadowCountdown < 2)
                        shadowCountdown += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                }
                else
                {
                    if (shadowCountdown < 0)
                    {
                        shadows = false;
                        shadowCountdown = 0;
                    }
                    else if (shadowCountdown > 0)
                        shadowCountdown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                }


                map.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds, cam);
                if (player != null && player.HP <= 0)
                {
                    //Point startPos = map.getRandomSpawn();
                    //player = new Player(Content.Load<Texture2D>("blob"), new Rectangle(startPos.X, startPos.Y, 16, 15));
                }
                pm.Update(map, (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);


                TextManager.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
                _prevKeyboard = keyboard;
                _prevGamePadState = pad;
                _prevMouseState = mouse;
                // TODO: Fügen Sie Ihre Aktualisierungslogik hier hinzu
            }
            _mapMutex.ReleaseMutex();
            if (map != null)
            {
                if (gameTime.ElapsedGameTime.TotalMilliseconds < 0)
                    Console.WriteLine("Weird shit");
                map.ProtocolHelper.Update((int)(float)gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        public void SetupLocalServer()
        {
            Server = new Server.Server();
        }

        public void OnRemove()
        {
            if (Server != null)
                Server.Exit();

            IGORR.Server.Management.LoginData.Close();
            IGORR.Server.Management.ClientInfoInterface.StoreInfo();

            _manager.RemoveScreen(_GUIOverlay);
            _manager.AddScreen(new UI.MainMenuScreen());
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

        void DrawCurrentAttacks()
        {
            int left = (int)input.LeftAttack-(int)Actions.Attack1;
            int right = (int)input.RightAttack-(int)Actions.Attack1;
            if (player.Body.Attacks[left] != null)
                spriteBatch.Draw(player.Body.Attacks[left].Texture, new Rectangle(360, 4, 16, 16), Color.White);
            if (player.Body.Attacks[right] != null)
                spriteBatch.Draw(player.Body.Attacks[right].Texture, new Rectangle(440, 4, 16, 16), Color.White);
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

        public UI.GUIScreen GUI
        {
            get { return _GUIOverlay; }
        }

        public Player Player
        {
            get { return player; }
            set { if (value == null || (player != null && player == value)) return; player = value; _GUIOverlay.SetPlayer(player); }
        }

        public Server.Server Server
        {
            get;
            set;
        }
    }
}
