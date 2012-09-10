using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using IGORRProtocol;
using IGORRProtocol.Messages;

namespace IGORR_Server
{
    public class Client
    {
        static int idcounter = 1;
        NetConnection _clientConnection;
        int playerID;
        int id;
        string _name;
        Logic.Map _currentMap;

        public Client(NetConnection connection, string name)
        {
            id = idcounter++;
            _clientConnection = connection;
            _name = name;
        }

        public NetConnection Connection
        {
            get { return _clientConnection; }
        }

        public void SetMap(Logic.Map map, Vector2 pos)
        {
            Logic.Player p = null;
            if (_currentMap != null)
            {
                p = _currentMap.ObjectManager.GetPlayer(playerID);
                _currentMap.ObjectManager.Remove(playerID);
            }
            _currentMap = map;

            if (p != null)
            {
                p.SetPosition(pos);
                _currentMap.ObjectManager.Add(p);
            }

            IGORRProtocol.Messages.ChangeMapMessage cmm = (IGORRProtocol.Messages.ChangeMapMessage)IGORRProtocol.Protocol.NewMessage(MessageTypes.ChangeMap);
            cmm.mapid = map.ID;
            cmm.Encode();
            _clientConnection.SendMessage(cmm.GetMessage(), NetDeliveryMethod.ReliableOrdered, 1);

            SpawnMessage sm;
            for (int x = 0; x < _currentMap.ObjectManager.Objects.Count; x++)
            {
                sm = (SpawnMessage)Protocol.GetContainerMessage(MessageTypes.Spawn, Connection);
                sm.position = _currentMap.ObjectManager.Objects[x].MidPosition;
                sm.objectType = _currentMap.ObjectManager.Objects[x].ObjectType;
                sm.id = _currentMap.ObjectManager.Objects[x].ID;
                sm.Name = _currentMap.ObjectManager.Objects[x].Name;
                if (_currentMap.ObjectManager.Objects[x] is Logic.Player)
                    sm.CharName = (_currentMap.ObjectManager.Objects[x] as Logic.Player).CharFile;
                Protocol.SendContainer(sm, Connection);
                if(_currentMap.ObjectManager.Objects[x] is Logic.Player)
                {
                    Logic.Player play=_currentMap.ObjectManager.Objects[x] as Logic.Player;
                SetPlayerStatusMessage dm = (SetPlayerStatusMessage)Protocol.GetContainerMessage(MessageTypes.SetHP, Connection);
                dm.playerID = play.ID;
                dm.currentHP = play.HP;
                dm.maxHP = play.MaxHP;
                dm.Exp = play.TotalXP;
                dm.lastLevelExp = play.LastLevelXP;
                dm.nextLevelExp = play.NextLevelXP;
                Protocol.SendContainer(dm, Connection);
                }
            }

            for (int x = 0; x < _currentMap.TileMods.Count; x++)
            {
                ChangeTileMessage ctm = (ChangeTileMessage)Protocol.GetContainerMessage(MessageTypes.ChangeTile, Connection);
                ctm.tileID = _currentMap.TileMods[x].TileID;
                ctm.x = _currentMap.TileMods[x].Position.X;
                ctm.y = _currentMap.TileMods[x].Position.Y;
                ctm.layer = _currentMap.TileMods[x].layer;
                Protocol.SendContainer(ctm, Connection);
            }
            if (p != null)
            {
                AssignPlayerMessage apm = (AssignPlayerMessage)Protocol.GetContainerMessage(MessageTypes.AssignPlayer, Connection);
                apm.objectID = p.ID;
                Protocol.SendContainer(apm, Connection);
            }
            PlayMessage pm = (PlayMessage)Protocol.GetContainerMessage(MessageTypes.Play, Connection);
            pm.Loop = true;
            pm.Queue = false;
            pm.SongName = "Level01";

            if (p != null)
            {
                for (int x = 0; x < p.Parts.Count; x++)
                {
                    PickupMessage pum = (PickupMessage)Protocol.GetContainerMessage(MessageTypes.Pickup, Connection);
                    pum.id = p.Parts[x].GetID();
                    Protocol.SendContainer(pum, Connection);
                }
            }

            Protocol.FlushContainer(Connection, 1);
        }

        public int ID
        {
            get { return id; }
        }

        public int PlayerID
        {
            get { return playerID; }
            set { playerID = value; }
        }

        public Logic.Map CurrentMap
        {
            get { return _currentMap; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
