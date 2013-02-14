﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework;

namespace IGORR.Modules
{
    public class ObjectTemplate
    {
        public virtual Server.Logic.GameObject CreateServer(Server.Logic.IMap map, int objectID, Point p, BinaryReader bin)
        {
            return null;
        }

        public virtual Client.Logic.GameObject CreateClient(Client.Logic.IMap map, int objectID, Point p, string info)
        {
            return null;
        }

        public virtual ObjectControl GetEditorControl(byte[] bytes)
        {
            return null;
        }

        public virtual int TypeID
        {
            get { return -1; }
        }

    }
}
