using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Content;

namespace IGORR.Client.UI
{
    class PictureBox:UIElement
    {
        Texture2D _texture;
        Vector2 _size;

        public PictureBox(UIElement parent, Vector2 offset, Vector2 size, Texture2D texture)
            : base(parent, offset)
        {
            _texture = texture;
            _size = size;
        }

        public void SetSize(Vector2 size)
        {
            _size = size;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(_texture, TotalOffset, null, Color.White, 0, Vector2.Zero, new Vector2(_size.X / _texture.Width, _size.Y / _texture.Height), SpriteEffects.None, 0.1f);
            DrawChildren(batch);
        }
    }
}
