using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class Blocker : EventObject
    {
        float countdown = 12;
        int maxhp = 20;
        int hp = 20;
        bool blocked = false;
        bool damaged = false;

        public Blocker(IMap map, Rectangle position, int id)
            : base(map, position, id)
        {
            _objectType = 100;
            if (_map != null)
                _map.ChangeTile(1, this.MidPosition, 20);
        }

        public override void Update(float ms)
        {
            int damage = _map.ObjectManager.AttackManager.CheckObject(this);
            hp -= damage;
            if (hp <= maxhp/2 && !damaged)
            {
                _map.ChangeTile(1, this.MidPosition, 21);
                damaged = true;
            }
            else if (hp <= 0 && countdown == 12)
            {
                _map.ChangeTile(1, this.MidPosition, -1);
                countdown -= ms / 1000f;
            }
            else if(hp<=0)
            {
                countdown -= ms / 1000f;
                if (countdown <= 0 && !blocked)
                {
                    countdown = 12;
                    _map.ChangeTile(1, this.MidPosition, 20);
                    hp = maxhp;
                    damaged = false;
                }
            }
            if (blocked && countdown < 0)
            {
                countdown = 1;
                blocked = false;
            }
        }

        public override void Event(Player obj)
        {
            blocked = true;
        }
    }
}
