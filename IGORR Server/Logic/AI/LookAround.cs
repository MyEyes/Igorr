using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic.AI
{
    class LookAround : NPC
    {
        bool _Move = false;
        bool Left = false;
        float changeCountdown = 1f;

        public LookAround(Map map, Rectangle spawnRect, int id)
            : base(map, spawnRect, id)
        {
        }

        public LookAround(Map map, string file, Rectangle spawnRect, int id)
            : base(map, file, spawnRect, id)
        {
        }

        public override void Update(Map map, float seconds)
        {

            changeCountdown -= seconds;
            //If the countdown has expired decide wether to move or not
            if (changeCountdown <= 0)
            {
                _Move = _random.NextDouble() > 0.9f;
                Left = _random.NextDouble() > 0.5f;
                changeCountdown = 0.5f + (float)_random.NextDouble();
            }

            //If we've decided to move, move
            if (_Move)
            {
                if (Left)
                    Move(-0.00001f);
                else
                    Move(0.00001f);
            }
            base.Update(map, seconds);
        }
    }
}
