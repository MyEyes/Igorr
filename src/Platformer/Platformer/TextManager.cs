using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Client.Logic;

namespace IGORR.Client
{

    class InfoText
    {
        public string text;
        public float countdown;
    }

    static class TextManager
    {
        static List<BattleText> _texts;
        static List<InfoText> _info;
        static List<TextBubble> _bubbles;
        static SpriteFont _font;

        static TextManager()
        {
            _texts = new List<BattleText>();
            _info = new List<InfoText>();
            _bubbles = new List<TextBubble>();
        }

        public static void SetUp(SpriteFont font)
        {
            _font = font;
        }

        public static void Update(float ms)
        {
            for (int x = 0; x < _texts.Count; x++)
            {
                if (!_texts[x].Update(ms))
                {
                    _texts.RemoveAt(x);
                    x--;
                }
            }
            for (int x = 0; x < _info.Count; x++)
            {
                if (_info[x].countdown < 0)
                {
                    _info.RemoveAt(x);
                    x--;
                }
                else
                    _info[x].countdown -= ms;
            }
            for (int x = 0; x < _bubbles.Count; x++)
            {
                if (!_bubbles[x].Update(ms))
                {
                    _bubbles.RemoveAt(x);
                    x--;
                }
            }
        }

        public static void AddText(Vector2 position, float gravity, string text, float time, Color start, Color end)
        {
            _texts.Add(new BattleText(position, gravity, text, time, start, end, _font));
        }

        public static void Draw(SpriteBatch batch, IGORR.Client.Logic.Camera cam)
        {
            batch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, DepthStencilState.Default, null, null, cam.ViewMatrix);
            for (int x = 0; x < _texts.Count; x++)
                _texts[x].Draw(_font, batch);
            for (int x = 0; x < _bubbles.Count; x++)
                _bubbles[x].Draw(batch,cam);
            batch.End();
        }

        public static void Say(GameObject obj, string Text, float timeout)
        {
            _bubbles.Add(new TextBubble(Text,obj,timeout));
        }

        public static void Say(Vector2 pos, string Text, float timeout)
        {
            _bubbles.Add(new TextBubble(Text, pos, timeout));
        }

        public static void AddInfo(string info)
        {
            InfoText nfo = new InfoText();
            nfo.countdown = 5000;
            nfo.text = info;
            _info.Add(nfo);
        }

        public static void DrawInfo(SpriteBatch batch)
        {
            for (int x = 0; x < _info.Count; x++)
            {
                batch.DrawString(_font, _info[x].text, new Vector2(400, 10 + 30 * x), Color.White);
            }
        }
    }
}
