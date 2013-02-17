using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IGORR.Protocol.Messages;
using IGORR.Protocol;

namespace IGORR.Server.Logic.AI
{
    
    class SleepingOrb:NPC
    {
        Player target = null;
        Rectangle targetArea;
        Vector2 center = Vector2.Zero;

        public SleepingOrb(IMap map, Rectangle spawnRect, int id):base(map,spawnRect,id)
        {
            _objectType = 10000;
        }

        public SleepingOrb(IMap map, string file, Rectangle spawnRect, int id)
            : base(map, file, spawnRect, id)
        {
            _objectType = 10000;
            targetArea = spawnRect;
            targetArea.X -= 64;
            targetArea.Width += 128;
            targetArea.Y -= 16;
            targetArea.Height += 32;
            center = new Vector2(targetArea.Center.X, targetArea.Center.Y);
        }

        public override void Update(IMap map, float seconds)
        {
            if (target == null)
            {
                target = map.ObjectManager.GetPlayerInArea(targetArea, 1, true);
                if (target != null)
                {
                    SetAnimation(true, 0);
                }
            }
            else
            {

                if ((center - target.MidPosition).Length() > 100)
                {
                    SetAnimation(false, 0);
                    target = null;
                }
            }
            base.Update(map, seconds);
        }
    }
}
