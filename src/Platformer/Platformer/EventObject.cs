using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client
{
    public class EventObject:GameObject
    {
        protected Map _map;

        public EventObject(Map map, Texture2D texture, Rectangle rect, int id):base(texture,rect, id)
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

        public virtual void Update(float ms)
        {

        }

        public virtual void Event(Player obj)
        {

        }
    }
}
