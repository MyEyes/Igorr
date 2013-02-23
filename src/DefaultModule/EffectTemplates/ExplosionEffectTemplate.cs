using System;
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
    class ExplosionEffectTemplate : IGORR.Modules.EffectTemplate
    {
        Texture2D _smokeTex;
        Texture2D _fireTex;
        Texture2D _particleTex;

        public override void DoEffect(IGORR.Client.Logic.IMap map, Microsoft.Xna.Framework.Vector2 dir, Microsoft.Xna.Framework.Point position, string info)
        {
            if(_smokeTex==null)
                _smokeTex=ContentInterface.LoadTexture("dust");
            if(_fireTex==null)
                _fireTex=ContentInterface.LoadTexture("Boom");
            if(_particleTex==null)
                _particleTex=ContentInterface.LoadTexture("expOrb");

            Vector2 pos = new Vector2(position.X, position.Y);
            ParticleInfo pInfo = new ParticleInfo();
            pInfo.collides = true;

            map.SetGlow(-2, pos, Color.LightYellow, 300, true, 500);
            if(_smokeTex!=null)
            for (int x = 0; x < 60; x++)
                map.Particles.AddParticle(_smokeTex, 1.2f, pos + new Vector2((float)_random.NextDouble() * 40 - 20, (float)_random.NextDouble() * 40 - 20), new Vector2((float)_random.NextDouble() * 40 - 20, (float)-(15 + _random.NextDouble() * 10)), new Vector2(0, 20), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble() + 0.7f, pInfo);
            if (_fireTex != null)
            for (int x = 0; x < 12; x++)
                map.Particles.AddParticle(_fireTex, 0.7f, pos + new Vector2((float)_random.NextDouble() * 20 - 10, (float)_random.NextDouble() * 20 - 10), new Vector2((float)_random.NextDouble() * 40 - 20, (float)-(15 + _random.NextDouble() * 10)), new Vector2(0, 20), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble() * 0.5f + 0.7f, pInfo);
            if (_particleTex != null)
            for (int x = 0; x < 6; x++)
                map.Particles.AddParticle(_particleTex, 0.8f, pos, new Vector2((float)_random.NextDouble() * 600 - 300, (float)_random.NextDouble() * 600 - 300), new Vector2(0, 0), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble() * 0.2f + 0.4f, pInfo);
        }

        public override int EffectID
        {
            get
            {
                return 1;
            }
        }
    }
}
