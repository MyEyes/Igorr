using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    public class BodyPart
    {
        public float speedBonus = 0f;
        public float jumpBonus = 0f;
        public Texture2D _texture;
        AnimationController _animations;

        public int airJumpMax = 0;
        public int airJumpCount = 0;
        public float airJumpStrength = 0f;

        bool _behind;

        public BodyPart(Texture2D texture)
        {
            _texture = texture;
        }

        public void Clear()
        {
            speedBonus = 0;
            jumpBonus = 0;
            airJumpMax = 0;
            airJumpStrength = 0;
        }

        public void Add(BodyPart part)
        {
            this.speedBonus += part.speedBonus;
            this.jumpBonus += part.jumpBonus;
            this.airJumpMax += part.airJumpMax;
            this.airJumpStrength += part.airJumpStrength;
        }

        public virtual string GetName()
        {
            return "";
        }

        public virtual int GetID()
        {
            return 0;
        }

        public virtual float GetSpawnTime()
        {
            return 10000;
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }
    }
}
