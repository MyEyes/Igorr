using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic.AI
{
    class MiniBossBlob : NPC
    {
        private Vector2 startPos;
        private bool follow;
        private bool moveLeft = false;
        private float moveCountdown = 0;
        private Player target;

        public MiniBossBlob(Map map, Rectangle spawnPos, int id)
            : base(map, spawnPos, id)
        {
            startPos = new Vector2(spawnPos.X, spawnPos.Y);
            GivePart(new BigBlobLegs());
            GivePart(new Striker());
            _name = "BigBlob";
        }

        public MiniBossBlob(Map map, string charfile, Rectangle spawnPos, int id)
            : base(map, charfile, spawnPos, id)
        {
            startPos = new Vector2(spawnPos.X, spawnPos.Y);
            GivePart(new BigBlobLegs());
            GivePart(new Striker());
            _name = "BigBlob";
        }

        public override void Update(Map map, float seconds)
        {
            if (!follow && _random.NextDouble() > 0.9)
            {
                target = _map.ObjectManager.GetPlayerAround(_position, 150);
                if (target != null && (target.Position - startPos).Length() < 400)
                    follow = true;
            }
            if (follow)
            {
                if ((startPos - _position).Length() > 400)
                    follow = false;
                if (target.HP <= 0)
                    target = null;
                if (target != null)
                {
                    if (target.Position.X > this._position.X + _rect.Width)
                    {
                        Move(1);
                    }
                    else if (target.Position.X+target.Rect.Width < this._position.X)
                    {
                        Move(-1);
                    }
                    else if (target.Position.X > this._position.X)
                    {
                        Move(0.0001f);
                    }
                    if (target.Position.Y < this._position.Y)
                        Jump();
                    if ((target.Position+new Vector2(target.Rect.Width,target.Rect.Height)/2 - this.Position - new Vector2(Rect.Width, Rect.Height) / 2).Length() < 55 && _random.NextDouble() > 0.99f)
                        _map.ObjectManager.SpawnAttack(this.ID, Vector2.Zero, 1, 0);
                }
                else
                    follow = false;
            }
            else
            {
                if (moveCountdown < 0)
                {
                    if (_position.X > startPos.X)
                        moveLeft = _random.Next((int)(startPos.X - _position.X), 10) < 0;
                    else
                        moveLeft = _random.Next(-10, (int)(startPos.X-_position.X)) < 0;
                    if (_position.X < startPos.X - 400)
                        moveLeft = false;
                    else if (_position.X > startPos.X + 400)
                        moveLeft = true;
                    moveCountdown = (float)(10 / (_random.NextDouble() * Math.Abs(_position.X - startPos.X) + 1));
                }
                moveCountdown -= seconds;
                if(moveLeft)
                    Move(-0.5f);
                else
                    Move(0.5f);
            }
            base.Update(map, seconds);
        }
    }
}
