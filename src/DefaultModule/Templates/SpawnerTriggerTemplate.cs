using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefaultModule.Templates
{
    class TriggeredSpawnerTemplate : IGORR.Modules.ObjectTemplate
    {
        public override IGORR.Server.Logic.GameObject CreateServer(IGORR.Server.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, System.IO.BinaryReader bin)
        {
            return new Server.TriggeredEnemySpawner(map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID, bin.ReadInt32(), bin.ReadInt32(), bin.ReadInt32(), bin.ReadBoolean(), bin.ReadString(), bin.ReadBoolean(), bin.ReadString());
        }

        public override IGORR.Modules.ObjectControl GetEditorControl(System.IO.BinaryReader reader)
        {
            return new EditorControls.TriggeredSpawnerInfoControl(reader);
        }

        public override int TypeID
        {
            get
            {
                return 87;
            }
        }
    }
}
