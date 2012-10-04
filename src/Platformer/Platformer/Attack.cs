using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Game
{
    public struct AttackInfo
    {
        public bool Physics;
        public bool CollisionDespawn;
        public bool HitDespawn;
        public float LifeTime;
        public float bounceFactor;
    }

    class Attack : GameObject
    {
        public Vector2 Movement;
        public float lifeTime;
        public int parentID = -1;
        Vector2 _size;
        float _angle;
        AttackInfo _info;

        public Attack(Texture2D tex, Rectangle rect, Vector2 mov, int id, AttackInfo info)
            : base(tex, rect, id)
        {
            _info = info;
            Movement = mov;
            _size.X = tex.Width;
            _size.Y = tex.Height;
            _rect = rect;
            lifeTime = _info.LifeTime;
            _angle = (float)Math.Atan2(Movement.Y, Movement.X);
        }

        public bool Update(Map map, float seconds)
        {
            if (_info.Physics)
            {
                this.Movement.Y += Player.gravity * seconds;
                Move(new Vector2(0,Movement.Y*seconds));
                if (map.Collides(this))
                {
                    Move(new Vector2(0, -Movement.Y*seconds));
                    Movement.Y *= -_info.bounceFactor;
                    Movement.X *= _info.bounceFactor;
                }
                Move(new Vector2(Movement.X*seconds, 0));
                if (map.Collides(this))
                {
                    Move(new Vector2(-Movement.X*seconds,0));
                    Movement.X *= -_info.bounceFactor;
                    Movement.Y *= _info.bounceFactor;
                }
            }
            else
            {
                Move(Movement * seconds);
                if (_info.CollisionDespawn && map.Collides(this))
                    return false;
            }
            lifeTime -= seconds;
            _angle = (float)Math.Atan2(Movement.Y, Movement.X);
            return lifeTime>0;
        }

        public override void Draw(SpriteBatch batch)
        {
            Rectangle drawRect = _rect;
            drawRect.X += (int)_size.X / 2;
            drawRect.Y += (int)_size.Y / 2;
            batch.Draw(_texture, drawRect, null, Color.White, _angle, _size/2, (Movement.X < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0.45f);
        }
    }
}
