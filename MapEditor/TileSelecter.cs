using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor
{
    class TileSelecter:GraphicsDeviceControl
    {
        ContentManager content;
        SpriteBatch batch;
        Texture2D tileSet;
        const int tileSize = 16;
        int selected = -1;
        int offset=0;
        float scale = 1;
        HScrollBar scrollbar;
        MouseState _prevMouse;

        protected override void Initialize()
        
        {
            content=new ContentManager(Services, "Content");
            batch = new SpriteBatch(GraphicsDevice);
        }

        public void SetScrollbar(HScrollBar bar)
        {
            scrollbar = bar;
            if (tileSet != null)
            {
                if (tileSet.Width >= GraphicsDevice.PresentationParameters.BackBufferWidth)
                {
                    scrollbar.Maximum = tileSet.Width - GraphicsDevice.PresentationParameters.BackBufferWidth;
                    scrollbar.Enabled = true;
                }
                else
                {
                    scrollbar.Maximum = 0;
                    scrollbar.Enabled = false;
                }
            }
        }

        public void LoadTileSet(string name)
        {
            try
            {
                this.tileSet = content.Load<Texture2D>(name);
                scale = this.Bounds.Height / tileSize;

                if (scrollbar!=null && tileSet.Width*scale >= GraphicsDevice.PresentationParameters.BackBufferWidth)
                {
                    scrollbar.Maximum = (int)(tileSet.Width*scale) - GraphicsDevice.PresentationParameters.BackBufferWidth;
                    scrollbar.SmallChange = 16;
                    scrollbar.LargeChange = 60;
                    scrollbar.Enabled = true;
                }
                else
                {
                    scrollbar.Maximum = 0;
                    scrollbar.Enabled = false;
                }
            }
            catch (ContentLoadException e)
            {
                System.Windows.Forms.MessageBox.Show("Could not find asset: " + tileSet);
            }
        }

        protected void UpdateLogic()
        {
            MouseState mouse = Mouse.GetState();
            Vector2 correction = -new Vector2(Parent.PointToScreen(this.Location).X, Parent.PointToScreen(this.Location).Y);
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y) + correction;
            if (mousePos.X < 0 || mousePos.Y < 0 || mousePos.X > this.Width || mousePos.Y > this.Height)
            {
                return;
            }
            if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                selected = (int)(((mouse.X + correction.X) + offset) / (tileSize * scale));
                if (selected < 0 || selected * tileSize >= tileSet.Width || mouse.Y+correction.Y>16*scale)
                    selected = -1;
            }
            else if (mouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _prevMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                offset -= (int)((mouse.X - _prevMouse.X));
                offset = offset >= 0 ? offset : 0;
                offset = offset < scrollbar.Maximum ? offset : scrollbar.Maximum;
                scrollbar.Value = offset;
            }
            _prevMouse = mouse;
        }

        protected override void Draw()
        {
            if (scrollbar != null)
            {
                offset = scrollbar.Value;
                #region scaling
                scale = this.Bounds.Height / (float)tileSize;

                if (scrollbar != null && tileSet.Width * scale >= GraphicsDevice.PresentationParameters.BackBufferWidth)
                {
                    scrollbar.Maximum = (int)((tileSet.Width+32)*scale) - GraphicsDevice.PresentationParameters.BackBufferWidth;
                    scrollbar.SmallChange = (int)(16*scale);
                    scrollbar.LargeChange = (int)(60*scale);
                    scrollbar.Enabled = true;
                }
                else
                {
                    scrollbar.Maximum = 0;
                    scrollbar.Enabled = false;
                }
                #endregion
            }
            GraphicsDevice.Clear(Color.Black);
            if (tileSet != null)
            {
                if (this.Focused)
                    UpdateLogic();
                batch.Begin();
                batch.Draw(tileSet, new Vector2(-offset, 0), null, Color.Gray, 0, Vector2.Zero, scale, SpriteEffects.None, 0.5f);
                if (selected >= 0) batch.Draw(tileSet, new Rectangle((int)(-offset + selected * tileSize*scale), 0, (int)(16*scale), (int)(16*scale)), new Rectangle(selected * tileSize, 0, 16, 16), Color.White);
                batch.End();
            }
            Application.Idle += delegate { Invalidate(); };
        }

        public int SelectedTile
        {
            get { return selected; }
        }

    }
}
