using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Client.Logic.Body;
using Microsoft.Xna.Framework;

namespace IGORR.Client.Logic
{
    class Wings : MovementPart
    {
        float airJumpStrength = 120;
        int airJumpMax = 4;
        int airJumps = 0;

        public Wings(Texture2D tex)
            : base(tex)
        {
        }

        public override void Update(Player player, float ms)
        {
            if (player.OnGround)
                airJumps = 0;
        }

        public override void Jump(Player player, float strength)
        {
            if (!player.OnGround && airJumps<airJumpMax)
            {
                Vector2 newSpeed = player.Speed;
                newSpeed.Y += - airJumpStrength;
                if(newSpeed.Y<-airJumpStrength) newSpeed.Y=-airJumpStrength;
                player.Speed = newSpeed;
                airJumps++;
            }
        }

        public override string GetName()
        {
            return "Wings";
        }

        public override int GetID()
        {
            return 'e' - 'a';
        }
    }
}
