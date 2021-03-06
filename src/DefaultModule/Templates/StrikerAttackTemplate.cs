﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Content;
using IGORR.Modules;

namespace IGORR.Client.Logic
{
    class StrikerAttackTemplate: AttackTemplate
    {
        public override Attack CreateClient(int objectID, Microsoft.Xna.Framework.Vector2 dir, Microsoft.Xna.Framework.Point position, string info)
        {
            AttackInfo aInfo = new AttackInfo();
            aInfo.CollisionDespawn = true;
            aInfo.HitDespawn = true;
            aInfo.LifeTime = 0.1f;
            aInfo.Physics = false;
            return new Attack(Content.ContentInterface.LoadTexture("Attack"), new Microsoft.Xna.Framework.Rectangle(position.X, position.Y, 16, 12), dir, objectID, aInfo);
        }

        public override int TypeID
        {
            get
            {
                return 1;
            }
        }
    }
}
