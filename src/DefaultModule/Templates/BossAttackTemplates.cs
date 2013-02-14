using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Content;
using IGORR.Modules;

namespace IGORR.Client.Logic
{
    class BossAttack1Template: AttackTemplate
    {
        public override Attack CreateClient(int objectID, Microsoft.Xna.Framework.Vector2 dir, Microsoft.Xna.Framework.Point position, string info)
        {
            AttackInfo aInfo = new AttackInfo();
            aInfo.CollisionDespawn = true;
            aInfo.HitDespawn = true;
            aInfo.LifeTime = 0.5f;
            aInfo.Physics = false;
            return new Attack(Content.ContentInterface.LoadTexture("Attack"), new Microsoft.Xna.Framework.Rectangle(position.X, position.Y, 10, 16), dir, objectID, aInfo);
        }

        public override int TypeID
        {
            get
            {
                return 5004;
            }
        }
    }

    class BossAttack2Template : AttackTemplate
    {
        public override Attack CreateClient(int objectID, Microsoft.Xna.Framework.Vector2 dir, Microsoft.Xna.Framework.Point position, string info)
        {
            AttackInfo aInfo = new AttackInfo();
            aInfo.CollisionDespawn = true;
            aInfo.HitDespawn = true;
            aInfo.LifeTime = 2.2f;
            aInfo.Physics = false;
            return new Attack(Content.ContentInterface.LoadTexture("Attack"), new Microsoft.Xna.Framework.Rectangle(position.X, position.Y, 5, 8), dir, objectID, aInfo);
        }

        public override int TypeID
        {
            get
            {
                return 5005;
            }
        }
    }
}
