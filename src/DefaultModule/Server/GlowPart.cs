using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefaultModule.Server
{
    class GlowPart:IGORR.Server.Logic.Body.UtilityPart
    {
        public override string GetName()
        {
            return "Glow";
        }

        public override int GetID()
        {
            return 82;
        }
    }
}
