﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic
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

        public override Attack GetAttack(Player owner, int info)
        {
            Attack att = null;
            Rectangle startRect;
            startRect = owner.Rect;
            startRect.Height -= 4;
            startRect.Y += 3;
            startRect.X += owner.LookLeft ? -8 : 8;
            att = new Grenade(owner.map, 0, startRect, new Vector2(owner.LastSpeed.X + (owner.LookLeft ? -60 : 60), owner.LastSpeed.Y - 70), 2500, owner.ID, owner.GroupID, 3);
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
