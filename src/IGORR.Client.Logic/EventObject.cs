using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    public class EventObject:GameObject
    {
        protected IMap _map;

        public EventObject(IMap map, Texture2D texture, Rectangle rect, int id):base(texture,rect, id)
        {
            _map = map;
        }

        public void SetParent(Tile obj)
        {
            
        }

        public void Remove()
        {
            _map.RemoveEvent(this);
        }

        public virtual void Event(Player obj)
        {

        }
    }
}
