using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.UI
{
    class UIElement
    {
        protected UIElement _parent;
        protected List<UIElement> _childs;
        Vector2 _offset;

        public UIElement(UIElement parent, Vector2 offset)
        {
            _parent = parent;
            _offset = offset;
            _childs = new List<UIElement>();
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

        protected void UpdateChildren(float ms)
        {
            for (int x = 0; x < _childs.Count; x++)
                _childs[x].Update(ms);
        }

        public virtual void Draw(SpriteBatch batch) { DrawChildren(batch); }
        public virtual void Update(float ms) { UpdateChildren(ms); }

        public Vector2 Offset { get { return _offset; } }
        public Vector2 TotalOffset { get { return (_parent != null ? _parent.TotalOffset : Vector2.Zero) + _offset; } }
    }
}
