using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.UI
{
    class UIElement
    {
        protected UIElement _parent;
        protected List<UIElement> _childs;
        protected Vector2 _size;
        static protected MouseState _lastMouse;
        Vector2 _offset;
        protected float depth;
        const float minDepth = 0.1f;
        const float maxDepth = 0.05f;

        public UIElement(UIElement parent, Vector2 offset)
        {
            _parent = parent;
            _offset = offset;
            _childs = new List<UIElement>();
            depth=minDepth;
            if (parent != null)
                depth = parent.depth + maxDepth - parent.depth / 10.0f;
        }

        public void SetOffset(Vector2 offset)
        {
            _offset = offset;
        }

        public void AddChild(UIElement element)
        {
            _childs.Add(element);
            element._parent = this;
        }

        public void RemoveChild(UIElement element)
        {
            _childs.Remove(element);
        }

        protected void DrawChildren(SpriteBatch batch)
        {
            for (int x = 0; x < _childs.Count; x++)
            {
                _childs[x].Draw(batch);
            }
        }

        protected void UpdateChildren(float ms, MouseState mouse)
        {
            for (int x = 0; x < _childs.Count; x++)
                _childs[x].Update(ms, mouse);
        }

        public bool TryDrop(UIElement caller, IDraggable obj)
        {
            IDroppable drop = this as IDroppable;
            if(drop!=null && obj.Rect.Intersects(new Rectangle((int)TotalOffset.X,(int)TotalOffset.Y,(int)_size.X,(int)_size.Y)))
            {
                if (drop.Drop(obj))
                    return true;
            }
            for (int x = 0; x < _childs.Count; x++)
                if (_childs[x] != caller) if (_childs[x].TryDrop(this, obj)) return true;
            if (_parent!=null && _parent != caller)
                if (_parent.TryDrop(this, obj)) return true;
            return false;
        }

        public virtual void Draw(SpriteBatch batch) { DrawChildren(batch); }
        public virtual void Update(float ms, MouseState mouse) 
        {
            UpdateChildren(ms,mouse);
        }

        public Vector2 Offset { get { return _offset; } }
        public Vector2 TotalOffset { get { return (_parent != null ? _parent.TotalOffset : Vector2.Zero) + _offset; } }
    }
}
