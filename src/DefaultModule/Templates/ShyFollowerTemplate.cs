using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefaultModule
{
    class ShyFollowerTemplate : IGORR.Modules.ObjectTemplate
    {
        public override IGORR.Server.Logic.GameObject CreateServer(IGORR.Server.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, System.IO.BinaryReader bin)
        {
            return new IGORR.Server.Logic.AI.ShyFollower(map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 8, 8), objectID);
        }

        public override IGORR.Client.Logic.GameObject CreateClient(IGORR.Client.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, string info)
        {
            return new IGORR.Client.Logic.Player("shy", new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 8, 8), objectID);
        }

        public override int TypeID
        {
            get
            {
                return 5005;
            }
        }
    }
}
