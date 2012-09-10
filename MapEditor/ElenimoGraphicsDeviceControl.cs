using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if WINDOWS
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{
	// System.Drawing and the XNA Framework both define Color and Rectangle
	// types. To avoid conflicts, we specify exactly which ones to use.
	using Color = System.Drawing.Color;
	using Rectangle = Microsoft.Xna.Framework.Rectangle;


	/// <summary>
	/// Custom control uses the XNA Framework GraphicsDevice to render onto
	/// a Windows Form. Derived classes can override the Initialize and Draw
	/// methods to add their own drawing code.
	/// </summary>
	public abstract class ElenimoGraphicsDeviceControl
		: Control
	{
		public GraphicsDevice GraphicsDevice
		{
			get { return graphicsDeviceService.GraphicsDevice; }
		}

		public ServiceContainer Services
		{
			get { return services; }
		}

		protected abstract void Initialize();
		protected abstract void Draw();

		protected override void OnCreateControl()
		{
			// Don't initialize the graphics device if we are running in the designer.
			if (!DesignMode)
			{
				graphicsDeviceService = new ElenimoGraphicsDeviceService (Handle, ClientSize.Width, ClientSize.Height);

				// Register the service, so components like ContentManager can find it.
				services.AddService<IGraphicsDeviceService> (graphicsDeviceService);

				// Give derived classes a chance to initialize themselves.
				Initialize ();
			}

			base.OnCreateControl ();
		}

		protected override void Dispose (bool disposing)
		{
			if (graphicsDeviceService != null)
			{
				graphicsDeviceService.Release (disposing);
				graphicsDeviceService = null;
			}

			base.Dispose (disposing);
		}

		protected virtual void PaintUsingSystemDrawing (System.Drawing.Graphics graphics, string text)
		{
			graphics.Clear (Color.CornflowerBlue);

			using (Brush brush = new SolidBrush (Color.Black))
			{
				using (StringFormat format = new StringFormat ())
				{
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Center;

					graphics.DrawString (text, Font, brush, ClientRectangle, format);
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			string beginDrawError;
			if (BeginDraw (out beginDrawError))
			{
				// Draw the control using the GraphicsDevice.
				Draw ();
				EndDraw ();
			}
			else
			{
				// If BeginDraw failed, show an error message using System.Drawing.
				PaintUsingSystemDrawing (e.Graphics, beginDrawError);
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		private ServiceContainer services = new ServiceContainer();
		private ElenimoGraphicsDeviceService graphicsDeviceService;

		private bool BeginDraw (out string error)
		{
			// If we have no graphics device, we must be running in the designer.
			if (graphicsDeviceService == null)
			{
				error = string.Format ("{0}{1}{1}{2}", Text, Environment.NewLine, GetType ());
				return false;
			}

			// Make sure the graphics device is big enough, and is not lost.
			string deviceResetError;
			if (!HandleDeviceReset (out deviceResetError))
			{
				error = deviceResetError;
				return false;
			}

			Viewport viewport = new Viewport
			{
				X = 0,
				Y = 0,
				Width = ClientSize.Width,
				Height = ClientSize.Height,
				MinDepth = 0,
				MaxDepth = 1
			};

			GraphicsDevice.Viewport = viewport;

			error = string.Empty;
			return true;
		}

		private void EndDraw()
		{
			try
			{
				Rectangle sourceRectangle = new Rectangle (0, 0, ClientSize.Width, ClientSize.Height);

				GraphicsDevice.Present (sourceRectangle, null, this.Handle);
			}
			catch
			{
				// Present might throw if the device became lost while we were
				// drawing. The lost device will be handled by the next BeginDraw,
				// so we just swallow the exception.
			}
		}

		private bool HandleDeviceReset (out string error)
		{
			bool deviceNeedsReset = false;

			switch (GraphicsDevice.GraphicsDeviceStatus)
			{
				case GraphicsDeviceStatus.Lost:
					// If the graphics device is lost, we cannot use it at all.
					error = "Graphics device lost";
					return false;

				case GraphicsDeviceStatus.NotReset:
					// If device is in the not-reset state, we should try to reset it.
					deviceNeedsReset = true;
					break;

				default:
					// If the device state is ok, check whether it is big enough.
					PresentationParameters pp = GraphicsDevice.PresentationParameters;

					deviceNeedsReset = (ClientSize.Width != pp.BackBufferWidth) ||
									   (ClientSize.Height != pp.BackBufferHeight);
					break;
			}

			// Do we need to reset the device?
			if (deviceNeedsReset)
			{
				try
				{
					graphicsDeviceService.ResetDevice (ClientSize.Width, ClientSize.Height);
				}
				catch (Exception e)
				{
					error = string.Format ("Graphics device reset failed{1}{1}{0}", e, Environment.NewLine);
					return false;
				}
			}

			error = string.Empty;
			return true;
		}
	}
}
#endif