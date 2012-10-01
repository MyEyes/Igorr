using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR_Server.Logic
{
    class BossBlobLegs:BodyPart
    {
        public BossBlobLegs()
            : base()
        {
            speedBonus = 80;
            jumpBonus = 180;
        }

        public override string GetName()
        {
            return "BossBlobLegs";
        }

        public override int GetID()
        {
            return 'b'-'a';
        }
    }
}
