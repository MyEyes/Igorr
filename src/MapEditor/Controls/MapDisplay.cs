using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Windows.Forms;
using IGORR.Content;

namespace MapEditor
{
    class MapDisplay : GraphicsDeviceControl
    {
        Map map;
        ContentManager content;
        SpriteBatch batch;
        Camera cam;
        MouseState _prevMouse;
        SamplerState _drawState;
        TileSelecter selecter;
        int layer = -1;
		float minZoom = 2f;
		float maxZoom = 0.02f;

        protected override void Initialize()
        {
            content=new PackedContentManager(Services, "Content.7z", Nomad.Archive.SevenZip.KnownSevenZipFormat.SevenZip);
            batch = new SpriteBatch(GraphicsDevice);
            cam = new Camera(new Vector2(0, 0), new Rectangle(this.Bounds.X,this.Bounds.Y,this.Bounds.Width,this.Bounds.Height));
            //map = new Map(content, 100, 100);
            Application.Idle += delegate { Invalidate(); };
            _drawState = new SamplerState();
            _drawState.Filter = TextureFilter.Point;
        }

        public void SetTileSelecter(TileSelecter ts)
        {
            selecter = ts;

            if (map != null)
                selecter.LoadTileSet(map.TileSet);
        }

        public void SetLayer(int layer)
        {
            this.layer = layer;
            map.SetActiveLayer(layer);
        }

        protected void UpdateInput()
        {
			Mouse.WindowHandle = this.Handle;
            MouseState mouse = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y);

            if (mousePos.X < 0 || mousePos.Y < 0 || mousePos.X > this.Width || mousePos.Y > this.Height)
            {
                return;
            }
            if (mouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _prevMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                cam.Move(1/cam.ZoomFactor*new Vector2(mouse.X - _prevMouse.X, mouse.Y - _prevMouse.Y)*-1);
            }
            if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Vector2 worldPos = cam.ScreenToWorld(new Vector2(mouse.X,mouse.Y));
                if (selecter != null && layer >= 0 && layer < 3)
                    map.ChangeTile(layer, worldPos.X, worldPos.Y, selecter.SelectedTile);
                else if(selecter!=null && layer ==3)
                {
                    map.SetObject(worldPos.X, worldPos.Y);
                }
            }
			if (mouse.ScrollWheelValue != _prevMouse.ScrollWheelValue)
			{
				float scrollValue = ((mouse.ScrollWheelValue - _prevMouse.ScrollWheelValue) / 120) * 0.1f;
				cam.ZoomFactor = MathHelper.Clamp (cam.ZoomFactor + scrollValue, maxZoom, minZoom);
			}

            _prevMouse = mouse;
        }

        protected override void Draw()
        {
            if (map == null)
            {
                GraphicsDevice.Clear(Color.Black);
                return;
            }
            if (this.Focused)
            {
                UpdateInput();
            }
            cam.SetDim(new Rectangle(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height));
            GraphicsDevice.Clear(Color.Black);
            batch.Begin(SpriteSortMode.FrontToBack, null, _drawState, null, null, null, cam.ViewMatrix);
            map.Draw(cam, batch);
            batch.End();
        }

        public void SetMap(Map map)
        {
            this.map = map;
            cam.SetPos(Vector2.Zero);   
            if (selecter != null)
                selecter.LoadTileSet(map.TileSet);
        }

        public void Save()
        {
            map.Save();
        }

        public ContentManager Content
        {
            get { return content; }
        }
    }
}
