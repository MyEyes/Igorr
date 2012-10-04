using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Game
{
    interface IScreen
    {
        void Initialize(GraphicsDevice Device, ScreenManager manager);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
