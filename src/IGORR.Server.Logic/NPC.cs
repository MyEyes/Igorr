using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    public class NPC:Player
    {
        protected float _dropchance = 0.5f;
        protected int _XPBonus=0;


        public NPC(IMap map,Rectangle spawnPos, int id)
            : base(map, spawnPos, id)
        {
            _invincibleTime = 0;
            _baseBody.speedBonus = 30;
        }

        public NPC(IMap map,string charfile, Rectangle spawnPos, int id)
            : base(map,charfile, spawnPos, id)
        {
            _invincibleTime = 0;
            _baseBody.speedBonus = 30;
        }

        public override void Update(IMap map, float seconds)
        {
            /*
            moveCountdown -= seconds;
            if (moveCountdown < 0)
            {
                left = !left;
                moveCountdown = 1;
                Jump();
            }
            if (left)
                Move(-1);
            else
                Move(1);
             */
            base.Update(map, seconds);
        }

        public virtual void Interact(Player player)
        {

        }

        public int XPBonus
        {
            get { return _XPBonus; }
        }
    }
}
