using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IGORR.Client.UI
{
    class ItemIcon:PictureBox,IDraggable
    {
        bool dragged = false;
        Vector2 oldPos;
        Logic.ICollectible _item;

        public ItemIcon(UIElement parent, Vector2 position, Logic.ICollectible item)
            : base(parent, position, new Vector2(16, 16), item.Texture)
        {
            oldPos = position;
            _item = item;
        }

        public override void Update(float ms, MouseState mouse)
        {
            if (!dragged && (mouse.LeftButton == ButtonState.Pressed) && (_lastMouse.LeftButton != ButtonState.Pressed) && new Rectangle((int)TotalOffset.X, (int)TotalOffset.Y, (int)_size.X, (int)_size.Y).Contains(mouse.X, mouse.Y))
                StartDrag();
            if (dragged)
            {
                this.SetOffset(new Vector2(mouse.X, mouse.Y) - _parent.TotalOffset - _size*0.5f);
                if (mouse.LeftButton == ButtonState.Released)
                {
                    if (!this.TryDrop(this, this))
                    {
                        this.SetOffset(oldPos);
                    }
                    dragged = false;
                }
            }
            base.Update(ms,mouse);
        }

        public void StartDrag()
        {
            dragged = true;
            oldPos = Offset;
        }

        public void Drop(UIElement element)
        {
            InventoryWindow invWin = _parent as InventoryWindow;
            if (invWin != null)
            {
                invWin.Remove(this);
                invWin.UpdateContent();
            }
            _parent.RemoveChild(this);
        }

        public Rectangle Rect
        {
            get { return new Rectangle((int)TotalOffset.X, (int)TotalOffset.Y, (int)_size.X, (int)_size.Y); }
        }

        public Logic.ICollectible Item
        {get {return _item;}}
    }
}
