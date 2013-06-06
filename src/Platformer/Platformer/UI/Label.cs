using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.UI
{
    class Label:UIElement
    {
        static SpriteFont _font;
        string _text;

        public Label(UIElement parent, Vector2 position, string text=""):base(parent, position)
        {
            if (_font == null)
                _font = ContentInterface.LoadFont("font2");
            _text = text;
        }

        public void SetText(string text)
        {
            _text = text;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.DrawString(_font, _text, TotalOffset , Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth + 0.05f);
            base.Draw(batch);
        }
    }
}
