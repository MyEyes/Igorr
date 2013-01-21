using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class BlueTeamTeleporter:EventObject
    {
        public Vector2 targetPos= new Vector2(100,100);
        public int targetMapID = 1;
        public BlueTeamTeleporter(IMap map, Rectangle targetRect, int id)
            : base(map, targetRect, id)
        {
            _objectType = 27;
        }

        public override void Event(Player obj)
        {
            if (obj.GroupID == 2)
                return;
            obj.SetTeam(3);
            if (server != null)
                server.ChangePlayerMap(obj, targetMapID, targetPos);
            else if(_map.ObjectManager.Server!=null)
                _map.ObjectManager.Server.ChangePlayerMap(obj, targetMapID, targetPos);
        }
        
    }
}
