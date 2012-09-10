﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR_Server.Logic
{
    public class EventObject:GameObject
    {
        protected Tile _parent;
        public static Server server;

        public EventObject(Map map, Rectangle rect, int id):base(map,rect, id)
        {
            _map = map;
        }

        public void SetParent(Tile obj)
        {
            _parent = obj;
        }

        public virtual void Update(float ms)
        {

        }

        public virtual void Event(Player obj)
        {

        }
    }
}
