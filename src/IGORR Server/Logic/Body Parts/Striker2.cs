using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR_Server.Logic
{
    public class Striker2 : BodyPart
    {
        public Striker2()
            : base()
        {
            attackID = 2;
        }

        public override string GetName()
        {
            return "Striker2";
        }

        public override int GetID()
        {
            return 9;
        }
    }
}
