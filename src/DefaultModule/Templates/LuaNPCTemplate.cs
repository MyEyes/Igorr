using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Server.Logic;

namespace DefaultModule
{
    class LuaNPCTemplate:IGORR.Modules.ObjectTemplate
    {
        public override IGORR.Server.Logic.GameObject CreateServer(IMap map, int objectID, Microsoft.Xna.Framework.Point p, System.IO.BinaryReader bin)
        {
            return new IGORR.Server.Logic.AI.LuaNPC(bin.ReadString(), map,  bin.ReadString(), new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override IGORR.Client.Logic.GameObject CreateClient(IGORR.Client.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, string info)
        {
            return new IGORR.Client.Logic.Player(info, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override IGORR.Modules.ObjectControl GetEditorControl(System.IO.BinaryReader reader)
        {
            return new EditorControls.LuaNPCControl(reader);
        }

        public override int TypeID
        {
            get
            {
                return 10001;
            }
        }
    }
}
