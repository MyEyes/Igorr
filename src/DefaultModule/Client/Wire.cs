using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    class Wire : EventObject
    {
        int frame=0;
        bool on = false;

        public Wire(IMap map, Texture2D texture, Rectangle position, int id)
            : base(map, texture, position, id)
        {

        }

        public override void Update(float ms)
        {
        }

        public override void Event(Player obj)
        {
        }

        public override void GetInfo(string info)
        {
            frame = (int)info[0];
            on = info[1] == 1;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(_texture, _rect, new Rectangle(16 * frame, 0, 16, 16), !on ? new Color(50, 50, 50) : Color.Yellow);
        }
    }
}
