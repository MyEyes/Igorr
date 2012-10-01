using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR_Server.Logic.Body_Parts
{
    public class Builder : BodyPart
    {
        public Builder()
            : base()
        {
        }

        public override string GetName()
        {
            return "Builder Glove";
        }

        public override int GetID()
        {
            return 21;
        }
    }
}
