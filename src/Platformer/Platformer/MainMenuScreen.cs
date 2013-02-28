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
using IGORR.Content;

namespace IGORR.Client
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

        public void Initialize(GraphicsDevice Device, ScreenManager manager)
        {
            _manager=manager;
            _device = Device;
            TextRenderer text = new TextRenderer(ContentInterface.LoadFont("font"), Color.White);
            Skin skin = new Skin(ContentInterface.LoadTexture("ImageMap"), ContentInterface.LoadFile("Map"));
            _gui = new Gui(_gameRef, skin, text);
            Ruminate.GUI.Content.Panel panel = new Ruminate.GUI.Content.Panel(300, 200, 200, 300);
            _gui.AddWidget(panel);
            Ruminate.GUI.Content.Button button = new Ruminate.GUI.Content.Button(350, 230, 50, "Start");
            button.ClickEvent = new WidgetEvent(delegate { _manager.RemoveScreen(this); _gui.RemoveWidget(button); _gui.RemoveWidget(panel); _manager.AddScreen(new GameScreen()); });
            _gui.AddWidget(button);
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
