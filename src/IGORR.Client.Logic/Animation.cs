using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    public class AnimationContainer
    {
        Animation _animation;
        Texture2D _texture;
        Vector2 _size;

        public AnimationContainer(string name)
        {
            string file = Content.ContentInterface.LoadFile("animations/"+name);
            if (string.IsNullOrEmpty(file))
            {
                _texture = Content.ContentInterface.DefaultTexture;
                _animation = new Animation(0, 0, 0, new int[] { 0 });
                return;
            }
            string[] lines = file.Split(new string[]{"\n", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            _texture = Content.ContentInterface.LoadTexture(lines[0]);
            string[] frameStrings = lines[1].Split(' ');
            int timeDiff;
            int.TryParse(frameStrings[0],out timeDiff);
            int[] frames = new int[frameStrings.Length-1];
            for (int x = 1; x < frameStrings.Length; x++)
            {
                int.TryParse(frameStrings[x], out frames[x-1]);
            }
            _animation = new Animation(timeDiff, _texture.Height, _texture.Height, frames);
            _size = new Vector2(_texture.Height, _texture.Height);
        }

        public AnimationContainer(Texture2D texture, Animation animation)
        {
            _animation = animation;
            _texture = texture;
            _size = new Vector2(_texture.Height, _texture.Height);
        }

        public bool Update(float ms)
        {
            _animation.Update(ms);
            if (_animation.Finished)
                return false;
            return true;
        }

        public void Draw(SpriteBatch batch, Vector2 targetPos, bool Left)
        {
            batch.Draw(_texture, targetPos - _size / 2, _animation.GetFrame(), Color.White, 0, Vector2.Zero, 1, Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.43f);
        }
    }

    public class Animation
    {
        float _frameDiff;
        float _counter;
        Rectangle[] _frames;
        int frame;
        bool _finished = false;

        public Animation(float frameDiff, int dimX, int dimY, int[] frames)
        {
            _frameDiff = frameDiff;
            _counter = 0;
            _frames = new Rectangle[frames.Length];
            for (int x = 0; x < _frames.Length; x++)
                _frames[x] = new Rectangle(frames[x] * dimX, 0, dimX, dimY);
        }

        public void Update(float ms)
        {
            _counter-=ms;
            if (_counter < 0)
            {
                _counter = _frameDiff;
                frame++;
                frame %= _frames.Length;
                if (frame == 0)
                    _finished = true;
            }
        }

        public void SetFrame(int num)
        {
            frame = num;
            _counter = _frameDiff;
        }

        public Rectangle GetFrame()
        {
            return _frames[frame];
        }

        public int GetFrameNum()
        {
            return frame;
        }

        public bool Finished
        {
            get { return _finished; }
        }
    }
}
