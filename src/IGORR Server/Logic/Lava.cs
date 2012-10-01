using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR_Server.Logic
{
    class Lava : EventObject
    {

        public Lava(Map map, Rectangle position, int id)
            : base(map, position, id)
        {
            _objectType = 't' - 'a';
            _name = "Lava";
        }

        public override void Event(Player obj)
        {
            if (obj!=null && !obj.Invincible)
            {
                obj.GetDamage(20);
            }
        }
    }
}
