using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework;

namespace IGORR.Modules
{
    public class AttackTemplate
    {
        public virtual Client.Logic.Attack CreateClient(int objectID, Vector2 dir, Point position, string info)
        {
            return null;
        }
        
        public virtual int TypeID
        {
            get { return -1; }
        }

    }
}
