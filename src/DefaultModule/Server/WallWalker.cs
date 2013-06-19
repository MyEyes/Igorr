using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Server.Logic
{
    class WallWalker:Body.MovementPart
    {
        float speedBonus;
        public WallWalker()
            : base()
        {
            speedBonus = 80;
            //jumpBonus = 100;
        }

        public override void Jump(Player player, float strength)
        {
        }

        public override void Move(Player player, float dir, float yDir)
        {
            if (player.OnWall)
            {
                Vector2 speed = player.Speed;
                speed.Y += yDir*speedBonus;
                player.Speed = speed;
            }
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
