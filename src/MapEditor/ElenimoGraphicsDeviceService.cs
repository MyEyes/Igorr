using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{
	public class ElenimoGraphicsDeviceService
		: IGraphicsDeviceService
    {
		public ElenimoGraphicsDeviceService (IntPtr windowHandle, int width, int height)
        {
            parameters = new PresentationParameters();

            parameters.BackBufferWidth = Math.Max(width, 1);
            parameters.BackBufferHeight = Math.Max(height, 1);
            parameters.BackBufferFormat = SurfaceFormat.Color;
            parameters.DepthStencilFormat = DepthFormat.Depth24;
            parameters.PresentationInterval = PresentInterval.Immediate;
			parameters.DeviceWindowHandle = windowHandle;
            parameters.IsFullScreen = false;

            graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter,
				GraphicsProfile.HiDef, parameters);
        }

		// IGraphicsDeviceService events
		public event EventHandler<EventArgs> DeviceCreated;
		public event EventHandler<EventArgs> DeviceDisposing;
		public event EventHandler<EventArgs> DeviceReset;
		public event EventHandler<EventArgs> DeviceResetting;

        public void Release (bool disposing)
        {
            if (disposing)
            {
                if (DeviceDisposing != null)
                    DeviceDisposing(this, EventArgs.Empty);

                graphicsDevice.Dispose();
            }

            graphicsDevice = null;
        }

        public void ResetDevice(int width, int height)
        {
            if (DeviceResetting != null)
                DeviceResetting(this, EventArgs.Empty);

            parameters.BackBufferWidth = Math.Max(parameters.BackBufferWidth, width);
            parameters.BackBufferHeight = Math.Max(parameters.BackBufferHeight, height);
            parameters.BackBufferWidth = width;
            parameters.BackBufferHeight = height;

            graphicsDevice.Reset(parameters);

            if (DeviceReset != null)
                DeviceReset(this, EventArgs.Empty);
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        private GraphicsDevice graphicsDevice;
        private PresentationParameters parameters;
    }
}
