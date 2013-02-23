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
    class VaseDestroyedEffectTemplate:IGORR.Modules.EffectTemplate

    {
        Texture2D shard1, shard2, shard3, shard4;


        public override void DoEffect(IGORR.Client.Logic.IMap map, Vector2 dir, Point position, string info)
        {
            if (shard1 == null)
            {
                shard1 = ContentInterface.LoadTexture("shard1");
                shard2 = ContentInterface.LoadTexture("shard2");
                shard3 = ContentInterface.LoadTexture("shard3");
                shard4 = ContentInterface.LoadTexture("shard4");
            }

            ParticleInfo PInfo = new ParticleInfo();
            PInfo.collides = true;
            PInfo.sticky = false;
            for (int x = 0; x < 2; x++)
            {
                map.Particles.AddParticle(shard1, 2.5f, new Vector2(position.X + (float)_random.NextDouble() * 16 - 8, position.Y + (float)_random.NextDouble() * 16 - 8), new Vector2((float)_random.NextDouble() * 70 - 35, (float)_random.NextDouble() * (-15) - 40), new Vector2(0, 100), (float)_random.NextDouble() * 6, (float)_random.NextDouble() * 6, 0.5f, PInfo);
                map.Particles.AddParticle(shard2, 2.5f, new Vector2(position.X + (float)_random.NextDouble() * 16 - 8, position.Y + (float)_random.NextDouble() * 16 - 8), new Vector2((float)_random.NextDouble() * 70 - 35, (float)_random.NextDouble() * (-15) - 40), new Vector2(0, 100), (float)_random.NextDouble() * 6, (float)_random.NextDouble() * 6, 0.5f, PInfo);
                map.Particles.AddParticle(shard3, 2.5f, new Vector2(position.X + (float)_random.NextDouble() * 16 - 8, position.Y + (float)_random.NextDouble() * 16 - 8), new Vector2((float)_random.NextDouble() * 70 - 35, (float)_random.NextDouble() * (-15) - 40), new Vector2(0, 100), (float)_random.NextDouble() * 6, (float)_random.NextDouble() * 6, 0.5f, PInfo);
                map.Particles.AddParticle(shard4, 2.5f, new Vector2(position.X + (float)_random.NextDouble() * 16 - 8, position.Y + (float)_random.NextDouble() * 16 - 8), new Vector2((float)_random.NextDouble() * 70 - 35, (float)_random.NextDouble() * (-15) - 40), new Vector2(0, 100), (float)_random.NextDouble() * 6, (float)_random.NextDouble() * 6, 0.5f, PInfo);
            }

        }

        public override int EffectID
        {
            get
            {

                return 2;
            }
        }
    }
}
