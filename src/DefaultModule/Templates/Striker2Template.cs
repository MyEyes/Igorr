using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefaultModule.Templates
{
    class PiercerTemplate : IGORR.Modules.ObjectTemplate
    {
        public override IGORR.Server.Logic.GameObject CreateServer(IGORR.Server.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, System.IO.BinaryReader bin)
        {
            return new IGORR.Server.Logic.PartPickup(new IGORR.Server.Logic.Piercer(),map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID, true);
        }

        public override IGORR.Client.Logic.GameObject CreateClient(IGORR.Client.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, string info)
        {
            return new IGORR.Client.Logic.PartPickup(new IGORR.Client.Logic.Piercer(IGORR.Content.ContentInterface.LoadTexture("Piercer")), map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override int TypeID
        {
            get
            {
                return 9;
            }
        }
    }
}
