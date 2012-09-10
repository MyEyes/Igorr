using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic
{
    class Grenade:Attack
    {

        public Grenade(Map map, int damage, Rectangle rect, Vector2 mov, float lifeTime, int parentID, int groupID, int id)
            :base(map,damage,rect,mov,lifeTime,parentID,groupID,id)
        {
            Penetrates = true;
        }

        public override void Update(float ms)
        {
            ms /= 1000f;
            _movement.Y += Player.gravity * ms;
            Move(new Vector2(0, Movement.Y*ms));
            if (map.Collides(this))
            {
                Move(new Vector2(0, -Movement.Y*ms));
                _movement.Y *= -0.4f;
                _movement.X *= 0.4f;
            }
            Move(new Vector2(Movement.X*ms, 0));
            if (map.Collides(this))
            {
                Move(new Vector2(-Movement.X*ms, 0));
                _movement.X *= -0.4f;
                _movement.Y *= 0.4f;
            }
            lifeTime -= ms*1000;
            if (lifeTime <= 0)
            {
                Attack att = new Attack(_map, 64, new Rectangle((int)this.MidPosition.X-20, (int)this.MidPosition.Y-20, 40, 40), Vector2.Zero, 100, parentID, groupID, 4);
                att.Penetrates = true;
                att.HitOnce = false;
                _manager.Spawn(att);
            }
        }
    }
}
