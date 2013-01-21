using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR.Server.Logic
{
    public class BaseBody:BodyPart
    {
        public int Level = 1;
        public int Exp = 0;
        public int ExpToNextLevel = 70;
        public int LastExp = 0;

        public BaseBody()
            : base()
        {
            DefBonus = 3;
            AttBonus = 5;
            speedBonus = 60;
        }

    }
}
