using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Client.Logic;
using Microsoft.Xna.Framework;

namespace IGORR.Client.Logic
{
    class GlowPart:Body.UtilityPart
    {
        float currentPulse;
        float pulseMax = 20;
        float pulseSpeed = 0.02f;
        bool up = false;
        float strength;

        public GlowPart(Texture2D tex)
            : base(tex)
        {
            strength = 150;
        }

        public override void Update(Player player, float ms)
        {
            currentPulse += up ? ms * pulseSpeed : -ms * pulseSpeed;
            if (currentPulse > pulseMax)
                up = false;
            if(currentPulse<-pulseMax)
                up=true;
            player.Map.SetGlow(-1, player.MidPosition, Color.DarkGreen, strength + currentPulse, true, 2000);
        }

        public override string GetName()
        {
            return "GlowPart";
        }

        public override int GetID()
        {
            return 82;
        }
    }
}
