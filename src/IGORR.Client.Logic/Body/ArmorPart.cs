using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic.Body
{
    public class ArmorPart:BodyPart
    {
        public ArmorPart(Texture2D texture)
            : base(texture, BodyPartType.Armor)
        {

        }
    }
}
