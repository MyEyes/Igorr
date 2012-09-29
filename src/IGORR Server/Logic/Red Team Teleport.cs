using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic
{
    class RedTeamTeleporter:EventObject
    {
        public Vector2 targetPos= new Vector2(100,100);
        public int targetMapID = 1;
        public RedTeamTeleporter(Map map, Rectangle targetRect, int id)
            : base(map, targetRect, id)
        {
            _objectType = 26;
        }

        public override void Event(Player obj)
        {
            if (obj.GroupID == 3)
                return;
            obj.SetTeam(2);
            if (server != null)
                server.ChangePlayerMap(obj, targetMapID, targetPos);
            else if(_map.ObjectManager.Server!=null)
                _map.ObjectManager.Server.ChangePlayerMap(obj, targetMapID, targetPos);
        }
        
    }
}
