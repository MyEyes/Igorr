using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR.Server.Logic
{
    class Wings : Body.MovementPart
    {
        public Wings()
            : base()
        {
            //airJumpMax = 6;
            //airJumpStrength = 60f;
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
