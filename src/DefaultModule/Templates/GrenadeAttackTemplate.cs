using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Modules;

namespace IGORR.Client.Logic
{
    class GrenadeAttackTemplate : AttackTemplate
    {
        public override Attack  CreateClient(int objectID, Microsoft.Xna.Framework.Vector2 dir, Microsoft.Xna.Framework.Point position, string info)
        {
            AttackInfo aInfo = new AttackInfo();
            aInfo.CollisionDespawn = false;
            aInfo.Physics = true;
            aInfo.bounceFactor = 0.4f;
            aInfo.LifeTime = 2.5f;
            return new Grenade(Content.ContentInterface.LoadTexture("Egg"), new Microsoft.Xna.Framework.Rectangle(position.X, position.Y, 6, 6), dir, objectID, aInfo);
        }
        public override int TypeID
        {
            get
            {
                return 3;
            }
        }
    }
}
