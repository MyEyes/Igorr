using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IGORR.Server.Management
{
    public struct PlayerInfo
    {
        public int Map;
        public int PosX;
        public int PosY;
        public List<int> Parts;
    }

    class ClientInfo
    {
        Dictionary<string, int> _infoValues = new Dictionary<string, int>();
        List<int> _partIDs;
        int _map;
        int _posX;
        int _posY;

        public ClientInfo(StreamReader reader)
        {
            string firstLine = reader.ReadLine();
            string[] split = firstLine.Split(',');
            _map = int.Parse(split[0]);
            _posX = int.Parse(split[1]);
            _posY = int.Parse(split[2]);
            _partIDs = new List<int>();
            for (int x = 3; x < split.Length; x++)
                _partIDs.Add(int.Parse(split[x]));
            while (!reader.EndOfStream)
            {
                string[] lineSplit = reader.ReadLine().Split(',');
                SetValue(lineSplit[0], int.Parse(lineSplit[1]));
            }
        }

        public ClientInfo(Logic.Player player)
        {
            _partIDs = new List<int>();
            UpdateInfo(player);
        }

        public ClientInfo()
        {
            _partIDs = new List<int>();
        }

        public int GetValue(string infoName)
        {
            if (_infoValues.ContainsKey(infoName))
                return _infoValues[infoName];
            return -1;
        }

        public void SetValue(string infoName, int value)
        {
            if (!_infoValues.ContainsKey(infoName))
                _infoValues.Add(infoName, value);
            else
                _infoValues[infoName] = value;
        }

        public void Write(StreamWriter writer)
        {
            writer.Write(_map.ToString() + "," + _posX.ToString() + "," + _posY.ToString());
            for (int x = 0; x < _partIDs.Count; x++)
                writer.Write("," + _partIDs[x]);
            writer.WriteLine();
            foreach (string s2 in _infoValues.Keys)
            {
                int value = GetValue(s2);
                if (value != -1)
                    writer.WriteLine(s2 + "," + value.ToString());
            }
        }

        public void UpdateInfo(Logic.Player player)
        {
            _map = Logic.MapManager.GetMapID(player.map);
            _posX = (int)player.Position.X;
            _posY = (int)player.Position.Y;
            _partIDs.Clear();
            foreach (Logic.BodyPart part in player.Parts)
            {
                _partIDs.Add(part.GetID());
            }
        }

        public PlayerInfo GetInfo(Logic.Player player)
        {
            PlayerInfo info = new PlayerInfo();
            info.Map = _map;
            info.PosX = (int)_posX;
            info.PosY = (int)_posY;
            info.Parts = _partIDs;
            return info;
        }
    }

    public static class ClientInfoInterface
    {
        static Dictionary<string, ClientInfo> _clientInfos;

        public static void LoadInfos()
        {
            _clientInfos = new Dictionary<string, ClientInfo>();
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
            string[] files = Directory.GetFiles("Data");
            for (int x = 0; x < files.Length; x++)
            {
                LoadInfo(files[x]);
            }
        }

        private static void LoadInfo(string Filename)
        {
            string name = Path.GetFileNameWithoutExtension(Filename);

            StreamReader reader = new StreamReader(Filename);
            _clientInfos.Add(name, new ClientInfo(reader));
            reader.Close();
        }

        public static void StoreInfo()
        {
            foreach(string s in _clientInfos.Keys)
            {
                StreamWriter writer = new StreamWriter("Data/" + s);
                _clientInfos[s].Write(writer);
                writer.Close();
            }
        }

        public static void UpdateInfo(Logic.Player player)
        {
            if (!_clientInfos.ContainsKey(player.Name))
                _clientInfos.Add(player.Name, new ClientInfo(player));
            else
                _clientInfos[player.Name].UpdateInfo(player);
        }

        public static PlayerInfo GetInfo(Logic.Player player)
        {
            if (_clientInfos.ContainsKey(player.Name))
                return _clientInfos[player.Name].GetInfo(player);
            return new PlayerInfo();
        }

        public static int GetValue(string client, string infoName)
        {
            if (_clientInfos.ContainsKey(client))
                return _clientInfos[client].GetValue(infoName);
            else
                return -1;
        }

        public static void SetValue(string client, string infoName, int value)
        {
            if (!_clientInfos.ContainsKey(client))
                _clientInfos.Add(client, new ClientInfo());
            _clientInfos[client].SetValue(infoName, value);
        }
    }
}
