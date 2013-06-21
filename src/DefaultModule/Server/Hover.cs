using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Server.Logic
{
    class Hover:Body.MovementPart
    {
        const float maxHoverTime = 1500;
        float currentHoverTime=0;

        public Hover()
            : base()
        {
            //speedBonus = 30;
            //jumpBonus = 100;
        }

        public override void Update(Player player, float ms)
        {

            if (player.OnGround)
            {
                currentHoverTime = 0;
            }
            else
            {
                Vector2 playerMovement = player.Speed;
                if (playerMovement.Y > 0 && currentHoverTime < maxHoverTime)
                {
                    currentHoverTime += ms;
                    playerMovement.Y = 0;
                    player.Speed = playerMovement;
                }
            }
        }

        public override string GetName()
        {
            return "Hover";
        }

        public override int GetID()
        {
            return 88;
        }
    }
}
