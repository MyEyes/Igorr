using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using IGORR.Content;
namespace IGORR.Client.Logic
{

    public interface IMap
    {
        bool Collides(Vector2 position);
        bool Collides(GameObject obj);
        void RemoveEvent(EventObject obj);
        EventObject GetEvent(Player player);
    }

}
