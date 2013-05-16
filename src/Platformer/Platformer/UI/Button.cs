using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Content;

namespace IGORR.Client.UI
{
    enum UIButtonState
    {
        Nothing,
        Hover,
        Clicked
    }
    
    class Button:UIElement
    {
        Panel buttonBorder;
        Action _action;
        MouseState _prevState;
        Vector2 _size;
        Vector2 _textOffset;

        string _text;
        static SpriteFont _font;
        UIButtonState _state;

        public Button(UIElement parent, Vector2 position, Vector2 size, Texture2D buttonTex, Action action, string text="")
            : base(parent, position)
        {
            buttonBorder = new Panel(this, Vector2.Zero, size, buttonTex);
            this.AddChild(buttonBorder);
            _action=action;
            _size = size;
            if (_font == null)
                _font = ContentInterface.LoadFont("font2");
            Text = text;
            _state = UIButtonState.Nothing;
        }

        public override void Update(float ms)
        {
            base.Update(ms);
            MouseState mouse = Mouse.GetState();
            if (!(mouse.LeftButton == ButtonState.Pressed && _state == UIButtonState.Clicked))
            {
                if (_state == UIButtonState.Clicked)
                {
                    _action.Invoke();
                }
                _state = UIButtonState.Nothing;
                if (new Rectangle((int)buttonBorder.TotalOffset.X, (int)buttonBorder.TotalOffset.Y, (int)_size.X, (int)_size.Y).Contains(mouse.X, mouse.Y))
                {
                    _state = UIButtonState.Hover;
                    if (mouse.LeftButton == ButtonState.Pressed && _prevState.LeftButton == ButtonState.Released)
                    {
                        _state = UIButtonState.Clicked;
                    }
                }
            }
            _prevState = mouse;
        }
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Vector2 tSize = _font.MeasureString(_text);
                Vector2 tCenter = tSize / 2;
                Vector2 sCenter = _size / 2;
                _textOffset = sCenter - tCenter;
            }
        }
        public override void Draw(SpriteBatch batch)
        {
            Color color=Color.White;
            switch(_state)
            {
                case UIButtonState.Nothing: color = Color.Gray; break;
                case UIButtonState.Hover: color = Color.LightGray; break;
                case UIButtonState.Clicked: color = Color.White; break;
            }
            buttonBorder.Draw(batch, color);
            batch.DrawString(_font, Text, TotalOffset+_textOffset, color);
        }
    }
     
}
