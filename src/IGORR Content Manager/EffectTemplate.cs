using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Content
{
    public class EffectTemplate
    {
        public int NumParticles{get; protected set;}
        public Vector2 Offset { get; protected set; }
        public Vector2 SizeModRange { get; protected set; }
        public Vector2 SpeedRange { get; protected set; }
        public Vector2 LifeTimeRange { get; protected set; }
        public List<Vector2> AngleRanges { get; protected set; }
        public float InitialSpeedFactor { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public bool Collides { get; protected set; }
        public bool Sticky { get; protected set; }
        protected float[] _normalizedAngleRangeLength;
        protected float _totalAngleRangeSum=0;

        public EffectTemplate(string file)
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(file));
            NumParticles = reader.ReadInt32();
            Offset = new Vector2(reader.ReadSingle(),reader.ReadSingle());
            SizeModRange = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            SpeedRange = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            LifeTimeRange = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            int numRanges = reader.ReadInt32();
            AngleRanges = new List<Vector2>();
            _normalizedAngleRangeLength = new float[numRanges];
            for (int x = 0; x < numRanges; x++)
            {
                Vector2 newRange = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                _totalAngleRangeSum += (float)(((newRange.Y - newRange.X) + Math.PI * 2) % Math.PI * 2);
                _normalizedAngleRangeLength[x] = (float)(((newRange.Y - newRange.X) + Math.PI * 2) % Math.PI * 2);
                AngleRanges.Add(newRange);
            }
            for (int x = 0; x < numRanges; x++)
            {
                _normalizedAngleRangeLength[x] /= _totalAngleRangeSum;
            }
            _normalizedAngleRangeLength = new float[AngleRanges.Count];
            InitialSpeedFactor = reader.ReadSingle();
            string textureName = reader.ReadString();
            Collides = reader.ReadBoolean();
            Sticky = reader.ReadBoolean();
        }

        public bool isInAngles(float angle)
        {
            for (int x = 0; x < AngleRanges.Count; x++)
            {
                if ((AngleRanges[x].X < angle|| AngleRanges[x].X>AngleRanges[x].Y) && AngleRanges[x].Y>angle )
                    return true;
            }
            return false;
        }

        public float GetAngle(double random)
        {
            for (int x = 0; x < AngleRanges.Count; x++)
            {
                if (random < _normalizedAngleRangeLength[x])
                    return (float)(AngleRanges[x].X + random * _totalAngleRangeSum);
                random -= _normalizedAngleRangeLength[x];
            }
            return 0;
        }
    }
}
