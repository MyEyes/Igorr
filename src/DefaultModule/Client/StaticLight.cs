using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    class StaticLight :GameObject
    {
        IMap _map;

        public StaticLight(IMap map, Texture2D texture, Rectangle position, int id, Color c, int radius)
            : base(texture, position, id)
        {
            _map = map;
            _map.SetGlow(_id, this._position+new Vector2(8,8), c, radius, true);
        }

        public override void Update(float ms)
        {
           
        }


        public override void Draw(SpriteBatch batch)
        {

        }
    }
}
