using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    class Pusher : EventObject
    {
        float frame=0;
        public Pusher(IMap map, Texture2D texture, Rectangle position, int id)
            : base(map, texture, position, id)
        {

        }

        public override void Update(float ms)
        {
            frame += 5 * ms / 1000f;
        }

        public override void Event(Player obj)
        {
        }

        public override void Draw(SpriteBatch batch)
        {
            int iframe = (int)frame % 8;
            iframe = 0;
            //batch.Draw(_texture, _rect, new Rectangle(16 * iframe, 0, 16, 16), Color.White);
        }
    }
}
