using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Content;
using IGORR.Client.Logic;

namespace DefaultModule.EffectTemplates
{
    class SlimeTrailEffectTemplate:IGORR.Modules.EffectTemplate

    {
        Texture2D tex1;
        Texture2D tex2;


        public override void DoEffect(IGORR.Client.Logic.IMap map, Vector2 dir, Point position, string info)
        {
            if (tex1 == null)
            {
                tex1 = ContentInterface.LoadTexture("splat");
                tex2 = ContentInterface.LoadTexture("healthOrb");
            }

            ParticleInfo PInfo = new ParticleInfo();
            PInfo.collides = true;
            PInfo.sticky = false;
            for (int x = 0; x < 1; x++)
            {
                map.Particles.AddParticle(tex1, 0.3f + (float)_random.NextDouble() * 0.2f, new Vector2(position.X, position.Y), Vector2.Zero, new Vector2(0, 0), (float)_random.NextDouble() * 6, (float)_random.NextDouble() * 6, 0.5f, PInfo);
                map.Particles.AddParticle(tex2, 0.6f + (float)_random.NextDouble() * 0.2f, new Vector2(position.X, position.Y), Vector2.Zero, new Vector2(0, 0), (float)_random.NextDouble() * 6, (float)_random.NextDouble() * 6, 0.5f, PInfo);
            }

        }

        public override int EffectID
        {
            get
            {

                return 4;
            }
        }
    }
}
