using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic
{
    class ShadowOn : EventObject
    {

        public ShadowOn(Map map, Rectangle rect, int id)
            : base(map, rect, id)
        {
        }

        public override void Update(float ms)
        {
            base.Update(ms);
        }

        public override void Event(Player obj)
        {
            obj.ShadowsOn = true;
        }
    }
}
