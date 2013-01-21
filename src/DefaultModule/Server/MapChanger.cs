using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class MapChanger:EventObject
    {
        public Vector2 targetPos= new Vector2(100,100);
        public int targetMapID = 1;
        public MapChanger(IMap map, Rectangle targetRect, int id)
            : base(map, targetRect, id)
        {
            _objectType = 'g' - 'a';
        }

        public override void Event(Player obj)
        {
            if (server != null)
                server.ChangePlayerMap(obj, targetMapID, targetPos);
            else if(_map.ObjectManager.Server!=null)
                _map.ObjectManager.Server.ChangePlayerMap(obj, targetMapID, targetPos);
        }
        
    }
}
