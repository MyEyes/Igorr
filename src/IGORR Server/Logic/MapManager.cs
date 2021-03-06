﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IGORR.Server.Logic
{
    static class MapManager
    {
        static List<Map> _maps;

        public static void LoadMaps(IGORR.Server.Server server)
        {
            Console.WriteLine();
            _maps = new List<Map>();
            string curDir=Directory.GetCurrentDirectory();
            StreamReader reader = new StreamReader("Content\\map\\maps.lst");
            string[] lines = reader.ReadToEnd().Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int x = 0; x < lines.Length; x++)
            {
                _maps.Add(new Map(lines[x],server,x));
            }
        }

        public static Map GetMapByID(int id)
        {
            if (id>=0 && id < _maps.Count)
                return _maps[id];
            return null;
        }

        public static void Update(float ms)
        {
            for (int x = 0; x < _maps.Count; x++)
            {
                if (_maps[x].Players > 0 || !_maps[x].TimedOut)
                    _maps[x].Update(ms);
            }
        }


        public static int GetMapID(IMap map)
        {
            for (int x = 0; x < _maps.Count; x++)
                if (map!=null && map.ID == _maps[x].ID)
                    return x;
            return -1;
        }

        public static int ActiveMaps()
        {
            int counter = 0;
            for (int x = 0; x < _maps.Count; x++)
            {
                if (_maps[x].Players > 0 || !_maps[x].TimedOut)
                    counter++;
            }
            return counter;
        }
    }
}
