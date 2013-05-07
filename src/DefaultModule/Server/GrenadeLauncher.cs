using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    public class GrenadeLauncher : BodyPart
    {
        public GrenadeLauncher()
            : base()
        {
            //attackID = 3;
            hasAttack = true;
        }

        public override string GetName()
        {
            return "Grenade Launcher";
        }

        public override Attack GetAttack(Player owner,Vector2 dir,int DmgBonus, int info)
        {
            Attack att = null;
            Rectangle startRect;
            startRect = owner.Rect;
            startRect.Height =6;
            startRect.Width = 6;
            startRect.X += 5;
            startRect.Y += 3;
            startRect.X += (int)(dir.X);
            att = new Grenade(owner.map, 0, startRect, dir*250, 2500, owner.ID, owner.GroupID, owner.map.ObjectManager.getID());
            att.HitOnce = false;
            att.Penetrates = true;
            owner.Attack(1f);
            return att;
        }

        public override int GetID()
        {
            return 80;
        }
    }
}
