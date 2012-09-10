using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Legs:BodyPart
    {
        public Legs(Texture2D tex)
            : base(tex)
        {
            speedBonus = 30;
            jumpBonus = 100;
        }

        public override string GetName()
        {
            return "Legs";
        }

        public override int GetID()
        {
            return 1;
        }
    }
}
