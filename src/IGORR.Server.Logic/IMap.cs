using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace IGORR.Server.Logic
{
    public interface IMap
    {
        IObjectManager ObjectManager { get; }
        bool Collides(GameObject obj);
        void DoPlayerEvents(Player player);
        void AddObject(EventObject obj);
        void ChangeTile(int layer, Vector2 position, int tileID);
        bool GetTrigger(string triggerName);
        void SetTrigger(string triggerName, bool val);
        void TimeSpawn(int id, Vector2 position, float countdown);
        Rectangle MapBoundaries { get; }
    }
}
