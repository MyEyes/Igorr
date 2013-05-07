using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR.Client.Logic
{
    class SlimeShot:Attack
    {

        public SlimeShot(Microsoft.Xna.Framework.Graphics.Texture2D tex, Microsoft.Xna.Framework.Rectangle rect, Microsoft.Xna.Framework.Vector2 dir, int id, AttackInfo info)
            : base(tex, rect, dir, id, info)
        {

        }

        public override bool Update(IMap map, float seconds)
        {
            bool alive = base.Update(map, seconds);
            Modules.ModuleManager.DoEffect(4, map, -this.Movement, this.Rect.Center, "");
            map.SetGlow(_id, this.MidPosition, Microsoft.Xna.Framework.Color.Green, 20, false, 300);
            return alive;
        }
    }
}
