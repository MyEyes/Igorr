using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Content;
using IGORR.Modules;

namespace IGORR.Client.Logic
{
    class SlimeShotAttackTemplate: AttackTemplate
    {
        public override Attack CreateClient(int objectID, Microsoft.Xna.Framework.Vector2 dir, Microsoft.Xna.Framework.Point position, string info)
        {
            AttackInfo aInfo = new AttackInfo();
            aInfo.CollisionDespawn = true;
            aInfo.Physics = true;
            aInfo.bounceFactor = 0.4f;
            aInfo.LifeTime = 1.5f;
            aInfo.HitDespawn = true;
            return new SlimeShot(Content.ContentInterface.LoadTexture("healthOrb"), new Microsoft.Xna.Framework.Rectangle(position.X, position.Y, 6, 6), dir, objectID, aInfo);
        }

        public override int TypeID
        {
            get
            {
                return 5;
            }
        }
    }
}
