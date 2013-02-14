using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Modules;

namespace DefaultModule.Templates
{
    class PlayerTemplate : ObjectTemplate
    {
        public override IGORR.Client.Logic.GameObject CreateClient(IGORR.Client.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, string info)
        {
            return new IGORR.Client.Logic.Player("default", new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override int TypeID
        {
            get
            {
                return 0;
            }
        }
    }
}
