using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Client.Logic;
using IGORR.Protocol;
using IGORR.Protocol.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client
{
    public struct Choice
    {
        public int id;
        public string text;

        public Choice(int id, string text)
        {
            this.id = id;
            this.text = text;
        }
    }

    class InteractiveTextBubble:TextBubble
    {
        Choice[] _choices;
        Rectangle[] _choiceRects;
        Vector2 _offset;
        float _rectOffset;
        static Texture2D _selectRect;
        int selected = -1;
        Vector2 _worldSpaceMouse;
        MouseState _prevMouse;

        public InteractiveTextBubble(Choice[] choices, string text, GameObject partner):base(text,partner,0)
        {
            _selectRect = IGORR.Content.ContentInterface.LoadTexture("selectRect");
            string bubbleText = text + Environment.NewLine;
            _offset = _font.MeasureString(bubbleText)/2.0f;
            _offset.X = 0;
            _choices = choices;
            _rectOffset = _font.MeasureString("- ").X/2.0f;
            _choiceRects = new Rectangle[choices.Length];
            for (int x = 0; x < _choices.Length; x++)
            {
                _choices[x].text = "- " + _choices[x].text;
                bubbleText += Environment.NewLine+_choices[x].text;
                Vector2 size = _font.MeasureString(_choices[x].text);
                _choiceRects[x] = new Rectangle(0, 0, (int)(size.X/2.0f), (int)(size.Y/2.0f));
            }
            SetText(bubbleText);
            _prevMouse = Mouse.GetState();
        }

        private void CalculateRects()
        {
            Vector2 offset = _offset;
            for (int x = 0; x < _choices.Length; x++)
            {
                _choiceRects[x].X = (int)(offset.X + _textStart.X);
                _choiceRects[x].Y = (int)(offset.Y + _textStart.Y);
                offset.Y += _font.LineSpacing/2.0f;
            }
        }

        public override bool Update(float ms)
        {
            MouseState mouse = Mouse.GetState();

            bool standard = base.Update(ms);
            CalculateRects();
            if (mouse.LeftButton == ButtonState.Pressed && _prevMouse.LeftButton == ButtonState.Released)
                Choose(selected);
            _prevMouse = mouse;

            return standard;
        }

        void Choose(int selected)
        {
            if (selected < 0 || selected > _choices.Length)
            {
                alive = false;
                return;
            }
            if (_choiceRects[selected].Contains((int)_worldSpaceMouse.X, (int)_worldSpaceMouse.Y))
            {
                InteractMessage im = (InteractMessage)WorldController.ProtocolHelper.NewMessage(MessageTypes.Interact);
                im.action = InteractAction.Respond;
                im.info = _choices[selected].id;
                if (_speaker != null)
                    im.objectID = _speaker.ID;
                im.Encode();
                WorldController.SendReliable(im);
                alive = false;
            }
        }

        public override void Draw(SpriteBatch batch, Logic.Camera cam)
        {
            base.Draw(batch, cam);
            _worldSpaceMouse = cam.ViewToWorldPosition(new Vector2(_prevMouse.X, _prevMouse.Y));
            selected = -1;
            for (int x = 0; x < _choiceRects.Length; x++)
                if (_choiceRects[x].Contains((int)_worldSpaceMouse.X, (int)_worldSpaceMouse.Y))
                    selected = x;
            if (selected >= 0 && selected < _choices.Length)
            {
                Rectangle rect = _choiceRects[selected];
                rect.X -= 2-(int)_rectOffset;
                rect.Width += 4-(int)_rectOffset;
                batch.Draw(_selectRect, rect, Color.Orange);
            }
        }
    }
}
