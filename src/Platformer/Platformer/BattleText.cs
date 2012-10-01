using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Game
{
    class BattleText
    {
        string _text;
        Vector2 _position;
        Vector2 _size;
        Vector2 _speed;
        float _timeLeft;
        float _startTime;
        float _gravity;
        Color _startColor;
        Color _endColor;
        static Random _random;

        public BattleText(Vector2 position, float gravity, string text, float startTime, Color start, Color end, SpriteFont font)
        {
            if (_random == null)
                _random = new Random();
            _gravity = gravity;
            _speed.X = _random.Next(-(int)(gravity / 5), (int)(gravity / 5));
            _speed.Y = -gravity/2;
            _text = text;
            _timeLeft = startTime;
            _startTime = startTime;
            _startColor = start;
            _endColor = end;
            _position = position;
            _size = font.MeasureString(text) / 2;
        }

        public bool Update(float ms)
        {
            _timeLeft -= ms;
            _speed.Y += ms * _gravity / 1000.0f;
            _position += _speed * ms / 1000f;
            return _timeLeft > 0;
        }

        public void Draw(SpriteFont font, SpriteBatch batch)
        {
            //batch.DrawString(font, _text, _position - _size * 1.1f, Color.Lerp(Color.Black, _endColor, 1 - _timeLeft / _startTime), 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            batch.DrawString(font, _text, _position - _size, Color.Lerp(_startColor, _endColor, 1 - _timeLeft / _startTime), 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0);
        }
    }
}
