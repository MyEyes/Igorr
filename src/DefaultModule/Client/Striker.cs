﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    class Striker : Body.AttackPart
    {
        public Striker(Texture2D tex)
            : base(tex)
        {
        }

        public override string GetName()
        {
            return "Striker";
        }

        public override int GetID()
        {
            return 'd' - 'a';
        }
    }

}
