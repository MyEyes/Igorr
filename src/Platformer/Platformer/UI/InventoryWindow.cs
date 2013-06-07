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
    class InventoryWindow : UIWindow,IDroppable
    {
        const int partSize = 16;
        const int sideOffset = 8;
        float iconSpread = 1.3f;
        Inventory _inventory;

        int _lastActions;

        public InventoryWindow(Vector2 position, Vector2 size, Inventory inventory)
            : base(position, size, ContentInterface.LoadTexture("UITest"), ContentInterface.LoadTexture("UITest"))
        {
            _size = size;
            _inventory = inventory;
            _lastActions = inventory.Actions;
            UpdateContent();
        }

        public override void Update(float ms, Microsoft.Xna.Framework.Input.MouseState mouse)
        {
            if (_lastActions != _inventory.Actions)
            {
                _lastActions = _inventory.Actions;
                UpdateContent();
            }
            base.Update(ms, mouse);
        }

        public void UpdateContent()
        {
            _childs.RemoveRange(3, _childs.Count - 3);
            int rowLength = (int)((_size.X - 2 * sideOffset) / (partSize * iconSpread));
            for (int x = 0; x < _inventory.Count; x++)
            {
                int row = x / rowLength;
                AddChild(new UI.ItemIcon(this, new Vector2(sideOffset + x % rowLength * partSize * iconSpread, 15 + row * partSize * iconSpread), _inventory[x]));
            }
        }

        public bool Drop(IDraggable item)
        {
            ItemIcon ii = item as ItemIcon;
            if (ii == null || ii.Item==null)
                return false;
            UIElement element = item as UIElement;
            Vector2 relPos = element.TotalOffset - this.TotalOffset + 0.5f * new Vector2(item.Rect.Width, item.Rect.Height);
            if (relPos.X < sideOffset || relPos.X > _size.X - sideOffset || relPos.Y < 0 || relPos.Y > _size.Y)
                return false;
            int rowLength = (int)((_size.X - 2 * sideOffset) / (partSize * iconSpread));
            int row = (int)((relPos.Y - 15) / (partSize * iconSpread));
            int column = (int)((relPos.X - sideOffset) / (partSize * iconSpread));
            column = column >= 0 ? column : 0;
            column = column < rowLength ? column : rowLength - 1; 
            int index = column + row * rowLength;
            item.Drop(this);
            _inventory.Insert(ii.Item, index);
            UpdateContent();
            return true;
        }

        public void Remove(IDraggable item)
        {
            ItemIcon ii = item as ItemIcon;
            if (ii != null)
            {
                _inventory.Remove(ii.Item);
                RemoveChild(item as UIElement);
                UpdateContent();
            }
        }
    }
}
