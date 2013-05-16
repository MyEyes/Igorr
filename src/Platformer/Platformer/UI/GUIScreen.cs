using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.UI
{
    class GUIScreen : UIScreen
    {
        BodyPartListWindow _inventory;
        InGameMenu _menu;
        UIWindow _buttonWindow;
        Logic.Player _player;

        public GUIScreen()
        {
            _buttonWindow = new UIWindow(new Vector2(0, 0), new Vector2(100, 80), Content.ContentInterface.LoadTexture("UITest"), Content.ContentInterface.LoadTexture("UITest"));
            AddChild(_buttonWindow);
            _buttonWindow.AddChild(new Button(_buttonWindow, new Vector2(9, 10), new Vector2(82, 24), Content.ContentInterface.LoadTexture("UITest"),
                delegate { if(_player!=null)ToggleInventoryWindow(); }, "Inventory"));
            _buttonWindow.AddChild(new Button(_buttonWindow, new Vector2(9, 44), new Vector2(82, 24), Content.ContentInterface.LoadTexture("UITest"),
    delegate { if (_player != null)ToggleMenuWindow(); }, "Menu"));
        }

        public void SetPlayer(Logic.Player player)
        {
            _player = player;
            _inventory = null;
        }

        public void UpdateInventoryWindow()
        {
            if (_inventory != null)
                _inventory.UpdateContent();
        }

        public void ToggleInventoryWindow()
        {
            if(_inventory==null && _player!=null)
                _inventory=new UI.BodyPartListWindow(new Vector2(200, 200), new Vector2(95, 100), _player);
            if (!_childs.Contains(_inventory))
                AddChild(_inventory);
            else
                RemoveChild(_inventory);
        }

        public void ToggleMenuWindow()
        {
            if (_menu == null && _player != null)
                _menu = new UI.InGameMenu(new Vector2(300, 200), new Vector2(200, 280), _manager.Game);
            if (!_childs.Contains(_menu))
                AddChild(_menu);
            else
                RemoveChild(_menu);
        }

    }
}
