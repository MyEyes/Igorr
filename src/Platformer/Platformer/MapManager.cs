using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace IGORR.Game
{
    static class MapManager
    {
        static List<Map> _maps;

        public static void LoadMaps(ContentManager content, GraphicsDevice dev)
        {
            _maps = new List<Map>();
            StreamReader reader = new StreamReader("maps.lst");
            string[] lines = reader.ReadToEnd().Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int x = 0; x < lines.Length; x++)
            {
                _maps.Add(new Map(lines[x],content,dev));
            }
        }

        public static Map GetMapByID(int id)
        {
            if (id < _maps.Count)
                return _maps[id];
            return null;
        }

        public static void Update(float ms)
        {
        }

    }
}
