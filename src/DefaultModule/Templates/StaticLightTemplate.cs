using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DefaultModule
{
    class StaticLightTemplate : IGORR.Modules.ObjectTemplate
    {

        public override IGORR.Server.Logic.GameObject CreateServer(IGORR.Server.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, System.IO.BinaryReader bin)
        {
            string color = "" + (char)bin.ReadByte() + (char)bin.ReadByte() + (char)bin.ReadByte() + (char)bin.ReadByte();
            int radius = bin.ReadInt32();
            color += (char)(byte)(radius / 256);
            color += (char)(byte)(radius % 256);
            return new IGORR.Server.Logic.DummyObject(10003, map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID, color);
        }

        public override IGORR.Client.Logic.GameObject CreateClient(IGORR.Client.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, string info)
        {
            Microsoft.Xna.Framework.Color c = new Microsoft.Xna.Framework.Color((byte)info[1],(byte)info[2],(byte)info[3],(byte)info[0]);
            int radius = 256 * info[4] + info[5];
            return new IGORR.Client.Logic.StaticLight(map, null, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID, c,radius);
        }

        public override IGORR.Modules.ObjectControl GetEditorControl(BinaryReader reader)
        {
            return new EditorControls.LightColorControl(reader);
        }

        public override int TypeID
        {
            get
            {
                return 10003;
            }
        }
    }
}
