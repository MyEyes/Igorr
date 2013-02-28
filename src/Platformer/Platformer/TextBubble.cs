using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Client.Logic;

namespace IGORR.Client
{
    class TextBubble
    {
        Vector2 _origin;
        Vector2 _textStart;
        Vector2 _origTextStart;
        GameObject _speaker;
        string _text;
        Vector2 _textSize;
        static SpriteFont _font;
        VertexPositionColor[] _vertices;
        static BasicEffect _effect;
        float _timeOut = 0;
        bool inSight = true;
        public bool alive = true;

        public TextBubble(string Text, Vector2 origin, float timeout)
        {
            _text = Text;
            _origin = origin;
            if (_font == null)
                _font = IGORR.Content.ContentInterface.LoadFont("textFont");
            _textSize = _font.MeasureString(_text);
            CalculateTextStart();
            CalculateVertices();
            _timeOut = timeout;
        }

        public TextBubble(string Text, GameObject speaker, float timeout)
        {
            _text = Text;
            _origin = speaker.MidPosition - Vector2.UnitY * speaker.Rect.Height * 0.5f;
            _speaker = speaker;
            if (_font == null)
                _font = IGORR.Content.ContentInterface.LoadFont("textFont");
            _textSize = _font.MeasureString(_text);
            CalculateTextStart();
            CalculateVertices();
            _timeOut = timeout;
        }

        private void CalculateVertices()
        {
            //First 3 vertices are from Speaker to box
            //Next 2*3 are rectangular center
            //Next 8*3 are top,left,right,bottom
            //Next 16*3 are rounded corners
            float depth = 0.12f;

            if (_vertices == null)
                _vertices = new VertexPositionColor[81];
            //Line to center of text box
            _vertices[0] = new VertexPositionColor(new Vector3(_origin,depth),Color.White);
            Vector2 dir = _textStart + _textSize * 0.25f - _origin;
            dir.Normalize();
            dir = new Vector2(-dir.Y, dir.X)*5;
            _vertices[1] = new VertexPositionColor(new Vector3(_textStart + _textSize * 0.25f + dir, depth), Color.White);
            _vertices[2] = new VertexPositionColor(new Vector3(_textStart + _textSize * 0.25f - dir,depth), Color.White);
            //Center of text box
            _vertices[3] = new VertexPositionColor(new Vector3(_textStart, depth), Color.White);
            _vertices[4] = new VertexPositionColor(new Vector3(_textStart + new Vector2(_textSize.X, 0) * 0.5f, depth), Color.White);
            _vertices[5] = new VertexPositionColor(new Vector3(_textStart + new Vector2(_textSize.X, _textSize.Y) * 0.5f, depth), Color.White);
            _vertices[6] = new VertexPositionColor(new Vector3(_textStart, depth), Color.White);
            _vertices[7] = new VertexPositionColor(new Vector3(_textStart + new Vector2(_textSize.X, _textSize.Y) * 0.5f, depth), Color.White);
            _vertices[8] = new VertexPositionColor(new Vector3(_textStart + new Vector2(0, _textSize.Y) * 0.5f, depth), Color.White);
            //Upper border
            Vector2 offset = new Vector2(0, -3);
            Vector2 wh = new Vector2(_textSize.X * 0.5f, 3);
            _vertices[9] = new VertexPositionColor(new Vector3(_textStart+offset, depth), Color.White);
            _vertices[10] = new VertexPositionColor(new Vector3(_textStart +offset+new Vector2(wh.X,0), depth), Color.White);
            _vertices[11] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, wh.Y), depth), Color.White);
            _vertices[12] = new VertexPositionColor(new Vector3(_textStart +offset, depth), Color.White);
            _vertices[13] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, wh.Y), depth), Color.White);
            _vertices[14] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(0, wh.Y), depth), Color.White);
            //Lower border
            offset = new Vector2(0, _textSize.Y*0.5f);
            wh = new Vector2(_textSize.X * 0.5f, 3);
            _vertices[15] = new VertexPositionColor(new Vector3(_textStart + offset, depth), Color.White);
            _vertices[16] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, 0), depth), Color.White);
            _vertices[17] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, wh.Y), depth), Color.White);
            _vertices[18] = new VertexPositionColor(new Vector3(_textStart + offset, depth), Color.White);
            _vertices[19] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, wh.Y), depth), Color.White);
            _vertices[20] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(0, wh.Y), depth), Color.White);
            //Right border
            offset = new Vector2(_textSize.X*0.5f, 0);
            wh = new Vector2(3, _textSize.Y * 0.5f);
            _vertices[21] = new VertexPositionColor(new Vector3(_textStart + offset, depth), Color.White);
            _vertices[22] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, 0), depth), Color.White);
            _vertices[23] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, wh.Y), depth), Color.White);
            _vertices[24] = new VertexPositionColor(new Vector3(_textStart + offset, depth), Color.White);
            _vertices[25] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, wh.Y), depth), Color.White);
            _vertices[26] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(0, wh.Y), depth), Color.White);
            //Left border
            offset = new Vector2(-3, 0);
            wh = new Vector2(3, _textSize.Y * 0.5f);
            _vertices[27] = new VertexPositionColor(new Vector3(_textStart + offset, depth), Color.White);
            _vertices[28] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, 0), depth), Color.White);
            _vertices[29] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, wh.Y), depth), Color.White);
            _vertices[30] = new VertexPositionColor(new Vector3(_textStart + offset, depth), Color.White);
            _vertices[31] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(wh.X, wh.Y), depth), Color.White);
            _vertices[32] = new VertexPositionColor(new Vector3(_textStart + offset + new Vector2(0, wh.Y), depth), Color.White);
            //Corners
            float angleAdvance = (float)Math.PI / 8;
            float angle = 0;
            float radius = 3;
            int count = 33;
            //Upper right corner
            offset = _textStart + new Vector2(_textSize.X, 0) * 0.5f;
            for (int x = 0; x < 4; x++)
            {
                _vertices[count] = new VertexPositionColor(new Vector3(offset, depth), Color.White);
                _vertices[count+1] = new VertexPositionColor(new Vector3(offset+radius*new Vector2((float)Math.Cos(angle+angleAdvance),(float)-Math.Sin(angle+angleAdvance)), depth), Color.White);
                _vertices[count+2] = new VertexPositionColor(new Vector3(offset + radius * new Vector2((float)Math.Cos(angle), (float)-Math.Sin(angle)), depth), Color.White);
                count += 3;
                angle += angleAdvance;
            }
            //Upper left corner
            offset = _textStart;
            for (int x = 0; x < 4; x++)
            {
                _vertices[count] = new VertexPositionColor(new Vector3(offset, depth), Color.White);
                _vertices[count + 1] = new VertexPositionColor(new Vector3(offset + radius * new Vector2((float)Math.Cos(angle + angleAdvance), (float)-Math.Sin(angle + angleAdvance)), depth), Color.White);
                _vertices[count + 2] = new VertexPositionColor(new Vector3(offset + radius * new Vector2((float)Math.Cos(angle), (float)-Math.Sin(angle)), depth), Color.White);
                count += 3;
                angle += angleAdvance;
            }
            //Lower left corner
            offset = _textStart + new Vector2(0, _textSize.Y) * 0.5f;
            for (int x = 0; x < 4; x++)
            {
                _vertices[count] = new VertexPositionColor(new Vector3(offset, depth), Color.White);
                _vertices[count + 1] = new VertexPositionColor(new Vector3(offset + radius * new Vector2((float)Math.Cos(angle + angleAdvance), (float)-Math.Sin(angle + angleAdvance)), depth), Color.White);
                _vertices[count + 2] = new VertexPositionColor(new Vector3(offset + radius * new Vector2((float)Math.Cos(angle), (float)-Math.Sin(angle)), depth), Color.White);
                count += 3;
                angle += angleAdvance;
            }
            //Lower right corner
            offset = _textStart + new Vector2(_textSize.X, _textSize.Y) * 0.5f;
            for (int x = 0; x < 4; x++)
            {
                _vertices[count] = new VertexPositionColor(new Vector3(offset, depth), Color.White);
                _vertices[count + 1] = new VertexPositionColor(new Vector3(offset + radius * new Vector2((float)Math.Cos(angle + angleAdvance), (float)-Math.Sin(angle + angleAdvance)), depth), Color.White);
                _vertices[count + 2] = new VertexPositionColor(new Vector3(offset + radius * new Vector2((float)Math.Cos(angle), (float)-Math.Sin(angle)), depth), Color.White);
                count += 3;
                angle += angleAdvance;
            }
        }

        private void CalculateTextStart()
        {
            _origTextStart = _origin + new Vector2(-_textSize.X / 4, -_textSize.Y/2 - 16);
        }

        public bool Update(float ms)
        {
            if (_timeOut > 0)
            {
                _timeOut -= ms;
                if (_timeOut <= 0)
                    return false;
            }
            return inSight &&alive;
        }

        public void Draw(SpriteBatch batch, Camera cam)
        {
            if (_speaker != null)
                _origin = _speaker.MidPosition - Vector2.UnitY * _speaker.Rect.Height * 0.5f;
            //CalculateTextStart();
            _textStart = _origTextStart;
            Rectangle rect = cam.ViewSpace;
            if (cam.ViewSpace.Y > _textStart.Y)
                _textStart.Y += 32 + _textSize.Y;
            if (cam.ViewSpace.X > _textStart.X && _textStart.X + _textSize.X * 0.5f > cam.ViewSpace.X)
                _textStart.X += 0.9f * (cam.ViewSpace.X - _textStart.X);
            else if (cam.ViewSpace.Right < _textStart.X + _textSize.X * 0.5f && _textStart.X < cam.ViewSpace.Right)
                _textStart.X += 0.9f * (cam.ViewSpace.Right - _textStart.X - _textSize.X * 0.5f);
            if (_textStart.X > rect.Right || _textStart.X + _textSize.X * 0.5f < rect.Left || _textStart.Y > rect.Bottom || _textStart.Y + _textSize.Y*0.5f < rect.Top)
                inSight = false;
            
            if (_effect == null)
            {
                _effect = new BasicEffect(batch.GraphicsDevice);
                _effect.VertexColorEnabled = true;
                _effect.LightingEnabled = false;
                _effect.TextureEnabled = false;
            }
            CalculateVertices();
            _effect.World = Matrix.Identity;
            _effect.View = cam.ViewMatrix;
            _effect.Projection = cam.ProjectionMatrix;
            _effect.CurrentTechnique.Passes[0].Apply();
            batch.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            batch.GraphicsDevice.BlendState = BlendState.Opaque;
            batch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length / 3);
            batch.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            batch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            batch.DrawString(_font, _text, _textStart, Color.Black,0, new Vector2(0,0),0.5f,SpriteEffects.None,0.11f);
        }
    }
}
