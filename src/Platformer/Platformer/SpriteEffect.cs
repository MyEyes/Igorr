using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client
{
    class SpriteEffect : GameObject
    {
        Animation _animation;

        public SpriteEffect(Animation ani, Texture2D tex, Rectangle spawnRect, int id)
            : base(tex, spawnRect, id)
        {
            _animation = ani;
        }

        public override void Draw(SpriteBatch batch)
        {
            _animation.Update(16);
            batch.Draw(_texture, _rect, _animation.GetFrame(), Color.White);
            if (!_animation.Finished)
                base.Draw(batch);
        }

        public bool Remove
        {
            get { return _animation.Finished; }
        }
    }
}
