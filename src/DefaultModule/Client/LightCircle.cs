using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    class LightCircle :EventObject
    {
        float frame=0;
        IMap _map;

        public LightCircle(IMap map, Texture2D texture, Rectangle position, int id)
            : base(map,texture, position, id)
        {
            _map = map;
        }

        public override void Update(float ms)
        {
            frame += 0.5f * ms / 1000f;
            _map.SetGlow(_id, this._position + new Vector2((float)Math.Cos(frame), (float)Math.Sin(frame)) * 48, Color.Yellow, 120, true);
        }


        public override void Draw(SpriteBatch batch)
        {
        }
    }
}
