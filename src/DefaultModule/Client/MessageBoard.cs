using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Content;

namespace IGORR.Client.Logic
{
    class MessageBoard : EventObject
    {
        SpriteFont font;
        string text = "";
        float offset = 0;
        public MessageBoard(IMap map, Texture2D texture, Rectangle position, int id)
            : base(map,texture, position, id)
        {
            font = ContentInterface.LoadFont("font");
        }


        public override void GetInfo(string info)
        {
            text = info;
            offset = font.MeasureString(text).X / 2;
        }

        public override void Update(float ms)
        {
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(_texture, _rect, null,Color.White,0,Vector2.Zero, SpriteEffects.None,0.47f);
            batch.DrawString(font, text, new Vector2(_rect.X - offset*0.125f, _rect.Y - 10), Color.White, 0, Vector2.Zero, 0.25f, SpriteEffects.None, 0.46f);
        }
    }
}
