using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR_Server.Logic
{
    class Wings : BodyPart
    {
        public Wings()
            : base()
        {
            airJumpMax = 6;
            airJumpStrength = 60f;
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
