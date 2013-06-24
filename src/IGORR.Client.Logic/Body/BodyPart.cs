using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic.Body
{
    public class BodyPart:ICollectible
    {
        public Texture2D _texture;
        AnimationController _animations;

        bool _behind;

        public BodyPart(Texture2D texture, BodyPartType type)
        {
            _texture = texture;
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

        public override bool Equals(object obj)
        {
            BodyPart part = obj as BodyPart;
            if (part.GetID() == GetID())
                return true;
            return base.Equals(obj);
        }
    }
}
