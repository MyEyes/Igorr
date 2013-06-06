using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Client.UI
{
    class CharacterWindow:UIWindow
    {
        Label _nameLabel, _nameVLabel;
        Label _LevelLabel, _LevelVLabel;
        Logic.Player _player;
        public CharacterWindow(Vector2 position, Vector2 size, Logic.Player player)
            :base(position, size, Content.ContentInterface.LoadTexture("UITest"),Content.ContentInterface.LoadTexture("UITest"))
        {
            _player = player;
            _nameLabel = new Label(this, new Vector2(10, 4), "Name:");
            _nameVLabel = new Label(this, new Vector2(100, 4), player.Name);
            _LevelLabel = new Label(this, new Vector2(10, 22), "Level:");
            _LevelVLabel = new Label(this, new Vector2(100, 22), player.Level.ToString());
            AddChild(_nameLabel);
            AddChild(_nameVLabel);
            AddChild(_LevelLabel);
            AddChild(_LevelVLabel);
        }

        public void Update()
        {
            _nameVLabel.SetText(_player.Name);
            _LevelVLabel.SetText(_player.Level.ToString());
        }
    }
}
