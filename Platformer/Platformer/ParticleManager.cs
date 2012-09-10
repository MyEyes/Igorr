﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Platformer
{
    public struct ParticleInfo
    {
        public bool collides;
        public bool sticky;
    }

    public class ParticleManager
    {
        const int maxParticles = 4000;
        Particle[] _particles;
        int startIndex;
        int numParticles;
        Random _random;
        Texture2D _dustTex;
        Texture2D _boomTex;
        Texture2D _splatTex;
        Texture2D _bloodTex;

        public ParticleManager(ContentManager content)
        {
            _random = new Random();
            _particles = new Particle[maxParticles];
            for (int x = 0; x < _particles.Length; x++)
            {
                _particles[x] = new Particle();
            }
            startIndex = 0;
            numParticles = 0;
            _dustTex = content.Load<Texture2D>("dust");
            _boomTex = content.Load<Texture2D>("Boom");
            _splatTex = content.Load<Texture2D>("splat");
            _bloodTex = content.Load<Texture2D>("blood");
        }

        public void Update(Map map, float secs)
        {
            startIndex %= maxParticles;
            for (int x = 0; x < numParticles; x++)
            {
                int num = startIndex + x;
                num %= maxParticles;
                _particles[num].Update(map, secs);
                if(num==startIndex && !_particles[num].Alive)
                {
                    startIndex++;
                    numParticles--;
                    x--;
                }
            }
        }

        public void LandParticles(Vector2 positionLeft, Vector2 positionRight)
        {
            ParticleInfo info = new ParticleInfo();
            for (int x = 0; x < 8; x++)
                AddParticle(_dustTex, 0.25f, positionLeft, new Vector2((float)_random.NextDouble() * 15 - 40, (float)-(15 + _random.NextDouble() * 10)), new Vector2(0, 20), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble() * 0.5f + 0.2f, info);

            for (int x = 0; x < 8; x++)
                AddParticle(_dustTex, 0.25f, positionRight, new Vector2((float)-_random.NextDouble() * 15 + 40, (float)-(15 + _random.NextDouble() * 10)), new Vector2(0, 20), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble() * 0.5f + 0.2f, info);
        }

        public void Boom(LightMap light, Vector2 pos)
        {
            ParticleInfo info = new ParticleInfo();
            light.SetGlow(-2, pos, 300, true, 500);
            for (int x = 0; x < 80; x++)
                AddParticle(_dustTex, 1.2f, pos+new Vector2((float)_random.NextDouble()*40-20,(float)_random.NextDouble()*40-20), new Vector2((float)_random.NextDouble() * 40 - 20, (float)-(15 + _random.NextDouble() * 10)), new Vector2(0, 20), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble() + 0.7f, info);

            for (int x = 0; x < 6; x++)
                AddParticle(_boomTex, 0.5f, pos + new Vector2((float)_random.NextDouble() * 20 - 10, (float)_random.NextDouble() * 20 - 10), new Vector2((float)_random.NextDouble() * 40 - 20, (float)-(15 + _random.NextDouble() * 10)), new Vector2(0, 20), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble() * 0.4f + 0.7f, info);
        }

        public void Splat(Vector2 position, Vector2 dir)
        {
            ParticleInfo info = new ParticleInfo();
            info.collides = true;
            for (int x = 0; x < 18; x++)
                AddParticle(_splatTex, 4f, position, dir + new Vector2((float)_random.NextDouble() * 40 - 20, (float)-(25 + _random.NextDouble() * 20)), new Vector2(0, 60), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble(), info);
            info.sticky = true;
            for (int x = 0; x < 50; x++)
                AddParticle(_bloodTex, 6f, position, dir + new Vector2((float)_random.NextDouble() * 40 - 20, (float)-(25 + _random.NextDouble() * 20)), new Vector2(0, 60), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble(), info);
        }

        public void Run(Vector2 position, Vector2 direction)
        {
            ParticleInfo info = new ParticleInfo();
            for (int x = 0; x < 8; x++)
                AddParticle(_dustTex, 0.25f, position, direction*((float)_random.NextDouble()/2+1)+Vector2.UnitY*((float)_random.NextDouble()*10-5), new Vector2(0, 60), (float)(_random.NextDouble() * Math.PI * 2), (float)(_random.NextDouble() * Math.PI * 2), (float)_random.NextDouble() * 0.5f + 0.2f,info);
        }

        public void AddParticle(Texture2D tex, float lifeTime, Vector2 Position, Vector2 Speed, Vector2 Acceleration, float Rotation, float RotationSpeed, float SizeMod, ParticleInfo info)
        {
            if (numParticles >= maxParticles)
                return;
            _particles[(startIndex + numParticles) % maxParticles].Create(tex, lifeTime, Position, Speed, Acceleration, Rotation, RotationSpeed, SizeMod, info);
            numParticles++;
        }

        public void Draw(SpriteBatch batch)
        {
            startIndex %= maxParticles;
            for (int x = 0; x < numParticles; x++)
            {
                int num = startIndex + x;
                num %= maxParticles;
                _particles[num].Draw(batch);
            }
        }
    }
}
