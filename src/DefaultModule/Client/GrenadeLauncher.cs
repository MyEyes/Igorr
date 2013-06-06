using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    public class GrenadeLauncher : Body.AttackPart
    {
        public GrenadeLauncher(Texture2D tex)
            : base(tex)
        {
        }

        public override string GetName()
        {
            return "Egg Launcher";
        }


        public override int GetID()
        {
            return 80;
        }
    }
}
