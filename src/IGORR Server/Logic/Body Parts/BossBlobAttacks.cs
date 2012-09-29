using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR_Server.Logic
{
    class BossBlobAttack1:BodyPart
    {
        public BossBlobAttack1()
            : base()
        {
            attackID = 1;
        }
        public override int GetID()
        {
            return 2;
        }
    }

    class BossBlobAttack2 : BodyPart
    {
        public BossBlobAttack2()
            : base()
        {
            attackID = 2;
        }

        public override int GetID()
        {
            return 3;
        }
    }

    class BossBlobAttack3 : BodyPart
    {
        public BossBlobAttack3()
            : base()
        {
            attackID = 3;
        }

        public override int GetID()
        {
            return 4;
        }
    }
}
