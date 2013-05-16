using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{

    public class Tile : GameObject
    {
        public TileGeometry Geometry;
        Rectangle _selectRect;
        bool _collides;
        EventObject _eventObject;
        int textureWidth = 0;
        int tilesPerLine = 0;
        const int tileSize = 16;

        public Tile(Rectangle tile,Rectangle rect, Texture2D texture, bool collides)
            : base(texture, rect, -1)
        {
            _collides = collides;
            _selectRect = tile;
            textureWidth = texture.Width;
            tilesPerLine = textureWidth /tileSize;
        }

        public void SetChild(EventObject obj)
        {
            obj.SetParent(this);
            _eventObject = obj;
        }

        public void RemoveChild()
        {
            _eventObject.SetParent(null);
            _eventObject = null;
        }

        public void SetTile(int id)
        {
            int line = id / tilesPerLine;
            _selectRect = new Rectangle(tileSize * (id%tilesPerLine), line*tileSize, tileSize,tileSize);
        }

        public int GetTileID()
        {
            return _selectRect.X / 16;
        }

        public void SetCollides(bool val)
        {
            _collides = val;
        }

        public bool Colliding
        {
            get { return _collides; }
        }

        public void Draw(float depth, SpriteBatch batch)
        {
            batch.Draw(_texture, _rect, _selectRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, depth);
            if(_eventObject!=null)_eventObject.Draw(batch);
        }

        public virtual bool Collides(GameObject obj)
        {
            if (_collides)
                return base.Collides(obj);
            else return false;
        }

        public EventObject EventObject
        {
            get { return _eventObject; }
        }

    }
}
