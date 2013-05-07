using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    public class Striker2 : BodyPart
    {
        public Striker2()
            : base()
        {
            //attackID = 2;
            hasAttack = true;
        }

        public override Attack GetAttack(Player owner,Vector2 dir, int DmgBonus, int info)
        {
            Attack att = null;
            Rectangle startRect = owner.Rect;
            startRect = owner.Rect;
            startRect.Height -= 4;
            startRect.Y += 3;
            startRect.X += (int)(dir.X * 8);
            att = new Attack(owner.map, 1, startRect, new Vector2(owner.LastSpeed.X, 0) + 200 * dir, 200, owner.ID, owner.GroupID, 2, owner.map.ObjectManager.getID());
            att.HitOnce = false;
            att.Penetrates = true;
            owner.Attack(0.6f);
            return att;
        }

        public override string GetName()
        {
            return "Striker2";
        }

        public override int GetID()
        {
            return 9;
        }
    }
}
