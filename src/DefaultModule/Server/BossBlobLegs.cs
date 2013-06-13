using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Server.Logic
{
    class BossBlobLegs:Body.MovementPart
    {
        public BossBlobLegs()
            : base()
        {
            //speedBonus = 80;
            //jumpBonus = 180;
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

        public override void Move(Player player, float dir)
        {
            Vector2 currentSpeed = player.Speed;
            currentSpeed.X += 20*dir;
            player.Speed = currentSpeed;
        }

        public override string GetName()
        {
            return "BossBlobLegs";
        }

        public override int GetID()
        {
            return 'b'-'a';
        }
    }
}
