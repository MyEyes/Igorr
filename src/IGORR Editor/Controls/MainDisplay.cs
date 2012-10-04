using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using IGORR.Content;

namespace IGORR.Editor
{
    class MainDisplay : GraphicsDeviceControl
    {

        protected override void Initialize()
        {
            Application.Idle += delegate { Invalidate(); };
        }

        protected void UpdateInput()
        {
        }

        protected override void Draw()
        {
        }

        public void SetUpContent()
        {
        }

        public void Save()
        {
        }
    }
}
