using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR_Server.Logic
{
    class Legs:BodyPart
    {
        public Legs()
            : base()
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
            return 'b'-'a';
        }
    }
}
