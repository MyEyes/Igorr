using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR_Server.Logic
{
    class Striker:BodyPart
    {
        public Striker()
            : base()
        {
            attackID = 1;
        }

        public override string GetName()
        {
            return "Striker";
        }

        public override int GetID()
        {
            return 'd'-'a';
        }
    }
}
