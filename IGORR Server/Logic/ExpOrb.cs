using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORRProtocol;
using IGORRProtocol.Messages;

namespace IGORR_Server.Logic
{
    class ExpOrb : EventObject
    {
        int _expAmount;
        public ExpOrb(int exp, Map map, Rectangle position, int id)
            : base(map, position, id)
        {
            _objectType = 20;
            _name = "Exp Orb";
            _expAmount = exp;
        }

        public override void Event(Player obj)
        {
            _map.ObjectManager.GiveXP(_expAmount);
            _map.ObjectManager.Remove(this);
            _parent.RemoveChild();
        }
    }
}
