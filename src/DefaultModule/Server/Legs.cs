using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Server.Logic
{
    class Legs:Body.MovementPart
    {
        public Legs()
            : base()
        {
            //speedBonus = 30;
            //jumpBonus = 100;
        }

        public override void Jump(Player player, float strength)
        {
            if (player.OnGround)
            {
                Vector2 currentSpeed = player.Speed;
                currentSpeed.Y -= 100;
                player.Speed = currentSpeed;
            }
        }

        public override void Move(Player player, float dir, float yDir)
        {
            Vector2 currentSpeed = player.Speed;
            currentSpeed.X += 30 * dir;
            player.Speed = currentSpeed;
        }

        public override string GetName()
        {
            return "Legs";
        }

        public override int GetID()
        {
            return 'b'-'a';
        }
    }
}
