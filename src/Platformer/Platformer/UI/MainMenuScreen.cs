using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.UI
{
    class MainMenuScreen : UIScreen
    {
        public override void Initialize(GraphicsDevice Device, ScreenManager manager)
        {
            base.Initialize(Device, manager);
            Settings.LoadSettings();
            Panel panel = new Panel(this, new Vector2(300, 160), new Vector2(200, 280), Content.ContentInterface.LoadTexture("UITest"));
            Button button = new Button(this, new Vector2(60, 30), new Vector2(80, 40), Content.ContentInterface.LoadTexture("UITest"),
                delegate { Settings.ServerAddress = "localhost";
                    _manager.RemoveScreen(this); GameScreen screen = new GameScreen();
                    Server.Server server = new Server.Server(); screen.Server = server; _manager.AddScreen(new GameScreen()); },
                "Local");
            AddChild(panel);
            panel.AddChild(button);
            button = new Button(this, new Vector2(20, 90), new Vector2(160, 40), Content.ContentInterface.LoadTexture("UITest"),
    delegate { _manager.RemoveScreen(this); _manager.AddScreen(new GameScreen()); },
    "Online");
            panel.AddChild(button);
            button = new Button(this, new Vector2(20, 150), new Vector2(160, 40), Content.ContentInterface.LoadTexture("UITest"),
delegate { Console.WriteLine("Test2"); },
"Testbutton2");
            panel.AddChild(button);
            button = new Button(this, new Vector2(20, 210), new Vector2(160, 40), Content.ContentInterface.LoadTexture("UITest"),
delegate { Console.WriteLine("Test3"); },
"Testbutton3");
            panel.AddChild(button);
        }
    }
}

