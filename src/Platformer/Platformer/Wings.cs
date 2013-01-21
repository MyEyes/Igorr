using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client
{
    class Wings : BodyPart
    {
        public Wings(Texture2D tex)
            : base(tex)
        {
            airJumpMax = 4;
            airJumpStrength = 120f;
        }

        public override string GetName()
        {
            return "Wings";
        }

        public override int GetID()
        {
            return 'e' - 'a';
        }
    }
}
