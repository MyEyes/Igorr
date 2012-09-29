using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace MapEditor
{
    public class TileSelecter
		: GraphicsDeviceControl
    {
        ContentManager content;
        SpriteBatch batch;
        Texture2D tileSet;
		Texture2D tileHighlight;
        const int tileSize = 16;
		const int targetSize = 64;
        int selected = -1;
        int offset = 0;
		int tilesPerRow = 0;
		int tileCount = 0;
        ScrollBar scrollbar;
        MouseState prevMouse;
		float scale;

		public int SelectedTile
        {
            get { return selected; }
        }

        protected override void Initialize()
        {
            content = new ContentManager(Services, "Content");
            batch = new SpriteBatch(GraphicsDevice);

			Application.Idle += (o, e) => Invalidate();
			this.Resize += (o, e) => invalidateScrollbar();
        }

        public void SetScrollbar(ScrollBar bar)
        {
            scrollbar = bar;
			scrollbar.ValueChanged += (o, e) => invalidateScrollbar();
			scrollbar.Scroll += (o, e) => invalidateScrollbar();

            if (tileSet != null)
				invalidateScrollbar();
        }

        public void LoadTileSet(string name)
        {
            try
            {
                tileSet = content.Load<Texture2D> (name);
				tileHighlight = content.Load<Texture2D> ("tileHighlight");
				tileCount = tileSet.Width / tileSize;

				invalidateScrollbar();
            }
            catch (ContentLoadException)
            {
                MessageBox.Show ("Could not find asset: " + name);
            }
        }

        protected void UpdateLogic()
        {
			// Deal with the scroll bar
			if (scrollbar != null)
            {
                offset = scrollbar.Value;
				invalidateScrollbar();
            }

			if (Focused)
			{
				Mouse.WindowHandle = this.Handle;
				MouseState mouse = Mouse.GetState();

				bool mouseOffScreen = mouse.X < 0 || mouse.Y < 0 || mouse.X > Width || mouse.Y > Height;
            
				if (mouseOffScreen)
					return;

				if (mouse.LeftButton == ButtonState.Pressed)
				{
					// Tiles per pervious rows + tiles in current row
					selected = ((mouse.Y / targetSize) * tilesPerRow) + (mouse.X / targetSize) - 1;
            
					// Don't let the user select an invalid tile
					if (selected < 0 || selected * tileSize >= tileSet.Width)
						selected = -1;
				}

				prevMouse = mouse;
			}
        }

        protected override void Draw()
        {
			GraphicsDevice.Clear (Color.Black);

            if (tileSet != null)
            {
                UpdateLogic();

                batch.Begin (SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullCounterClockwise);
                
				int row = 0;
				int index = 1;

				for (int i=1; i < tileCount; i++)
				{
					Rectangle sourceRect = new Rectangle ((i-1) * tileSize, 0, tileSize, tileSize);
					Rectangle destRect = new Rectangle (index * targetSize,
						(row * targetSize) - offset, targetSize, targetSize);
					
					batch.Draw (tileSet, destRect, sourceRect, Color.White);

					if (selected == i-1)
						batch.Draw (tileHighlight, destRect, Color.White);

					index++;

					if (index >= tilesPerRow)
					{
						index = 0;
						row++;
					}
				}

				// Draw the tile highlight for the eraser tile
				if (selected == -1 && tileHighlight!=null)
					batch.Draw (tileHighlight, new Rectangle(0, 0, targetSize, targetSize), Color.White);
                
				batch.End();
            }
        }

		protected void invalidateScrollbar ()
		{
			tilesPerRow =  this.Bounds.Width / targetSize;
			int rowCount = tileCount / tilesPerRow;
			bool needScrollbar = rowCount * targetSize > Bounds.Height;

            if (scrollbar != null && needScrollbar)
            {
                scrollbar.Maximum = (int)(rowCount * targetSize) - Bounds.Height;
                scrollbar.SmallChange = (int)(targetSize);
                scrollbar.LargeChange = (int)(targetSize * 2);
                scrollbar.Enabled = true;
            }
            else
            {
                scrollbar.Maximum = 0;
                scrollbar.Enabled = false;
            }
		}
    }
}