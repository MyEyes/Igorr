using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic.AI
{
    class SlimeShooter : NPC
    {
        float moveCountdown = 1;
        bool left;
        Point spawnPoint;
        Rectangle targetRect;
        Player _target = null;
        bool _Move = false;
        bool Left = false;
        bool attack = false;
        float changeCountdown = 1f;
        float attackCooldown = 0f;

        public SlimeShooter(IMap map,Rectangle spawnRect, int id)
            : base(map,spawnRect, id)
        {
            _XPBonus = 25;
            GivePart(new Legs());
            Logic.Body.AttackPart part = new SlimeLauncher();
            GivePart(part);
            EquipAttack(0, part);
            _groupID = 5;

            targetRect = new Rectangle(spawnRect.X - 224, spawnRect.Y - 160, 448, 320);
            spawnPoint = new Point(spawnRect.X + spawnRect.Width / 2, spawnRect.Y + spawnRect.Height / 2);
            _objectType = 5000;
        }

        public SlimeShooter(IMap map, string charfile, Rectangle spawnRect, int id)
            : base(map, charfile, spawnRect, id)
        {
            GivePart(new Legs());
            Body.AttackPart part = new SlimeLauncher();
            GivePart(part);
            _groupID = 5;
            _XPBonus = 25;
            EquipAttack(0, part);
            targetRect = new Rectangle(spawnRect.X - 224, spawnRect.Y - 160, 448, 320);
            spawnPoint = new Point(spawnRect.X + spawnRect.Width / 2, spawnRect.Y + spawnRect.Height / 2);
            _objectType = 5000;
        }

        public override void Update(IMap map, float seconds)
        {
            //Try to find target
            if (_target == null)
            {
                _target = _map.ObjectManager.GetPlayerInArea(targetRect);
            }

            //If you get out of the follow area or your target does, do this
            if (_target!=null && !targetRect.Contains((int)_target.MidPosition.X, (int)_target.MidPosition.Y))
            {
                _target = null;
                Left = this.MidPosition.X > spawnPoint.X;
                SetAnimation(false,0);
            }

            float dx = 0;
            float dy = 0;
            /*
            if (_target != null)
            {
                dx = Math.Abs(_target.MidPosition.X - this.MidPosition.X);
                dy = -(_target.MidPosition.Y - this.MidPosition.Y);
                if (dx < dy)
                {
                    _target = null;
                    SetAnimation(false, 0);
                }
            }
             */
            if (_target != null &&!attack)
            {
                    
                Left = _target.MidPosition.X < this.MidPosition.X;
                //Cheat to make enemy face in the right direction, look wrong way first, then the right way, this way we don't actually move
                Move(!Left ? -0.01f : 0.01f,0);
                Move(Left ? -0.01f : 0.01f,0);
                if (_target.HP <= 0)
                    _target = null;
                if (attackCooldown <= 0 &&!attack)
                {
                    _Move = false;
                    attack = true;
                    attackCooldown = 0.4f;
                    SetAnimation(true, 0);
                }
            }
            if (_target != null && attack && attackCooldown <= 0)
            {
                attackCooldown = 3.5f;
                attack = false;
                //Calculate strength of shot.

                //Shooting at 45° for simplicity:
                //px=t*s -> t=px/s, px=dx -> t=dx/s
                //vy=s-t*g
                //py=t*s-1/2t^2*g, py=dx-0.5f*(dx^2/s^2)*g -> s^2*dy=s^2*dx-0.5f*dx^2*g ->s^2*(dy-dx)=-0.5f*dx^2*g
                //s^2=-0.5f*dx^2*g/(dy-dx)
                //s=sqrt(-0.5f*dx^2*g/(dy-dx)

                //Shooting so that thing hits after some time t
                //vx=s*cos(a)
                //vy=s*sin(a)-t*g
                //px=t*s*cos(a)=dx
                //py=t*s*sin(a)-0.5f*t^2*g=dy
                //dx=t*s*cos(a) -> s=dx/(t*cos(a)
                //dy=dx/(cos(a))*sin(a)-0.5f*t^2*g
                //dy=dx*tan(a)-0.5f*t^2*g ->(dy-0.5f*t^2*g)/dx=tan(a) -> atan((dy-0.5f*t^2*g)/dx)=a
                //t*s*sin(a)-0.5f*t^2*g=dy -> s= (dy-0.5f*t^2*g)/(t*sin(a))

                /*
                float s = (float)Math.Sqrt(0.5f * Math.Abs((dx * dx * gravity) / (dy - dx)));
                s = s > 200 ? 200 : s;
                _Move = Left != _target.MidPosition.X < this.MidPosition.X;
                Left = _target.MidPosition.X < this.MidPosition.X;
                _map.ObjectManager.SpawnAttack(ID, s * new Vector2(Left ? -1 : 1, -1), 0, 0);
                 */
                float t = 0.75f + (float)_random.NextDouble();
                Vector2 diff = _target.MidPosition - this.MidPosition;
                diff.Normalize();
                diff = new Vector2(diff.Y, -diff.X);
                float dist = 48 *(float)(-1 + 2 * _random.NextDouble());

                dx = _target.MidPosition.X + t * _target.LastSpeed.X + diff.X * dist - this.MidPosition.X;
                dy = (_target.MidPosition.Y + t * _target.LastSpeed.Y + diff.Y * dist - this.MidPosition.Y);
                float a = (float)Math.Atan((dy - 0.5f * t * t * gravity) / dx);
                float s = (float)(dx / (t * (float)Math.Cos(a)));
                _map.ObjectManager.SpawnAttack(ID, s * new Vector2((float)Math.Cos(a), (float)Math.Sin(a)), 0, 0);
                SetAnimation(false, 0);
            }
            attackCooldown -= seconds;
            base.Update(map, seconds);
        }
    }
}
