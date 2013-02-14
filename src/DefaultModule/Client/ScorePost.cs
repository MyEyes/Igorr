using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    class ScorePost : EventObject
    {
        public enum ScoreColor{
            Red=2,
            Blue=1,
            Neutral=0
        }

        ScoreColor color;
        float radius = 80;
        bool lightInc = false;

        public ScorePost(IMap map, Texture2D texture, Rectangle position, int id)
            : base(map,texture, position, id)
        {
        }

        public void SetColor(ScoreColor color)
        {
            this.color = color;
        }

        public override void GetInfo(string info)
        {
            switch (info)
            {
                case "n": SetColor(ScoreColor.Neutral); break;
                case "r": SetColor(ScoreColor.Red); break;
                case "b": SetColor(ScoreColor.Blue); break;
            }
        }

        public override void Update(float ms)
        {
            if (lightInc)
            {
                radius += ms / 1000;
                if (radius > 90)
                    lightInc = false;
            }
            else
            {
                radius -= ms / 1000;
                if (radius < 70)
                    lightInc = true;
            }

            _map.SetGlow(_id, new Vector2(Rect.X + 12, Rect.Y + 10), color == ScoreColor.Neutral ? Color.Gray : color == ScoreColor.Blue ? Color.Blue : Color.Red, radius, false);
        }

        public override void Draw(SpriteBatch batch)
        {
            int frame = (int)color;
            batch.Draw(_texture, _rect, new Rectangle(24 * frame, 0, 24, 64), Color.White,0,Vector2.Zero, SpriteEffects.None, 0.43f);
        }
    }
}
