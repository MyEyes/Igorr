﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefaultModule
{
    class ShadowOnTemplate:IGORR.Modules.ObjectTemplate
    {
        public override IGORR.Server.Logic.GameObject CreateServer(IGORR.Server.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, System.IO.BinaryReader bin)
        {
            return new IGORR.Server.Logic.ShadowOn(map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override int TypeID
        {
            get
            {
                return 13;
            }
        }
    }
}
