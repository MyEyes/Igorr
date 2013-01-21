using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using IGORR.Modules;
using System.Windows.Forms;

namespace DefaultModule
{
    public class TouchTriggerTemplate : ObjectTemplate
    {
        public override IGORR.ObjectInterface CreateClient(int objectID, Point p, BinaryReader bin)
        {
            return base.CreateClient(objectID, p, bin);
        }

        public override IGORR.ObjectInterface CreateServer(int objectID, Point p, BinaryReader bin)
        {
            return base.CreateServer(objectID, p, bin);
        }

        public override Control GetEditorControl()
        {
            return base.GetEditorControl();
        }
    }
}
