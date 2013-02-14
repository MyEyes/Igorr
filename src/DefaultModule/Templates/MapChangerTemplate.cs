using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefaultModule.Templates
{
    class MapChangerTemplate : IGORR.Modules.ObjectTemplate
    {
        public override IGORR.Server.Logic.GameObject CreateServer(IGORR.Server.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, System.IO.BinaryReader bin)
        {
            IGORR.Server.Logic.MapChanger changer = new IGORR.Server.Logic.MapChanger(map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
            changer.targetMapID = bin.ReadInt32();
            changer.targetPos = new Microsoft.Xna.Framework.Vector2(bin.ReadInt32(),bin.ReadInt32());
            return changer;
        }

        public override IGORR.Client.Logic.GameObject CreateClient(IGORR.Client.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, string info)
        {
            return new IGORR.Client.Logic.Exit(IGORR.Content.ContentInterface.LoadTexture("Exit"), map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override int TypeID
        {
            get
            {
                return 6;
            }
        }
    }
}
