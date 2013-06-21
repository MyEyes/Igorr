using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IGORR.Server.Logic;
using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR.Server.Logic
{

    public class ObjectManager:IObjectManager
    {
        static int nextID = 0;
        int updateCounter = 0;
        List<GameObject> _objects;
        Map _map;
        IGORR.Server.Server _server;

        IAttackManager _attacks;
        int _playerCounter = 0;
        float _updateTimeout = 0;

        public ObjectManager(IGORR.Server.Server server)
        {
            _server = server;
            _objects = new List<GameObject>();
            _attacks = new AttackManager(server);
        }

        public void SetMap(Map map)
        {
            //_map.Add(map);
            _map = map;
        }

        public void SetServer(IGORR.Server.Server server)
        {
            _server = server;
        }

        public void Add(Vector2 position, int id, int type)
        {
            switch(type)
            {
                case 'a' - 'a': Add(new Player(_map, new Rectangle((int)position.X, (int)position.Y, 16, 15), id)); break;
            }
        }

        public void Add(Vector2 position, int id, string Name)
        {
            Player player = new Player(_map, new Rectangle((int)position.X, (int)position.Y, 16, 15), id);
            player.Name = Name;
            Add(player);
        }

        public void Add(GameObject obj)
        {
            _server.SetChannel(1);
            _objects.Add(obj);
            Player player = obj as Player;
            if (player!=null && !(obj is NPC))
                _playerCounter++;
            SpawnMessage spawn = (SpawnMessage)Server.ProtocolHelper.NewMessage(MessageTypes.Spawn);
            spawn.id = obj.ID;
            spawn.position = obj.Rect;
            spawn.objectType = obj.ObjectType;
            spawn.move = obj.Movement;
            spawn.Info = obj.Info;
            spawn.Encode();

            BodyConfigurationMessage bcm = (BodyConfigurationMessage)Server.ProtocolHelper.NewMessage(MessageTypes.BodyConfiguration);
            if (!(obj is Attack))
            {
                _server.SendAllMapReliable(_map, spawn, true);
            }
            else
                _server.SendAllMapReliable(_map, spawn, false);
        }

        public void Remove(GameObject obj)
        {
            _server.SetChannel(1);
            if (_objects.Contains(obj))
            {
                _objects.Remove(obj);
                DeSpawnMessage deSpawn = (DeSpawnMessage)Server.ProtocolHelper.NewMessage(MessageTypes.DeSpawn);
                deSpawn.id = obj.ID;
                if (obj is Player && !(obj is NPC))
                {
                    _playerCounter--;
                    if (_playerCounter == 0)
                        _updateTimeout = 15f;
                }
                deSpawn.Encode();
                if (!(obj is Attack))
                    _server.SendAllMapReliable(_map, deSpawn, true);
                else
                    _server.SendAllMapReliable(_map, deSpawn, false);
                Console.WriteLine("Despawned " + obj.ID + " " + obj.Name);
            }
        }

        public void RemoveQuiet(GameObject obj)
        {
            if (_objects.Contains(obj))
            {
                if (obj is Player)
                    (obj as Player).GetDamage((obj as Player).HP + 1, -1);
                _objects.Remove(obj);
            }
        }

        public void Remove(int id)
        {
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x].ID == id)
                {
                    Remove(_objects[x]);
                    return;
                }
            }
        }


        public bool UpdatePosition(Vector2 newPos, Vector3 newMove, int id, long timestamp)
        {
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x] is Player)
                {
                    Player player = _objects[x] as Player;
                    if (player.ID == id)
                    {
                        if (timestamp >= player.LastUpdate)
                        {
                            bool sendAgain = false;
                            float timeDiff = timestamp - player.LastUpdate;
                            player.SetUpdateTime(timestamp);
                            sendAgain = (player.Position - newPos).Length() > 5;
                            sendAgain |= player.UpdateCountdown;
                            sendAgain |= (Math.Abs(player.LastSpeed.Y - newMove.Z) > 10 || player.LastMovement.X != newMove.X || player.LastMovement.Y != newMove.Y);

                            //Console.WriteLine(player.Movement.ToString()+" "+newMove.ToString());
                            //if (player.Movement != newMove && newMove == Vector2.Zero)
                                //player.SetPosition(newPos); ;
                            player.SetPosition(newPos);
                            player.SetMove(newMove);
                            return sendAgain;
                        }
                        else
                            return false;
                    }
                }
            }
            return true;
        }

        public GameObject GetObject(int id)
        {
            GameObject obj = null;
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x].ID == id)
                {
                    obj = _objects[x];
                    break;
                }
            }
            return obj;
        }

        public Player GetPlayer(int id)
        {
            Player player = null;
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x].ID == id)
                {
                    if (_objects[x] is Player)
                        player = (Player)_objects[x];
                    break;
                }   
            }
            return player;
        }

        public Player GetPlayerAround(Vector2 position, float radius)
        {
            for (int x = 0; x < _objects.Count; x++)
            {
                if ((_objects[x].Position - position).Length() <= radius && (_objects[x] is Player) && !(_objects[x] is NPC))
                    return _objects[x] as Player;
            }
            return null;
        }

        public Player GetPlayerInArea(Rectangle area)
        {
            
            for (int x = 0; x < _objects.Count; x++)
            {
                if (area.Contains((int)_objects[x].MidPosition.X,(int)_objects[x].MidPosition.Y) && (_objects[x] is Player) && !(_objects[x] is NPC))
                    return _objects[x] as Player;
            }
            return null;
        }

        public Player GetPlayerInArea(Rectangle area, int groupID, bool sameGroup)
        {

            for (int x = 0; x < _objects.Count; x++)
            {
                if (area.Contains((int)_objects[x].MidPosition.X, (int)_objects[x].MidPosition.Y) && (_objects[x] is Player) && !(_objects[x] is NPC) && ((_objects[x] as Player).GroupID==groupID)==sameGroup)
                    return _objects[x] as Player;
            }
            return null;
        }

        public void Update(float ms)
        {
            _server.SetChannel(2);
            if (_server.Enabled)
                updateCounter++;
            if (updateCounter == 2)
                updateCounter = 0;
            if (_playerCounter == 0)
                _updateTimeout -= ms / 1000;
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x] is NPC)
                {
                    NPC npc = _objects[x] as NPC;
                    npc.Update(_map, ms / 1000f);
                    if (_server.Enabled && (updateCounter == 0 || (npc._lastPosition-npc.Position).LengthSquared()>256f || (npc._lastlastSpeed-npc.LastSpeed).LengthSquared()>0.01f))
                    {
                        PositionMessage pm = (PositionMessage)_server.ProtocolHelper.NewMessage(MessageTypes.Position);
                        pm.id = npc.ID;
                        pm.Move = new Vector3(npc.LastMovement, npc.Speed.Y);
                        pm.Position = npc.Position;
                        npc._lastPosition = npc.Position;
                        npc._lastlastSpeed = npc.LastSpeed;
                        //Marked
                        pm.Encode();
                        _server.SendAllMap(_map, pm, false);
                    }
                    int damage = _attacks.CheckPlayer(npc);
                    if (npc.HP <= 0 && _server.Enabled)
                    {
                        KillPlayer(npc);
                    }
                }
                else if (_objects[x] is Player)
                {
                    Player player = _objects[x] as Player;
                    bool shadows = player.ShadowsOn;
                    player.Update(_map, ms / 1000f);
                    int damage = _attacks.CheckPlayer(player);
                    if (player.HP <= 0 && _server.Enabled)
                    {
                        KillPlayer(player);
                    }
                    if (player.ShadowsOn != shadows)
                    {
                        ShadowMessage sm = (ShadowMessage)Server.ProtocolHelper.NewMessage(MessageTypes.Shadow);
                        sm.shadows = player.ShadowsOn;
                        sm.Encode();
                        _server.SendClient(player, sm);
                    }
                }
            }
            _attacks.Update(_map, ms);
            //Protocol.FlushContainer(null);
        }

        public void SpawnAttack(int playerID,Vector2 dir, int attackID, int info)
        {
            Player player = GetPlayer(playerID);
            if (player != null)
            {
                if (player.CanAttack(attackID))
                {
                    /*
                    switch (attackID)
                    {
                        case 1:
                            Rectangle startRect = player.Rect;
                            startRect.Height -= 4;
                            startRect.Y += 3;
                            startRect.X += player.LookLeft ? -8 : 8;
                            _attacks.Spawn(3, startRect, new Vector2(player.LookLeft ? -200 : 200, 0), 100, playerID, getID()); 
                            player.Attack(0.3f);
                            break;
                    }
                     */
                    Attack a = player.GetAttack(attackID,dir, info);
                    if (a != null)
                        _attacks.Spawn(a);
                }
            }
        }

        public void KillPlayer(Player player)
        {
            if (player == null)
                return;
            if (!(player is NPC))
            {
                Point spawnPoint = _map.getRandomSpawn();
                Player newPlayer = new Player(_map, new Rectangle(spawnPoint.X, spawnPoint.Y, 16, 15), getID());
                newPlayer.Name = player.Name;
                newPlayer.SetTeam(player.GroupID);
                //Send client their new playerID
                AssignPlayerMessage apm = (AssignPlayerMessage)Server.ProtocolHelper.NewMessage(MessageTypes.AssignPlayer);
                apm.objectID = newPlayer.ID;
                apm.Encode();
                _server.SendClient(player, apm);
                //Add the new player to the game objects
                Add(newPlayer);
                newPlayer.Body = player.Body;
                newPlayer.Inventory = player.Inventory;
                for (int x = 0; x < player.Inventory.Count; x++)
                {
                    Body.BodyPart part = player.Inventory[x] as Body.BodyPart;
                    if(part==null)
                        continue;
                    PickupMessage pum = (PickupMessage)Server.ProtocolHelper.NewMessage(MessageTypes.Pickup);
                    pum.id = part.GetID();
                    pum.autoEquip = false;
                    pum.Encode();
                    _map.ObjectManager.Server.SendClient(player, pum);
                }
                //newPlayer.GetExp(player.TotalXP, Vector2.Zero);
                newPlayer.GetExp(0, Vector2.Zero);
                //Change the local clients stored playerid
                Client client = _server.getClient(player);
                if (client == null)
                    return;
                client.PlayerID = newPlayer.ID;

                newPlayer.Body.SetOwner(newPlayer);
                newPlayer.Body.SendBody(null);

                KillMessage km = (KillMessage)Server.ProtocolHelper.NewMessage(MessageTypes.Kill);
                km.killerID = player.Attacker;
                km.deadID = player.ID;
                km.Encode();
                player.Die();
                _server.SendAllMapReliable(_map, km, true);
                RemoveQuiet(player);
            }
            else
            {
                Remove(player);
                Player killer = GetPlayer(player.Attacker);
                if (killer != null)
                {
                    killer.GetExp((player as NPC).XPBonus, player.MidPosition);
                    Console.WriteLine("Giving " + (player as NPC).XPBonus.ToString() + " XP to " + killer.Name);
                }
            }
        }

        public int getID()
        {
            return nextID++;
        }

        public List<GameObject> Objects
        {
            get { return _objects; }
        }

        public IServer Server
        {
            get { return _server; }
        }

        public IMap Map
        {
            get { return _map; }
        }

        public void Remove(string parameters)
        {
            int id;
            if (int.TryParse(parameters, out id))
            {
                Remove(id);
            }
        }

        public int Players
        {
            get { return _playerCounter; }
        }

        public bool TimedOut
        {
            get { return _updateTimeout <= 0; }
        }

        public IAttackManager AttackManager
        {
            get { return _attacks; }
        }
    }
}
