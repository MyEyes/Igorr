using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client
{
    class Particle
    {
        Texture2D _texture;
        Vector2 _position;
        Vector2 _speed;
        Vector2 _acceleration;
        Vector2 _size;
        bool _alive;
        float _lifeTime;
        float _rotation;
        float _rotSpeed;
        float _sizeMod = 1;
        bool _stuck = false;
        const float dampening = 0.7f;
        ParticleInfo _info;

        public void Create(Texture2D tex, float lifeTime, Vector2 Position, Vector2 Speed, Vector2 Acceleration, float Rotation, float RotationSpeed, float SizeMod, ParticleInfo info)
        {
            _texture = tex;
            _lifeTime = lifeTime;
            _position = Position;
            _speed = Speed;
            _size.X = _texture.Width;
            _size.Y = _texture.Height;
            _acceleration = Acceleration;
            _rotation = Rotation;
            _rotSpeed = RotationSpeed;
            _alive = true;
            _stuck = false;
            _sizeMod = SizeMod;
            _info = info;
        }

        public void Update(Map map, float secs)
        {
            if (_info.attractor != null)
            {
                Vector2 diff = (_info.attractor.MidPosition - _position);
                float length = diff.Length();
                _acceleration = diff*_info.attraction / (length);
                if (length < 8)
                    _alive = false;
            }
            if (_info.collides)
            {
                _position.Y += _speed.Y * secs;
                if (map.Collides(_position))
                {
                    _position.Y -= _speed.Y * secs;
                    _speed.Y = -_speed.Y*dampening;
                    if (_info.sticky)
                        _stuck = true;
                }
                _position.X += _speed.X * secs;
                if (map.Collides(_position))
                {
                    _position.X -= _speed.X * secs;
                    _speed.X = -_speed.X*dampening;
                    if (_info.sticky)
                        _stuck = true;
                }
            }
            else
            {
                _position += _speed * secs;
            }
            _speed += _acceleration * secs;
            if (_info.maxSpeed > 0)
            {
                float len =_speed.Length();
                if (_speed.Length() > _info.maxSpeed)
                    _speed /= len / _info.maxSpeed;
            }
            if (_stuck)
                _speed = Vector2.Zero;
            _lifeTime -= secs;
            _rotation += _rotSpeed * secs;
            if (_lifeTime <= 0)
                _alive = false;
        }

        public void Draw(SpriteBatch batch)
        {
            if (_alive)
                batch.Draw(_texture, _position, null, Color.Lerp(Color.Transparent, Color.White, _lifeTime), _rotation, _size / 2, _sizeMod, SpriteEffects.None, 0.53f);
        }

        public bool Alive
        {
            get { return _alive; }
        }
    }
}
