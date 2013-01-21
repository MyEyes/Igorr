using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR.Server.Logic
{
    class ExpOrb : EventObject
    {
        int _expAmount;
        public ExpOrb(int exp, IMap map, Rectangle position, int id)
            : base(map, position, id)
        {
            _objectType = 20;
            _name = "Exp Orb";
            _expAmount = exp;
        }

        public override void Event(Player obj)
        {
            obj.GetExp(_expAmount, MidPosition);
            _map.ObjectManager.Remove(this);
            _parent.RemoveChild();
        }
    }
}
