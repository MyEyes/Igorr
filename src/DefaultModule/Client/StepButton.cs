﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    class StepButton : EventObject
    {
        int frame=0;
        public StepButton(IMap map, Texture2D texture, Rectangle position, int id)
            : base(map, texture, position, id)
        {

        }

        public override void Update(float ms)
        {
        }

        public override void Event(Player obj)
        {
        }

        public override void GetInfo(string info)
        {
            frame = (int)info[0];
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(_texture, _rect, new Rectangle(16 * frame, 0, 16, 16), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
        }
    }
}
