using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Client.Logic;
using Microsoft.Xna.Framework;

namespace IGORR.Client.Logic
{
    class Hover:Body.MovementPart
    {
        const float maxHoverTime=1500;
        float currentHoverTime;

        public Hover(Texture2D tex)
            : base(tex)
        {
            
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
                if (playerMovement.Y > 0  && currentHoverTime < maxHoverTime)
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
