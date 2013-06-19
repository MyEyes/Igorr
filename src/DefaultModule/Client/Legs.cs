using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Client.Logic;
using Microsoft.Xna.Framework;

namespace IGORR.Client.Logic
{
    class Legs:Body.MovementPart
    {
        float speedBonus;
        float jumpBonus;

        public Legs(Texture2D tex)
            : base(tex)
        {
            speedBonus = 30;
            jumpBonus = 100;
        }

        public override void Jump(Player player, float strength)
        {
            if (player.OnGround)
            {
                Vector2 newSpeed = player.Speed;
                newSpeed.Y -= jumpBonus;
                player.Speed = newSpeed;
            }
        }

        public override void Move(Player player, float dir, float yDir)
        {
            Vector2 newSpeed = player.Speed;
            newSpeed.X += speedBonus * dir;
            player.Speed = newSpeed;
        }

        public override string GetName()
        {
            return "Legs";
        }

        public override int GetID()
        {
            return 1;
        }
    }
}
