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

        public virtual void Update(float ms)
        {

        }

        public virtual void Move(float x, float y)
        {

        }

        public virtual void Jump(float strength)
        {

        }
    }
}
