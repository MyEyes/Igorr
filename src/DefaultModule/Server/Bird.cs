using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic.AI
{
    //Das Jule Hühnchen (23.06.12)
    class Bird:NPC
    {
        Point spawnPoint;
        Rectangle followRect;
        bool _Move = false;
        bool Left = false;
        float changeCountdown = 1f;

        public Bird(IMap map, Rectangle spawnRect, int id):base(map,spawnRect,id)
        {
            followRect = new Rectangle(spawnRect.X - 80, spawnRect.Y - 16, 160, 32);
            spawnPoint = new Point(spawnRect.X + spawnRect.Width / 2, spawnRect.Y + spawnRect.Height / 2);
            _objectType = 5006;
        }

        public Bird(IMap map, string file, Rectangle spawnRect, int id)
            : base(map, file, spawnRect, id)
        {
            followRect = new Rectangle(spawnRect.X - 80, spawnRect.Y - 16, 160, 32);
            spawnPoint = new Point(spawnRect.X + spawnRect.Width / 2, spawnRect.Y + spawnRect.Height / 2);
            _objectType = 5006;
        }

        public override void Update(IMap map, float seconds)
        {

            changeCountdown -= seconds;
            //If the countdown has expired decide wether to move or not
            if (changeCountdown <= 0)
            {
                _Move = _random.NextDouble() > 0.9f;
                Left = _random.NextDouble() > 0.5f;
                changeCountdown = 0.5f + (float)_random.NextDouble();
            } 
            
            if (!followRect.Contains((int)this.MidPosition.X, (int)this.MidPosition.Y))
            {
                Left = this.MidPosition.X > spawnPoint.X;
            }

            //If we've decided to move, move
            if (_Move)
            {
                if (Left)
                    Move(-0.75f);
                else
                    Move(0.75f);
            }
            base.Update(map, seconds);
        }
    }
}
