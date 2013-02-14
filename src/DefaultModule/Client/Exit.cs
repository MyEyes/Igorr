using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    class Exit : EventObject
    {
        float frame = 0;
        public Exit(Texture2D texture, IMap map, Rectangle position, int id)
            : base(map, texture, position, id)
        {

        }

        public override void Update(float ms)
        {
            frame += 7.5f * ms / 1000f;
            if (frame > 1000)
                frame -= 1000;
        }

        public override void Draw(SpriteBatch batch)
        {
            int iframe = (int)frame % 4;
            batch.Draw(_texture, _rect, new Rectangle(16 * iframe, 0, 16, 16), Color.White,0, Vector2.Zero, SpriteEffects.None, 0.43f);
        }
    }
}
