using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using IGORR.Protocol;
using IGORR.Protocol.Messages;
using IGORR.Server.Logic;

namespace IGORR.Server
{
    public class Client
    {
        static int idcounter = 1;
        NetConnection _clientConnection;
        int playerID;
        int id;
        string _name;
        Map _currentMap;

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

        public void SetMap(Map map, Vector2 pos)
        {
            Player p = null;
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

            IGORR.Protocol.Messages.ChangeMapMessage cmm = (IGORR.Protocol.Messages.ChangeMapMessage)IGORR.Protocol.ProtocolHelper.GetContainerMessage(MessageTypes.ChangeMap, Connection);
            cmm.mapid = map.ID;
            ProtocolHelper.SendContainer(cmm, Connection);
            //_clientConnection.SendMessage(cmm.GetMessage(), NetDeliveryMethod.ReliableOrdered, 1);

            if (p != null)
            {
                AssignPlayerMessage apm = (AssignPlayerMessage)ProtocolHelper.GetContainerMessage(MessageTypes.AssignPlayer, Connection);
                apm.objectID = p.ID;
                ProtocolHelper.SendContainer(apm, Connection);
            }

            SpawnMessage sm;
            for (int x = 0; x < _currentMap.ObjectManager.Objects.Count; x++)
            {
                sm = (SpawnMessage)ProtocolHelper.GetContainerMessage(MessageTypes.Spawn, Connection);
                sm.position = _currentMap.ObjectManager.Objects[x].Rect;
                sm.objectType = _currentMap.ObjectManager.Objects[x].ObjectType;
                sm.id = _currentMap.ObjectManager.Objects[x].ID;
                sm.Info = _currentMap.ObjectManager.Objects[x].Info;
                ProtocolHelper.SendContainer(sm, Connection);
                _currentMap.ObjectManager.Objects[x].SendInfo(Connection);
                if(_currentMap.ObjectManager.Objects[x] is Player)
                {
                    Player play=_currentMap.ObjectManager.Objects[x] as Player;
                SetPlayerStatusMessage dm = (SetPlayerStatusMessage)ProtocolHelper.GetContainerMessage(MessageTypes.SetHP, Connection);
                dm.playerID = play.ID;
                dm.currentHP = play.HP;
                dm.maxHP = play.MaxHP;
                dm.Exp = 0;// play.TotalXP;
                dm.lastLevelExp = 0;// play.LastLevelXP;
                dm.nextLevelExp = 0;// play.NextLevelXP;
                ProtocolHelper.SendContainer(dm, Connection);
                }
            }

            for (int x = 0; x < _currentMap.TileMods.Count; x++)
            {
                ChangeTileMessage ctm = (ChangeTileMessage)ProtocolHelper.GetContainerMessage(MessageTypes.ChangeTile, Connection);
                ctm.tileID = _currentMap.TileMods[x].TileID;
                ctm.x = _currentMap.TileMods[x].Position.X;
                ctm.y = _currentMap.TileMods[x].Position.Y;
                ctm.layer = _currentMap.TileMods[x].layer;
                ProtocolHelper.SendContainer(ctm, Connection);
            }
            PlayMessage pm = (PlayMessage)ProtocolHelper.GetContainerMessage(MessageTypes.Play, Connection);
            pm.Loop = true;
            pm.Queue = false;
            pm.SongName = "Level01";

            if (p != null)
            {
                for (int x = 0; x < p.Inventory.Count; x++)
                {
                    PickupMessage pum = (PickupMessage)ProtocolHelper.GetContainerMessage(MessageTypes.Pickup, Connection);
                    pum.id = p.Inventory[x].GetID();
                    ProtocolHelper.SendContainer(pum, Connection);
                }
                p.Body.SendBody(Connection, true);
            }

            ProtocolHelper.FlushContainer(Connection, 1);
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

        public Map CurrentMap
        {
            get { return _currentMap; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
