using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class SlimeShot:Attack
    {

        public SlimeShot(IMap map, int damage, Rectangle rect, Vector2 mov, float lifeTime, int parentID, int groupID, int id)
            :base(map,damage,rect,mov,lifeTime,parentID,groupID,5,id)
        {
            this.HitOnce = true;
            this.Penetrates = false;
            _objectType = 5;
        }

        public override void Update(float ms)
        {
            ms /= 1000f;
            _movement.Y += Player.gravity * ms;
            Move(new Vector2(0, Movement.Y*ms));
            if (map.Collides(this))
            {
                Move(new Vector2(0, -Movement.Y*ms));
                _movement.Y *= -0.8f;
                _movement.X *= 0.8f;
                lifeTime = 0;
            }
            Move(new Vector2(Movement.X*ms, 0));
            if (map.Collides(this))
            {
                Move(new Vector2(-Movement.X*ms, 0));
                _movement.X *= -0.8f;
                _movement.Y *= 0.8f;
                lifeTime = 0;
            }
            lifeTime -= ms*1000;
            if (lifeTime <= 0)
            {
                Hit();
            }
        }

        public override void Hit()
        {
            Attack att = new Attack(_map, 4, new Rectangle((int)this.MidPosition.X - 20, (int)this.MidPosition.Y - 20, 40, 40), Vector2.Zero, 50, parentID, groupID, 6, map.ObjectManager.getID());
            att.Penetrates = true;
            att.HitOnce = false;
            _manager.Spawn(att);
            DoEffect(3, _rect.Center, _movement, "");
        }
    }
}
