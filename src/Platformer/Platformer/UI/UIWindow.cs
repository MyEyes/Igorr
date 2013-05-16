using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.UI
{
    class UIWindow:UIElement
    {
        const int topSize = 16;
        Panel _topElement;
        Panel _mainWindow;
        Vector2 _size;

        MouseState _prevState;
        bool _dragged = false;

        public UIWindow(Vector2 offset, Vector2 size, Texture2D top, Texture2D main)
            : base(null, offset)
        {
            _size = size;
            _topElement = new Panel(this, new Vector2(0, -topSize), new Vector2(size.X, topSize), top);
            _mainWindow = new Panel(this, Vector2.Zero, new Vector2(size.X, size.Y), top);
            AddChild(_topElement);
            AddChild(_mainWindow);
            AddChild(new Button(this, new Vector2(_size.X - topSize, -topSize), new Vector2(topSize, topSize), top, delegate { Console.WriteLine("clicked"); _parent.RemoveChild(this); }, "X"));
        }

        public override void Update(float ms)
        {
            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && _prevState.LeftButton == ButtonState.Released
                && new Rectangle((int)_topElement.TotalOffset.X, (int)_topElement.TotalOffset.Y, (int)_size.X, topSize).Contains(mouse.X, mouse.Y))
            {
                _dragged = true;
            }

            if (_dragged && mouse.LeftButton == ButtonState.Released)
                _dragged = false;

            if (_dragged)
                this.SetOffset(Offset + new Vector2(mouse.X - _prevState.X, mouse.Y - _prevState.Y));
            UpdateChildren(ms);
            _prevState = mouse;
        }

        public override void Draw(SpriteBatch batch)
        {
            DrawChildren(batch);
        }
    }
}
