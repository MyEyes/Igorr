using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Content;

namespace IGORR.Client.UI
{
    class Panel : UIElement
    {
        const int cornerSize = 8;
        Texture2D _texture;

        //Texture is assumed to be consisting of 3x3 tiles which are 8x8 pixels each, one for each side, one for each corner and the center
        public Panel(UIElement parent, Vector2 offset, Vector2 size, Texture2D texture):base(parent,offset)
        {
            SetSize(size);
            _texture = texture;
        }

        public void SetSize(Vector2 size)
        {
            _size = size;
        }

        //Stuff breaks when the panel is smaller than 2*cornerSize but if you make such a small panel
        //You fucking deserve it

        public override void Draw(SpriteBatch batch)
        {
            Draw(batch, Color.White);
        }

        public void Draw(SpriteBatch batch, Color color)
        {
            Vector2 ul = TotalOffset;
            Vector2 uc = ul + Vector2.UnitX * cornerSize;
            Vector2 ur = ul + Vector2.UnitX * (_size.X - cornerSize);
            Vector2 cl = ul + Vector2.UnitY * cornerSize;
            Vector2 cr = ur + Vector2.UnitY * cornerSize;
            Vector2 cc = cl + Vector2.UnitX * cornerSize;
            Vector2 ll = ul + Vector2.UnitY * (_size.Y - cornerSize);
            Vector2 lc = ll + Vector2.UnitX * cornerSize;
            Vector2 lr = ll + Vector2.UnitX * (_size.X - cornerSize);

            batch.Draw(_texture, ul, new Rectangle(0, 0, cornerSize, cornerSize), color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);
            batch.Draw(_texture, uc, new Rectangle(cornerSize, 0, cornerSize, cornerSize), color, 0, Vector2.Zero, new Vector2(_size.X / cornerSize - 2, 1), SpriteEffects.None, depth);
            batch.Draw(_texture, ur, new Rectangle(2 * cornerSize, 0, cornerSize, cornerSize), color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);
            batch.Draw(_texture, cl, new Rectangle(0, cornerSize, cornerSize, cornerSize), color, 0, Vector2.Zero, new Vector2(1, _size.Y / cornerSize - 2), SpriteEffects.None, depth);
            batch.Draw(_texture, cc, new Rectangle(cornerSize, cornerSize, cornerSize, cornerSize), color, 0, Vector2.Zero, new Vector2(_size.X / cornerSize - 2, _size.Y / cornerSize - 2), SpriteEffects.None, depth);
            batch.Draw(_texture, cr, new Rectangle(2 * cornerSize, cornerSize, cornerSize, cornerSize), color, 0, Vector2.Zero, new Vector2(1, _size.Y / cornerSize - 2), SpriteEffects.None, depth);
            batch.Draw(_texture, ll, new Rectangle(0, 2 * cornerSize, cornerSize, cornerSize), color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);
            batch.Draw(_texture, lc, new Rectangle(cornerSize, 2 * cornerSize, cornerSize, cornerSize), color, 0, Vector2.Zero, new Vector2(_size.X / cornerSize - 2, 1), SpriteEffects.None, depth);
            batch.Draw(_texture, lr, new Rectangle(2 * cornerSize, 2 * cornerSize, cornerSize, cornerSize), color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);

            DrawChildren(batch);
        }
    }
}
