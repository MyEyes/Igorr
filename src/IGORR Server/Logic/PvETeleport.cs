using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic
{
    class PvETeleporter:EventObject
    {
        public Vector2 targetPos= new Vector2(100,100);
        public int targetMapID = 1;
        public PvETeleporter(Map map, Rectangle targetRect, int id)
            : base(map, targetRect, id)
        {
            _objectType = 28;
        }

        public override void Event(Player obj)
        {
            obj.SetTeam(1);
            if (server != null)
                server.ChangePlayerMap(obj, targetMapID, targetPos);
            else if(_map.ObjectManager.Server!=null)
                _map.ObjectManager.Server.ChangePlayerMap(obj, targetMapID, targetPos);
        }
        
    }
}
