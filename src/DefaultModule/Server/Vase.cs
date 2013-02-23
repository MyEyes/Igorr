using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class Vase : EventObject
    {
        int maxhp = 10;
        int hp = 10;

        public Vase(IMap map, Rectangle position, int id)
            : base(map, position, id)
        {
            _objectType = 3000;
        }

        public override void Update(float ms)
        {
            int damage = _map.ObjectManager.AttackManager.CheckObject(this);
            hp -= damage;
            if(hp<=0)
            {
                _map.ObjectManager.Remove(this);
                _parent.RemoveChild();
                DoEffect(2, Rect.Center, Vector2.Zero, "");
            }
        }
    }
}
