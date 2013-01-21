using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client
{
    public class GameObject
    {
        protected Rectangle _rect;
        protected Vector2 _position;
        protected Texture2D _texture;
        protected int _id = 0;
        protected long lastUpdate = 0;

        public GameObject(Texture2D texture, Rectangle boundingRect, int id)
        {
            _rect = boundingRect;
            Position = new Vector2(boundingRect.X, boundingRect.Y);
            _texture = texture;
            _id = id;
        }

        public void Move(Vector2 diff)
        {
            Position = _position + diff;
        }

        public void SetUpdateTime(long time)
        {
            lastUpdate = time;
        }

        public long LastUpdate
        {
            get { return lastUpdate; }
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            Position = _position;
        }

        public Vector2 Position
        {
            protected set { _position = value; _rect.X = (int)_position.X; _rect.Y = (int)_position.Y; }
            get { return _position; }
        }

        public Vector2 MidPosition
        {
            get { return _position + new Vector2(_rect.Width, _rect.Height) / 2; }
        }

        public virtual bool Collides(GameObject obj)
        {
            return obj.Rect.Intersects(this._rect);
        }

        public virtual void Update(float ms)
        {

        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(_texture, _rect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.43f);
        }

        public int ID
        {
            get { return _id; }
        }

        public Rectangle Rect
        {
            get { return _rect; }
        }

        public virtual void GetInfo(string info)
        {

        }
    }
}
