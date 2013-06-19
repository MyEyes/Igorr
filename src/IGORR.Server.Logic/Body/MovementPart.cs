using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Server.Logic.Body
{
    public class MovementPart:BodyPart
    {
        public MovementPart()
            : base(BodyPartType.Movement)
        {

        }


        public virtual void Move(Player player, float dir, float yDir)
        {

        }

        public virtual void Jump(Player player, float strength)
        {

        }
    }
}
