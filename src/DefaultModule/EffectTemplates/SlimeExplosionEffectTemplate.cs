﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Modules;
using IGORR.Content;
using IGORR.Client.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DefaultModule.EffectTemplates
{
    class SlimeExplosionEffectTemplate : IGORR.Modules.EffectTemplate
    {
        Texture2D slimeTex;

        public override void DoEffect(IGORR.Client.Logic.IMap map, Microsoft.Xna.Framework.Vector2 dir, Microsoft.Xna.Framework.Point position, string info)
        {
            if(slimeTex==null)
                slimeTex=ContentInterface.LoadTexture("healthOrb");

            Vector2 pos = new Vector2(position.X, position.Y);
            ParticleInfo pInfo = new ParticleInfo();
            pInfo.collides = true;
            pInfo.sticky = true;

            map.SetGlow(-2, pos, Color.Green, 40, false, 300);
            Vector2 sideWays = new Vector2(dir.Y, -dir.X);
            if (slimeTex != null)
                for (int x = 0; x < 80; x++)
                    map.Particles.AddParticle(slimeTex, 0.7f, pos, 0.6f*dir*(float)_random.NextDouble() + (float)(-0.15f+0.3f*_random.NextDouble()) * sideWays, new Vector2(0, 100), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble()*0.7f + 0.4f, pInfo);
        }

        public override int EffectID
        {
            get
            {
                return 3;
            }
        }
    }
}