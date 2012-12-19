using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using IGORR.Content;

namespace IGORR.Editor.ParticleEditor
{
    class ParticleDisplay : GraphicsDeviceControl
    {
        List<Particle> _particles;

        protected override void Initialize()
        {
            SetUpContent();
            _particles = new List<Particle>();
            Application.Idle += delegate { Invalidate(); };
        }

        protected void UpdateInput()
        {
            for (int x = 0; x < _particles.Count; x++)
            {
                _particles[x].Update();
            }
        }

        protected override void Draw()
        {
            UpdateInput();

            for (int x = 0; x < _particles.Count; x++)
            {
                _particles[x].Draw();
            }
        }

        public void SetUpContent()
        {
            ContentInterface.SetContent(Services, "Content", "Content.7z");
        }

        public void Save()
        {
        }
    }
}
