using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework;
using IGORR.Client.Logic;

namespace IGORR.Modules
{
    public class EffectTemplate
    {
        protected static Random _random = new Random();
        public virtual void DoEffect(IMap map, Vector2 dir, Point position, string info)
        {
            
        }
        
        public virtual int EffectID
        {
            get { return -1; }
        }

    }
}
