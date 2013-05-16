using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Server.Logic
{

    public class Tile : GameObject
    {
        Rectangle _selectRect;
        bool _collides;
        EventObject _eventObject;

        public Tile(IMap map,Rectangle tile,Rectangle rect,  bool collides)
            : base(map,rect, -1)
        {
            _collides = collides;
            _selectRect = tile;
        }

        public void SetChild(EventObject obj)
        {
            obj.SetParent(this);
            _eventObject = obj;
        }

        public void SetTile(bool collides,int id)
        {
            _selectRect = new Rectangle(16 * id, 0, 16, 16);
            SetCollides(collides);
            IGORR.Protocol.Messages.ChangeTileMessage ctm = (IGORR.Protocol.Messages.ChangeTileMessage)IGORR.Protocol.ProtocolHelper.NewMessage(IGORR.Protocol.MessageTypes.ChangeTile);
            ctm.x = _rect.X / 16;
            ctm.y = _rect.Y / 16;
            ctm.tileID = id;
            ctm.Encode();
            _map.ObjectManager.Server.SendAllMap(_map, ctm, true);
        }

        public void SetCollides(bool val)
        {
            _collides = val;
        }

        public void RemoveChild()
        {
            _eventObject.SetParent(null);
            _eventObject = null;
        }

        public override bool Collides(GameObject obj)
        {
            return base.Collides(obj);
        }

        public EventObject EventObject
        {
            get { return _eventObject; }
        }

        public int TileID { get { return _selectRect.X / 16; } }
    }
}
