using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.UI
{
    class UIScreen : UIElement, IScreen
    {
        SpriteBatch batch;
        protected ScreenManager _manager;

        public UIScreen()
            : base(null, Vector2.Zero)
        {

        }

        public virtual void Initialize(GraphicsDevice Device, ScreenManager manager)
        {
            _manager = manager;
            batch = new SpriteBatch(Device);
        }

        public void Remove()
        {
            _manager.RemoveScreen(this);
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            Update((float)gameTime.ElapsedGameTime.TotalMilliseconds, mouse);
            _lastMouse = mouse;
        }

        public void Draw(GameTime gameTime)
        {
            batch.Begin( SpriteSortMode.FrontToBack, null);
            Draw(batch);
            batch.End();
        }
    }
}
