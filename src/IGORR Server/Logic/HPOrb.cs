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
    class HPOrb : EventObject
    {
        int _healAmount;
        public HPOrb(int heal, Map map, Rectangle position, int id)
            : base(map, position, id)
        {
            _objectType = 15;
            _name = "HP Orb";
            _healAmount = heal;
        }

        public override void Event(Player obj)
        {
            if (obj!=null && !obj.Invincible)
            {
                obj.GetDamage(-_healAmount);
                _map.ObjectManager.Remove(this); 
                _parent.RemoveChild();
            }
        }
    }
}
