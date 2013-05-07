using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic.AI
{
    class ShyFollower:NPC
    {
        Point spawnPoint;
        Rectangle followRect;
        Player _target=null;
        bool _Move = false;
        bool Left = false;
        float changeCountdown = 1f;

        public ShyFollower(IMap map, Rectangle spawnRect, int id):base(map,spawnRect,id)
        {
            followRect = new Rectangle(spawnRect.X - 80, spawnRect.Y - 16, 160, 32);
            spawnPoint = new Point(spawnRect.X + spawnRect.Width / 2, spawnRect.Y + spawnRect.Height / 2);
            _invincible = true;
            _objectType = 5005;
        }

        public ShyFollower(IMap map, string file, Rectangle spawnRect, int id)
            : base(map, file, spawnRect, id)
        {
            followRect = new Rectangle(spawnRect.X - 80, spawnRect.Y - 16, 160, 32);
            spawnPoint = new Point(spawnRect.X + spawnRect.Width / 2, spawnRect.Y + spawnRect.Height / 2);
            _invincible = true;
            _objectType = 5005;
        }

        public override void Update(IMap map, float seconds)
        {
            //Try to find target
            if (_target == null)
            {
                _target = _map.ObjectManager.GetPlayerInArea(followRect);
            }

            //If you get out of the follow area or your target does, do this
            if(!followRect.Contains((int)this.MidPosition.X,(int)this.MidPosition.Y))
            {
                _target = null;
                Left = this.MidPosition.X > spawnPoint.X;
                changeCountdown = 0.1f;
                _Move = true;
            }
            changeCountdown -= seconds;
            //If the countdown has expired decide wether to move or not
            if (_target == null && changeCountdown <= 0)
            {
                _Move = _random.NextDouble() > 0.9f;
                Left = _random.NextDouble() > 0.5f;
                changeCountdown = 0.5f + (float)_random.NextDouble();
            }
            else if (_target != null)
            {
                _Move = (_target.MidPosition - this.MidPosition).Length() > 30;
                Left = _target.MidPosition.X < this.MidPosition.X;
                if (_target.HP <= 0)
                    _target = null;
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
