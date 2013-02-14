using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR.Client.Logic
{
    class Grenade:Attack
    {

        public Grenade(Microsoft.Xna.Framework.Graphics.Texture2D tex, Microsoft.Xna.Framework.Rectangle rect, Microsoft.Xna.Framework.Vector2 dir, int id, AttackInfo info)
            : base(tex, rect, dir, id, info)
        {

        }

        public override bool Update(IMap map, float seconds)
        {
            bool alive = base.Update(map, seconds);
            if (!alive)
                map.Particles.Boom(map, MidPosition);
            return alive;
        }
    }
}
