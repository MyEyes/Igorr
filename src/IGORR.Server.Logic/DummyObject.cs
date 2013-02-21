using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;

namespace IGORR.Server.Logic
{
    public class DummyObject:GameObject
    {

        public DummyObject(int typeID, IMap map, Rectangle boundingRect, int id):base(map,boundingRect,id)
        {
            _objectType = typeID;
            _id = id;
        }

    }
}
