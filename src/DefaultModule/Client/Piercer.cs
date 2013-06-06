using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    class Piercer : Body.AttackPart
    {
        public Piercer(Texture2D tex)
            : base(tex)
        {
        }

        public override string GetName()
        {
            return "Piercer";
        }

        public override int GetID()
        {
            return 9;
        }
    }

}
