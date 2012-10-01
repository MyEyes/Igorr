using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI;
using Ruminate.Utils;
using Ruminate.GUI.Framework;

namespace IGORR.Game
{
    class MainMenuScreen : IScreen
    {
        GraphicsDevice _device;
        Game1 _gameRef;
        Gui _gui;
        ScreenManager _manager;

        public MainMenuScreen(Game1 game)
        {
            _gameRef = game;
        }

        public void Initialize(ContentManager Content, GraphicsDevice Device, ScreenManager manager)
        {
            _manager=manager;
            _device = Device;
            TextRenderer text = new TextRenderer(Content.Load<SpriteFont>("font"), Color.White);
            Skin skin = new Skin(Content.Load<Texture2D>("ImageMap"), Content.Load<string>("Map"));
            _gui = new Gui(_gameRef, skin, text);
            _gui.AddWidget(new Ruminate.GUI.Content.Panel(300, 200, 200, 300));
            _gui.AddWidget(new Ruminate.GUI.Content.Button(350, 230, 50, "Start",
                new WidgetEvent(delegate { _manager.RemoveScreen(this); _manager.AddScreen(new GameScreen()); })));
        }

        public void Update(GameTime gameTime)
        {
            _gui.Update();
        }

        public void Draw(GameTime gameTime)
        {
            _gui.Draw();
        }
    }
}
