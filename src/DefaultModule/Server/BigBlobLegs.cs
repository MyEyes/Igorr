using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Server.Logic
{
    class BigBlobLegs:Body.MovementPart
    {
        public BigBlobLegs()
            : base()
        {
            //speedBonus = 10;
            //jumpBonus = 50;
        }

        public override string GetName()
        {
            return "BigBlobLegs";
        }

        public override int GetID()
        {
            return 1;
        }
    }
}
