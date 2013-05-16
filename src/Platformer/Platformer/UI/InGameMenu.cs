using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Content;

namespace IGORR.Client.UI
{
    class InGameMenu : UIWindow
    {
        public InGameMenu(Vector2 position, Vector2 size, Game game)
            : base(position, size, ContentInterface.LoadTexture("UITest"), ContentInterface.LoadTexture("UITest"))
        {
            Button button = new Button(this, new Vector2(60, 210), new Vector2(80, 40), Content.ContentInterface.LoadTexture("UITest"),
    delegate { game.Exit(); },
    "Exit");
            AddChild(button);
            button = new Button(this, new Vector2(20, 30), new Vector2(160, 40), Content.ContentInterface.LoadTexture("UITest"),
    delegate { Console.WriteLine("Test1"); },
    "Testbutton1");
            AddChild(button);
            button = new Button(this, new Vector2(20, 90), new Vector2(160, 40), Content.ContentInterface.LoadTexture("UITest"),
delegate { Console.WriteLine("Test2"); },
"Testbutton2");
            AddChild(button);
            button = new Button(this, new Vector2(20, 150), new Vector2(160, 40), Content.ContentInterface.LoadTexture("UITest"),
delegate { Console.WriteLine("Test3"); },
"Testbutton3");
            AddChild(button);
        }
    }
}
