using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic
{
    class Striker:BodyPart
    {
        public Striker()
            : base()
        {
            //attackID = 1;
            hasAttack = true;
        }

        public override Attack GetAttack(Player owner,Vector2 dir, int DmgBonus, int info)
        {
            Attack att = null;
            Rectangle startRect = owner.Rect;
            startRect.Height -= 4;
            startRect.Y += 3;
            startRect.X += (int)(dir.X*8);
            att = new Attack(owner.map, 15, startRect, new Vector2(owner.LastSpeed.X, 0)+200*dir, 100, owner.ID, owner.GroupID, 1);
            att.HitOnce = true;
            owner.Attack(0.3f);
            return att;
        }

        public override string GetName()
        {
            return "Striker";
        }

        public override int GetID()
        {
            return 'd'-'a';
        }
    }
}
