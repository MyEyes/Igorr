using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Client.UI
{
    interface IDraggable
    {
        void StartDrag();
        void Drop(UIElement element);
        Rectangle Rect { get; }
    }
}
