using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Client
{
    class Animation
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
