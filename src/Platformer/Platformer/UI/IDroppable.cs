using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR.Client.UI
{
    interface IDroppable
    {
        bool Drop(IDraggable obj);
        void Remove(IDraggable obj);
    }
}
