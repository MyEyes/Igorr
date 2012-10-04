using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Game
{
    class ScreenManager
    {
        List<IScreen> _screens;
        ContentManager _content;
        GraphicsDevice _device;

        public ScreenManager(ContentManager Content, GraphicsDevice device)
        {
            _content = Content;
            _device = device;
            _screens = new List<IScreen>();
        }

        public void Update(GameTime gameTime)
        {
            for (int x = 0; x < _screens.Count; x++)
            {
                _screens[x].Update(gameTime);
            }
        }

        public void AddScreen(IScreen screen)
        {
            screen.Initialize(_device, this);
            _screens.Add(screen);
        }

        public void RemoveScreen(IScreen screen)
        {
            _screens.Remove(screen);
        }

        public void Draw(GameTime gameTime)
        {
            for (int x = 0; x < _screens.Count; x++)
            {
                _screens[x].Draw(gameTime);
            }
        }
    }
}
