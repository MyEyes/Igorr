using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class BossBlobAttack1:BodyPart
    {
        public BossBlobAttack1()
            : base()
        {
            hasAttack = true;
        }

        public override Attack GetAttack(Player owner,Vector2 dir, int DmgBonus, int info)
        {
            Attack att = null;
            att = new Attack(owner.map, 17, new Rectangle((int)owner.MidPosition.X, (int)owner.Position.Y + owner.Rect.Height - 8, 5, 8), dir*200, 500, owner.ID, owner.GroupID, 5004);
            att.HitOnce = true;
            att.Knockback = new Vector2(-200, -50);
            return att;
        }
        public override int GetID()
        {
            return 2;
        }
    }

    class BossBlobAttack2 : BodyPart
    {
        public BossBlobAttack2()
            : base()
        {
            hasAttack = true;
        }
        public override Attack GetAttack(Player owner,Vector2 dir, int DmgBonus, int info)
        {
            Attack att = null;
            att = new Attack(owner.map, 17, new Rectangle((int)owner.MidPosition.X, (int)owner.Position.Y + owner.Rect.Height - 8, 5, 8), new Vector2(200, 0), 500, owner.ID, owner.GroupID, 5004);
            att.HitOnce = true;
            att.Knockback = new Vector2(200, -50);
            return att;
        }

        public override int GetID()
        {
            return 3;
        }
    }

    class BossBlobAttack3 : BodyPart
    {
        public BossBlobAttack3()
            : base()
        {
            //attackID = 3;
            hasAttack = true;
        }

        public override Attack GetAttack(Player owner,Vector2 dir, int DmgBonus, int info)
        {
            Attack att = null;
            if (owner.LookLeft)
            {
                att = new Attack(owner.map, 4, new Rectangle((int)owner.MidPosition.X, (int)owner.Position.Y + owner.Rect.Height - 16, 5, 8), new Vector2(-200, 0), 2200, owner.ID, owner.GroupID, 5005);
            }
            else
            {
                att = new Attack(owner.map, 4, new Rectangle((int)owner.MidPosition.X, (int)owner.Position.Y + owner.Rect.Height - 16, 5, 8), new Vector2(200, 0), 2200, owner.ID, owner.GroupID, 5005);
            }
            att.HitOnce = true;
            return att;
        }

        public override int GetID()
        {
            return 4;
        }
    }
}
