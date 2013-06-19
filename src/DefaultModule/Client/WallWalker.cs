using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Client.Logic;
using Microsoft.Xna.Framework;

namespace IGORR.Client.Logic
{
    class WallWalker:Body.MovementPart
    {

        float speedBonus;
        bool wasOnWall = false;
        public WallWalker(Texture2D tex)
            : base(tex)
        {
            speedBonus = 80;
        }

        public override void Jump(Player player, float strength)
        {
        }

        public override void Move(Player player, float dir, float yDir)
        {
            if (wasOnWall && !player.OnWall)
            {
                Vector2 speed = player.Speed;
                speed.Y = 0;
                player.Speed = speed;
            }
            if (player.OnWall)
            {
                Vector2 speed = player.Speed;
                speed.Y += yDir * speedBonus;
                player.Speed = speed;
                wasOnWall = true;
            }
            else
                wasOnWall = false;
        }

        public override string GetName()
        {
            return "WallWalker";
        }

        public override int GetID()
        {
            return 81;
        }
    }
}
