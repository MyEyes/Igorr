using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Server.Logic.Body
{
    public class BodyPart: ICollectible
    {
        
        public BodyPart(BodyPartType type)
        {
            Type = type;
            MaxStacks = 1;
        }

        public virtual void Update(Player player, float ms)
        {

        }

        public virtual string GetName()
        {
            return "";
        }

        public virtual int GetID()
        {
            return -1;
        }

        public virtual float GetSpawnTime()
        {
            return 10000;
        }

        public override bool Equals(object obj)
        {
            BodyPart part = obj as BodyPart;
            if (part != null)
                return part.GetID() == GetID();
            return base.Equals(obj);
        }

        public BodyPartType Type
        {
            get;
            private set;
        }

        public int MaxStacks
        {
            get;
            protected set;
        }
    }
}
