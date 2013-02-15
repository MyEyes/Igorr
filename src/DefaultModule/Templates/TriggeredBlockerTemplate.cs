using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DefaultModule
{
    class TriggeredBlockerTemplate : IGORR.Modules.ObjectTemplate
    {

        public override IGORR.Server.Logic.GameObject CreateServer(IGORR.Server.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, System.IO.BinaryReader bin)
        {
            return new IGORR.Server.Logic.TurnOffBlocker(bin.ReadString(),bin.ReadBoolean(), map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override IGORR.Modules.ObjectControl GetEditorControl(BinaryReader reader)
        {
            return new EditorControls.TriggerInfoControl(reader);
        }

        public override int TypeID
        {
            get
            {
                return 10;
            }
        }
    }
}
