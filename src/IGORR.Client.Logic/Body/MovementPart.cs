using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic.Body
{
    public class MovementPart:BodyPart
    {
        public MovementPart(Texture2D texture)
            : base(texture, BodyPartType.Movement)
        {

        }


        public virtual void Move(Player player, float dir)
        {

        }

        public virtual void Jump(Player player, float strength)
        {

        }
    }
}
