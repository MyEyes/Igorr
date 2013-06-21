using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    public class Camera
    {
        Vector2 _position;
        Rectangle _screenSize;
        Vector2 _dim;
        float zoom=2f;
        bool _jump;
        Matrix projection=Matrix.Identity;
        public static Camera CurrentCam = null;

        public Camera(Vector2 position, Rectangle ScreenSize)
        {
            _position = position;
            _screenSize = ScreenSize;
            _dim = new Vector2(ScreenSize.Width, ScreenSize.Height);
        }

        public void Move(Vector2 diff)
        {
            _position += diff;
        }

        public void JumpNext()
        {
            _jump = true;
        }

        public void MoveTo(Vector2 pos, float factor)
        {
            if (!_jump)
                _position += (pos - _position) * factor;
            else
            {
                _position = pos;
                _jump = false;
            }
        }

        public void SetPos(Vector2 position)
        {
            _position = position;
        }

        public void Zoom(float change)
        {
            zoom += change;
        }

        public void ZoomInOn(float factor, Vector2 position)
        {
            _position -= (1 - factor) * (position - _dim / 2) * (1/zoom);
            zoom *= factor;
        }

        public Vector2 Position
        {
            get { return _position; }
        }

        public Matrix ViewMatrix
        {
            get {
                Vector2 targetPos = new Vector2(_screenSize.Width, _screenSize.Height) / (2 * zoom) - new Vector2((int)_position.X, (int)_position.Y);
                targetPos.X = ((int)(zoom * targetPos.X)) / zoom;
                targetPos.Y = ((int)(zoom * targetPos.Y)) / zoom;
                return Matrix.CreateTranslation(new Vector3(targetPos, 0)) * Matrix.CreateScale(zoom, zoom, 1);
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                if (projection == Matrix.Identity)
                    projection = Matrix.CreateTranslation(-_screenSize.Width / 2 - 0.5f, -_screenSize.Height / 2 - 0.5f, 0) * Matrix.CreateScale(2 / ((float)_screenSize.Width), -2 / ((float)_screenSize.Height), 1);
                return projection;
            }
            set
            {
                projection = value;
            }
        }

        public Vector2 Size
        {
            get
            {
                return _dim / zoom;
            }
        }

        public Vector2 ViewToWorldPosition(Vector2 pos)
        {
            Matrix inversion = Matrix.Invert(ViewMatrix);
            pos=Vector2.Transform(pos, inversion);
            return pos;
        }

        public Rectangle ViewSpace
        {
            get { return new Rectangle((int)(_position.X - _screenSize.Width / (2 * zoom))-1, (int)(_position.Y - _screenSize.Height / (2 * zoom))-1, (int)(_screenSize.Width / zoom) + 1, (int)(_screenSize.Height / zoom) + 1); }
        }
    }
}
