﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Server.Logic;

namespace DefaultModule
{
    class VaseTemplate : IGORR.Modules.ObjectTemplate
    {
        public override IGORR.Server.Logic.GameObject CreateServer(IMap map, int objectID, Microsoft.Xna.Framework.Point p, System.IO.BinaryReader bin)
        {
            return new IGORR.Server.Logic.Vase(map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override IGORR.Client.Logic.GameObject CreateClient(IGORR.Client.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, string info)
        {
            return new IGORR.Client.Logic.EventObject(map, IGORR.Content.ContentInterface.LoadTexture("Vase"), new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override int TypeID
        {
            get
            {
                return 3000;
            }
        }
    }
}
