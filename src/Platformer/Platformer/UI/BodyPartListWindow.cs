using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Content;
using IGORR.Client.Logic;

namespace IGORR.Client.UI
{
    class BodyPartListWindow : UIWindow
    {
        const int partSize = 16;
        const int sideOffset = 8;
        float iconSpread = 1.3f;
        Vector2 _size;
        Player _player;

        public BodyPartListWindow(Vector2 position, Vector2 size, Player player)
            : base(position, size, ContentInterface.LoadTexture("UITest"), ContentInterface.LoadTexture("UITest"))
        {
            _size = size;
            _player = player;
            UpdateContent();
        }

        public void UpdateContent()
        {
            int rowLength = (int)((_size.X - 2 * sideOffset) / (partSize * iconSpread));
            for (int x = 0; x < _player.Parts.Count; x++)
            {
                int row = x / rowLength;
                AddChild(new UI.PictureBox(this, new Vector2(sideOffset + x % rowLength * partSize * iconSpread, 15 + row * partSize * iconSpread), new Vector2(16, 16), _player.Parts[x]._texture));
            }
        }
    }
}
