﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic.AI
{
    class BossMinions : NPC
    {
        Player target=null;
        float moveCountdown=0.5f;
        bool left = true;

        public BossMinions(IMap map, Rectangle spawnPos, int id)
            : base(map,spawnPos, id)
        {
            _groupID = 2;
            GivePart(new Legs());
        }

        public BossMinions(IMap map,string charfile, Rectangle spawnPos, int id)
            : base(map, charfile, spawnPos, id)
        {
            _groupID = 2;
            GivePart(new Legs());
            _objectType = 5008;
        }

        public override void Update(IMap map, float seconds)
        {
            if(target==null || target.HP<=0)
            {
                target = _map.ObjectManager.GetPlayerAround(_position, 250);
            }

            if (target != null)
            {
                if (target.HP <= 0)
                {
                    target = null;
                    base.Update(map, seconds);
                    return;
                }
                moveCountdown -= seconds;
                if (moveCountdown < 0)
                {
                    if (target.Position.X + target.Rect.Width / 2 < this.Position.X + _rect.Width / 2)
                        left = true;
                    else
                        left = false;
                    moveCountdown = (float)_random.NextDouble() + 0.5f;
                }
                if (left)
                    Move(-1,0);
                else
                    Move(1,0);
                if (_random.NextDouble() > 0.99f)
                    Jump();
            }
            base.Update(map, seconds);
        }
        /*
        public override Attack GetAttack(int id, int info)
        {
            Logic.Attack att;
            if (LookLeft)
            {
                att = new Attack(_map,1, new Rectangle((int)_position.X + _rect.Width / 2, (int)_position.Y + _rect.Height - 8, 5, 8), new Vector2(-200, 0), 400, this.ID, this.GroupID, 5006);
            }
            else
            {
                att = new Attack(_map, 1, new Rectangle((int)_position.X + _rect.Width / 2, (int)_position.Y + _rect.Height - 8, 5, 8), new Vector2(200, 0), 400, this.ID, this.GroupID, 5006);
            }
            att.HitOnce = true;
            Attack(1);
            return att;
        }
         */
    }
}
