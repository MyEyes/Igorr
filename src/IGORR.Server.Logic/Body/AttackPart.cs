using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Server.Logic.Body
{
    public class AttackPart : BodyPart
    {
        public AttackPart()
            : base(BodyPartType.Attack)
        {
        }



        public virtual Attack GetAttack(Player attacker, Vector2 dir, int damageBonus, int info)
        {
            return null;
        }

    }
}
