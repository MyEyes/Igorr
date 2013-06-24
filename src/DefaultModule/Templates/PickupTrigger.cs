﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefaultModule
{
    class PickupTriggerTemplate : IGORR.Modules.ObjectTemplate
    {
        public override IGORR.Server.Logic.GameObject CreateServer(IGORR.Server.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, System.IO.BinaryReader bin)
        {
            return new IGORR.Server.Logic.TriggerPickup(bin.ReadString(), bin.ReadBoolean(), bin.ReadString(), bin.ReadInt32(), bin.ReadString(), map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override IGORR.Client.Logic.GameObject CreateClient(IGORR.Client.Logic.IMap map, int objectID, Microsoft.Xna.Framework.Point p, string info)
        {
            return new IGORR.Client.Logic.PartPickup(new IGORR.Client.Logic.Body.BodyPart(IGORR.Content.ContentInterface.LoadTexture(info), IGORR.Client.Logic.Body.BodyPartType.None), map, new Microsoft.Xna.Framework.Rectangle(p.X, p.Y, 16, 16), objectID);
        }

        public override IGORR.Modules.ObjectControl GetEditorControl(System.IO.BinaryReader reader)
        {
            return new EditorControls.TriggerPickupInfoControl(reader);
        }


        public override int TypeID
        {
            get
            {
                return 89;
            }
        }
    }
}