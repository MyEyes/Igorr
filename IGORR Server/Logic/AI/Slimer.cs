using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic.AI
{
    class Slimer : NPC
    {
        float moveCountdown = 1;
        bool left;
        Point spawnPoint;
        Rectangle followRect;
        Player _target = null;
        bool _Move = false;
        bool Left = false;
        bool attack = false;
        float changeCountdown = 1f;
        float attackCooldown = 0f;

        public Slimer(Map map,Rectangle spawnRect, int id)
            : base(map,spawnRect, id)
        {
            _XPBonus = 25;
            GivePart(new Legs());
            GivePart(new Striker());
            _groupID = 5;

            followRect = new Rectangle(spawnRect.X - 80, spawnRect.Y - 16, 160, 32);
            spawnPoint = new Point(spawnRect.X + spawnRect.Width / 2, spawnRect.Y + spawnRect.Height / 2);
        }

        public Slimer(Map map, string charfile, Rectangle spawnRect, int id)
            : base(map, charfile, spawnRect, id)
        {
            GivePart(new Legs());
            GivePart(new Striker());
            _groupID = 5;
            _XPBonus = 25;
            followRect = new Rectangle(spawnRect.X - 80, spawnRect.Y - 16, 160, 32);
            spawnPoint = new Point(spawnRect.X + spawnRect.Width / 2, spawnRect.Y + spawnRect.Height / 2);
        }

        public override void Update(Map map, float seconds)
        {
            //Try to find target
            if (_target == null)
            {
                _target = _map.ObjectManager.GetPlayerInArea(followRect);
            }

            //If you get out of the follow area or your target does, do this
            if (!followRect.Contains((int)this.MidPosition.X, (int)this.MidPosition.Y))
            {
                _target = null;
                Left = this.MidPosition.X > spawnPoint.X;
                changeCountdown = 0.5f;
                _Move = true;
            }
            changeCountdown -= seconds;
            //If the countdown has expired decide wether to move or not
            if (_target == null && changeCountdown <= 0)
            {
                _Move = _random.NextDouble() > 0.7f;
                Left = _random.NextDouble() > 0.5f;
                changeCountdown = 0.25f + 0.5f*(float)_random.NextDouble();
            }
            else if (_target != null &&!attack)
            {
                _Move = Math.Abs(_target.MidPosition.X - _position.X) > 20;
                    
                Left = _target.MidPosition.X < this.MidPosition.X;
                if (_target.HP <= 0)
                    _target = null;
                if ((_target.MidPosition - _position).Length() < 26 && attackCooldown <= 0 &&!attack)
                {
                    _Move = false;
                    attack = true;
                    attackCooldown = 0.4f; 
                    IGORRProtocol.Messages.SetAnimationMessage sam = (IGORRProtocol.Messages.SetAnimationMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.SetAnimation);
                    sam.force = true;
                    sam.animationNumber = 0;
                    sam.objectID = ID;
                    sam.Encode();
                    _map.ObjectManager.Server.SendAllMap(_map, sam, true);
                }
            }
            if (attack && attackCooldown <= 0)
            {
                attackCooldown = 1;
                attack = false;
                _map.ObjectManager.SpawnAttack(ID, 1);
                _Move = Left != _target.MidPosition.X < this.MidPosition.X;
                Left = _target.MidPosition.X < this.MidPosition.X;
                IGORRProtocol.Messages.SetAnimationMessage sam = (IGORRProtocol.Messages.SetAnimationMessage)IGORRProtocol.Protocol.NewMessage(IGORRProtocol.MessageTypes.SetAnimation);
                sam.force = false;
                sam.animationNumber = 0;
                sam.objectID = ID;
                sam.Encode();
                _map.ObjectManager.Server.SendAllMap(_map, sam, true);
            }
            //If we've decided to move, move
            if (_Move)
            {
                if (Left)
                    Move(-1);
                else
                    Move(1);
            }
            attackCooldown -= seconds;
            base.Update(map, seconds);
        }

        public override Attack GetAttack(int id)
        {
            Rectangle startRect = this.Rect;
            startRect.Height -= 4;
            startRect.Y += 3;
            startRect.X += this.LookLeft ? -8 : 8;
            Attack att = new Attack(_map, 8, startRect, new Vector2(this.LookLeft ? -200 : 200, 0), 100, this.ID, this.GroupID, 5003);
            att.HitOnce = true;
            att.Penetrates = false;
            return att;
        }

        public override PartPickup GetDrop(Map map)
        {
            return null;
        }
    }
}
