using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic.Body
{
    public class BaseBody:BodyPart
    {
        public BaseBody(Texture2D texture)
            : base(texture, BodyPartType.BaseBody)
        {

        }
    }
}
