using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR_Server.Logic
{
    public class BodyPart
    {
        public float speedBonus = 0f;
        public float jumpBonus = 0f;
        public int attackID = -1;

        public int airJumpMax = 0;
        public int airJumpCount = 0;
        public float airJumpStrength = 0f;

        public int AttBonus = 0;
        public int DefBonus = 0;

        public int maxHPBonus=0;

        public BodyPart()
        {

        }

        public void Clear()
        {
            speedBonus = 0;
            jumpBonus = 0;
            airJumpMax = 0;
            airJumpCount = 0;
            airJumpStrength = 0;
            maxHPBonus = 0;
            AttBonus = 0;
            DefBonus = 0;
        }

        public void Add(BodyPart part)
        {
            this.speedBonus += part.speedBonus;
            this.jumpBonus += part.jumpBonus;
            this.airJumpMax += part.airJumpMax;
            this.airJumpStrength += part.airJumpStrength;
            this.maxHPBonus += part.maxHPBonus;
            this.AttBonus += part.AttBonus;
            this.DefBonus += part.DefBonus;
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
    }
}
