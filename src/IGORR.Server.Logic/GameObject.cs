using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;

namespace IGORR.Server.Logic
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
        protected string _info = "";
        protected IMap _map;
        protected long lastUpdate = 0;

        public GameObject(IMap map, Rectangle boundingRect, int id)
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
            return obj.Rect.Intersects(this._rect);
        }

        public virtual void SendInfo(NetConnection connection)
        {

        }

        public void DoEffect(int EffectID, Point pos, Vector2 dir, string info)
        {
            if(_map==null)
                return;
            IGORR.Protocol.Messages.DoEffectMessage dem = (IGORR.Protocol.Messages.DoEffectMessage)map.ObjectManager.Server.ProtocolHelper.NewMessage(Protocol.MessageTypes.DoEffect);
            dem.Info = info;
            dem.Position = pos;
            dem.Dir = dir;
            dem.EffectID = EffectID;
            dem.Encode();
            _map.ObjectManager.Server.SendAllMap(_map, dem, true);
        }

        public void Say(string Text, float timeout)
        {
            if (map == null)
                return;
            IGORR.Protocol.Messages.ChatMessage cm = (IGORR.Protocol.Messages.ChatMessage)map.ObjectManager.Server.ProtocolHelper.NewMessage(Protocol.MessageTypes.Chat);
            cm.Text = Text;
            cm.timeout = timeout;
            cm.objID = this._id;
            cm.Encode();
            _map.ObjectManager.Server.SendAllMap(_map, cm, true);
        }

        public void Say(string Text, float timeout, Player player)
        {
            if (map == null)
                return;
            IGORR.Protocol.Messages.ChatMessage cm = (IGORR.Protocol.Messages.ChatMessage)map.ObjectManager.Server.ProtocolHelper.NewMessage(Protocol.MessageTypes.Chat);
            cm.Text = Text;
            cm.timeout = timeout;
            cm.objID = this._id;
            cm.Encode();
            _map.ObjectManager.Server.SendClient(player, cm);
        }

        public void Ask(string questionString, Player player)
        {
            if (map == null)
                return;
            IGORR.Protocol.Messages.InteractMessage im = (IGORR.Protocol.Messages.InteractMessage)map.ObjectManager.Server.ProtocolHelper.NewMessage(Protocol.MessageTypes.Interact);
            im.action = Protocol.Messages.InteractAction.Ask;
            im.objectID = this._id;
            im.sinfo = questionString;
            im.Encode();
            _map.ObjectManager.Server.SendClient(player, im);
        }

        public virtual void Interact(Player player, string sinfo, int info)
        {

        }

        public virtual void Update(float ms)
        {

        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
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

        public virtual string Name
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

        public IMap map
        {
            get { return _map; }
        }

        public string Info
        {
            get { return _info; }
        }

        public virtual void GetInfo(string info)
        {

        }
    }
}
