using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR_Server.Logic
{
    public class GrenadeLauncher : BodyPart
    {
        public GrenadeLauncher()
            : base()
        {
            attackID = 3;
        }

        public override string GetName()
        {
            return "Grenade Launcher";
        }

        public override int GetID()
        {
            return 80;
        }
    }
}
