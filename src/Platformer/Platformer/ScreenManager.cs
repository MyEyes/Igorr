using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client
{
    class ScreenManager
    {
        List<IScreen> _screens;
        ContentManager _content;
        GraphicsDevice _device;

        public ScreenManager(ContentManager Content, GraphicsDevice device, Game1 gameRef)
        {
            _content = Content;
            _device = device;
            _screens = new List<IScreen>();
            Game = gameRef;
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
            _screens.Add(screen);
            screen.Initialize(_device, this);
        }

        public void RemoveScreen(IScreen screen)
        {
            screen.OnRemove();
            _screens.Remove(screen);
        }

        public void Draw(GameTime gameTime)
        {
            for (int x = 0; x < _screens.Count; x++)
            {
                _screens[x].Draw(gameTime);
            }
        }

        public Game1 Game
        {
            get;
            private set;
        }
    }
}
