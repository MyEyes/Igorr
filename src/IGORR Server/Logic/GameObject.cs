using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;

namespace IGORR_Server.Logic
{
    public class GameObject
    {
        protected static Random _random = new Random();
        protected Rectangle _rect;
        protected Vector2 _position;
        protected Vector2 _movement;
        protected int _id = 0;
        protected int _objectType = -1;
        protected string _name;
        protected Map _map;
        protected long lastUpdate = 0;

        public GameObject(Map map, Rectangle boundingRect, int id)
        {
            _rect = boundingRect;
            _map = map;
            Position = new Vector2(boundingRect.X, boundingRect.Y);
            _id = id;
        }

        public void SetUpdateTime(long time)
        {
            lastUpdate = time;
        }

        public long LastUpdate
        {
            get { return lastUpdate; }
        }

        public void Move(Vector2 diff)
        {
            Position = _position + diff;
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
            return obj._rect.Intersects(this._rect);
        }

        public virtual void SendInfo(NetConnection connection)
        {

        }

        public int ID
        {
            get { return _id; }
        }

        public int ObjectType
        {
            get { return _objectType; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Vector2 Movement
        {
            get { return _movement; }
        }

        public Rectangle Rect
        {
            get { return _rect; }
        }

        public Map map
        {
            get { return _map; }
        }
    }
}
