using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace IGORR_Server.Logic
{
    class SpawnCountdown
    {
        public float countDown;
        public int id;
    }

    struct TeleportPoint
    {
        public int mapID;
        public int X;
        public int Y;
    }

    public class TileModification
    {
        public int layer;
        public Point Position;
        public int TileID;
    }

    public class Map
    {
        Tile[][,] _layers;
        Texture2D tileSet;
        const int tileSize = 16;
        int[,] _spawnIDs;
        Dictionary<int, List<Point>> _spawnPoints;
        List<TeleportPoint> _teleportPoints;
        List<SpawnCountdown> _spawns;
        List<TileModification> _tileMods = new List<TileModification>();
        Dictionary<string,bool> _triggers = new Dictionary<string, bool>();
        ObjectManager manager;

        Random random;
        ContentManager _content;

        string _mapname;
        int _id;

        public Map(string fileName, Server server, int id)
        {
            random = new Random();

            _mapname = fileName;
            _id = id;

            _spawns = new List<SpawnCountdown>();
            manager = new IGORR_Server.ObjectManager(server);
            manager.SetMap(this);
            LoadNew(fileName);
            /*
            StreamReader reader = new StreamReader(fileName+".txt");
            string fileContent = reader.ReadToEnd();
            reader.Close();

            //_spawnPoints = new Dictionary<int, List<Point>>();
            string[] lines = fileContent.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int sizeX = int.Parse(lines[0]);
            int sizeY = int.Parse(lines[1]);
            string[] teleportPoints = lines[3].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            List<TeleportPoint> tpoints = new List<TeleportPoint>();
            for (int x = 0; x+2 < teleportPoints.Length; x+=3)
            {
                TeleportPoint newPoint = new TeleportPoint();
                newPoint.mapID = int.Parse(teleportPoints[x]);
                newPoint.X = int.Parse(teleportPoints[x+1]);
                newPoint.Y = int.Parse(teleportPoints[x+2]);
                tpoints.Add(newPoint);
            }
            _layers = new Tile[3][sizeX, sizeY];
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    int tileID = lines[4 + y][x] - 'a';
                    _t[x, y] = new Tile(this,new Rectangle(tileID * tileSize, 0, tileSize, tileSize), new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), tileID > 1);
                }
            }
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    int powerID = lines[4 + y + sizeY][x] - 'a';
                    switch (powerID)
                    {
                        case ('t' - 'a'):
                            Lava lava = new Lava(this, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), ObjectManager.getID());                            _tiles[x, y].SetChild(lava);
                            ObjectManager.Add(lava); break;
                        case ('f'-'a'):
                            BossBlobTrigger trigger = new Logic.BossBlobTrigger(this,new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), ObjectManager.getID());
                            ObjectManager.Add(trigger);
                            _tiles[x, y].SetChild(trigger);
                            break;
                        case ('g' - 'a'):
                            if (tpoints.Count > 0)
                            {
                                MapChanger mc = new MapChanger(this, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), ObjectManager.getID());
                                mc.targetMapID = tpoints[0].mapID;
                                mc.targetPos = new Vector2(tpoints[0].X, tpoints[0].Y);
                                Console.WriteLine("Added Teleporter");
                                tpoints.RemoveAt(0);
                                ObjectManager.Add(mc);
                                _tiles[x, y].SetChild(mc);
                            }
                            break;
                        default:
                            if (powerID >= 0 && powerID <= 's' - 'a')
                            {
                                /*
                                if (!_spawnPoints.ContainsKey(powerID))
                                    _spawnPoints.Add(powerID, new List<Point>());
                                _spawnPoints[powerID].Add(new Point(x * tileSize, y * tileSize));
                                 
                            }   
                            break;
                    }

                }
            }
            if(4 + 2 * sizeY<lines.Length)
                for (int x = 0; x < lines[4 + 2 * sizeY].Length; x++)
                {
                    SpawnItem(lines[4 + 2 * sizeY][x] - 'a');
                }
            //SpawnItem(1);
            //SpawnItem(1);
            //SpawnItem(3);
            //SpawnItem(3);
            //SpawnItem(4);
            */
        }

        public void LoadNew(string file)
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(file+".map"));
            int sizeX = reader.ReadInt32();
            int sizeY = reader.ReadInt32();
            reader.ReadString();
            /*
int tpCount = reader.ReadInt32();
_teleportPoints = new List<TeleportPoint>();
for (int x = 0; x < tpCount; x++)
{
    TeleportPoint point = new TeleportPoint();
    point.mapID = reader.ReadInt32();
    point.X = reader.ReadInt32();
    point.Y = reader.ReadInt32();
    _teleportPoints.Add(point);
}
 */
            _layers = new Tile[4][,];
            for (int layer = 0; layer < 4; layer++)
            {
                int count = 0;
                _layers[layer] = new Tile[sizeX, sizeY];
                for (int x = 0; x < sizeX; x++)
                    for (int y = 0; y < sizeY; y++)
                    {
                        if (layer == 3)
                        {
                            _layers[layer][x, y] = new Tile(this, new Rectangle(0, 0, tileSize, tileSize), new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), false);
                        }
                        else
                        {
                            int tileID = reader.ReadInt32();
                            if (tileID >= 0)
                            {
                                _layers[layer][x, y] = new Tile(this, new Rectangle(0, 0, tileSize, tileSize), new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), layer == 1);
                                count++;
                            }
                            else
                                _layers[layer][x, y] = null;
                        }
                    }
                Console.WriteLine(count.ToString() + " tiles in layer " + layer.ToString());
            }
            _spawnIDs = new int[sizeX, sizeY];
            _spawnPoints = new Dictionary<int, List<Point>>();
            try
            {
                for (int x = 0; x < sizeX; x++)
                    for (int y = 0; y < sizeY; y++)
                    {
                        int objectID = reader.ReadInt32();
                        if (objectID >= 0)
                        {
                            if (objectID >= 5000)
                            {
                                manager.SpawnMob((objectID-5000).ToString() + " " + (x * tileSize).ToString() + " " + (y * tileSize).ToString());
                            }
                            else
                            {
                                _spawnIDs[x, y] = objectID;
                                if (!_spawnPoints.ContainsKey(objectID))
                                    _spawnPoints.Add(objectID, new List<Point>());
                                _spawnPoints[objectID].Add(new Point(x * tileSize, y * tileSize));
                                SpawnItem(objectID, reader);
                            }
                        }
                        else
                            _spawnIDs[x, y] = -1;
                    }
            }
            catch (Exception e)
            {
            }
            reader.Close();
        }

        public Point getRandomSpawn()
        {
            if (_spawnPoints.ContainsKey('s' - 'a'))
                return _spawnPoints['s' - 'a'][random.Next(0, _spawnPoints['s'-'a'].Count)];
            else return Point.Zero;
        }


        public void SpawnItem(int id, BinaryReader reader)
        {
            if (_spawnPoints.ContainsKey(id))
            {
                Tile targetTile = null;
                Point p = new Point(0, 0);
                int countdown = 100;
                while ((targetTile == null || targetTile.EventObject != null) && countdown>0)
                {
                    p = _spawnPoints[id][random.Next(0, _spawnPoints[id].Count)];
                    targetTile = _layers[3][p.X / tileSize, p.Y / tileSize];
                    countdown--;
                }
                GameObject newObject = null;
                switch (id)
                {
                    case 'b' - 'a': newObject = new PartPickup(new Legs(), this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID(),true); break;
                    case 'd' - 'a': newObject = new PartPickup(new Striker(), this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID(),true); break;
                    case 'e' - 'a': newObject = new PartPickup(new Wings(), this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID(),true); break;
                    case 't' - 'a': newObject = new Lava(this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 'f' - 'a': newObject = new BossBlobTrigger(this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 'g' - 'a': if (reader == null) return;MapChanger mc = new MapChanger(this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); mc.targetMapID = reader.ReadInt32(); mc.targetPos = new Vector2(reader.ReadInt32(), reader.ReadInt32()); newObject = mc; break;
                    case 'i' - 'a': if (reader == null) return; newObject = new TouchTrigger(reader.ReadString(), reader.ReadBoolean(), this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 9: newObject = new PartPickup(new Striker2(), this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID(), true); break;
                    case 10: if (reader == null) return; newObject = new TurnOffBlocker(reader.ReadString(), reader.ReadBoolean(), this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 11: if (reader == null) return; newObject = new AttackTrigger(reader.ReadString(), reader.ReadBoolean(), this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 12: if (reader == null) return; newObject = new TurnOnBlocker(reader.ReadString(), reader.ReadBoolean(), this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 13: newObject = new ShadowOn(this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 14: newObject = new ShadowOff(this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 15: newObject = new HPOrb(5,this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 16: newObject = new HPOrb(10, this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 17: newObject = new HPOrb(20, this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 20: newObject = new ExpOrb(6, this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 22: newObject = new ScorePost(ScorePost.ScoreColor.Red, this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 23: newObject = new ScorePost(ScorePost.ScoreColor.Blue, this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 29: newObject = new ScorePost(ScorePost.ScoreColor.Neutral, this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    case 26: if (reader == null) return; RedTeamTeleporter rtt = new RedTeamTeleporter(this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); rtt.targetMapID = reader.ReadInt32(); rtt.targetPos = new Vector2(reader.ReadInt32(), reader.ReadInt32()); newObject = rtt; break;
                    case 27: if (reader == null) return; BlueTeamTeleporter btt = new BlueTeamTeleporter(this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); btt.targetMapID = reader.ReadInt32(); btt.targetPos = new Vector2(reader.ReadInt32(), reader.ReadInt32()); newObject = btt; break;
                    case 28: if (reader == null) return; PvETeleporter pvet = new PvETeleporter(this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); pvet.targetMapID = reader.ReadInt32(); pvet.targetPos = new Vector2(reader.ReadInt32(), reader.ReadInt32()); newObject = pvet; break;
                    case 100: newObject = new Blocker(this, new Rectangle(p.X, p.Y, tileSize, tileSize), ObjectManager.getID()); break;
                    //case 'c' - 'a': targetTile.SetChild(new Exit(_content.Load<Texture2D>("Exit"), this, new Rectangle(p.X, p.Y, tileSize, tileSize))); break;
                }
                if (newObject != null && targetTile!=null)
                {
                    if (newObject is EventObject)
                        targetTile.SetChild(newObject as EventObject);
                    Console.WriteLine("Spawned new object: " + newObject.ToString());
                    //ChangeTile(0, new Vector2(p.X, p.Y), 25);
                    manager.Add(newObject);
                }
            }
        }

        public void SpawnItem(PartPickup pickup)
        {
            if (pickup == null)
                return;
            if (!isValid((int)(pickup.MidPosition.X / tileSize), (int)(pickup.MidPosition.Y / tileSize)))
                return;
            Tile targetTile = _layers[3][(int)(pickup.MidPosition.X / tileSize), (int)(pickup.MidPosition.Y / tileSize)];
            targetTile.SetChild(pickup);
            if (pickup.Respawn && (!_spawnPoints.ContainsKey(pickup.ObjectType) || !_spawnPoints[pickup.ObjectType].Contains(new Point((int)(pickup.MidPosition.X / tileSize), (int)(pickup.MidPosition.Y / tileSize)))))
            {
                if (!_spawnPoints.ContainsKey(pickup.ObjectType))
                    _spawnPoints.Add(pickup.ObjectType, new List<Point>());
                _spawnPoints[pickup.ObjectType].Add(new Point((int)(pickup.MidPosition.X), (int)(pickup.MidPosition.Y)));
            }
            Console.WriteLine("Spawned new object: " + pickup.Name);
            manager.Add(pickup);
        }

        public void TimeSpawn(int id, float countdown)
        {
            SpawnCountdown cd = new SpawnCountdown();
            cd.countDown = countdown;
            cd.id = id;
            _spawns.Add(cd);
        }
         


        public void Update(float ms)
        {
            manager.Update(ms);
            for (int x = 0; x < _layers[3].GetLength(0); x++)
                for (int y = 0; y < _layers[3].GetLength(1); y++)
                    if (isValid(x, y))
                        if (_layers[3][x, y].EventObject != null) 
                            _layers[3][x, y].EventObject.Update(ms);

            for (int x = 0; x < _spawns.Count; x++)
            {
                _spawns[x].countDown -= ms;
                if (_spawns[x].countDown < 0)
                {
                    SpawnItem(_spawns[x].id, null);
                    _spawns.RemoveAt(x);
                    x--;
                }
            }
        }

        public bool Collides(GameObject obj)
        {
            int posX=(int)(obj.MidPosition.X/tileSize);
            int posY=(int)(obj.MidPosition.Y/tileSize);
            int sizeX = (obj.Rect.Width / tileSize)/2+1;
            int sizeY = (obj.Rect.Height / tileSize)/2+1;
            bool result = false;

            for (int x = -sizeX; x <= sizeX; x++)
                for (int y = -sizeY; y <= sizeY; y++)
                    if (isValid(posX + x, posY + y) && _layers[1][posX + x, posY + y] != null)
                        result = (result || _layers[1][posX + x, posY + y].Collides(obj));

            return result;
        }
        /*
        public EventObject GetEvent(GameObject obj)
        {

            if (obj is NPC)
                return null;
            int posX=(int)(obj.MidPosition.X/tileSize);
            int posY=(int)(obj.MidPosition.Y/tileSize);

            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    if (isValid(posX + x, posY + y))
                        if (_layers[3][posX + x, posY + y].EventObject!=null && _layers[3][posX + x, posY + y].EventObject.Collides(obj))
                            return _layers[3][posX + x, posY + y].EventObject;
            return null;
        }
         */

        public void DoPlayerEvents(Player player)
        {
            if (player is NPC)
                return ;
            int posX = (int)(player.MidPosition.X / tileSize);
            int posY = (int)(player.MidPosition.Y / tileSize);

            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    if (isValid(posX + x, posY + y))
                        if (_layers[3][posX + x, posY + y].EventObject != null && _layers[3][posX + x, posY + y].EventObject.Collides(player))
                            _layers[3][posX + x, posY + y].EventObject.Event(player);
        }

        public void AddObject(EventObject obj)
        {
            int posX = (int)(obj.MidPosition.X / tileSize);
            int posY = (int)(obj.MidPosition.Y / tileSize);
            if (isValid(posX, posY) && _layers[3][posX, posY].EventObject == null)
                _layers[3][posX, posY].SetChild(obj);
        }

        public void ChangeTile(int layer, Vector2 position, int tileID)
        {
            int posX = (int)(position.X / tileSize);
            int posY = (int)(position.Y / tileSize);
            if (isValid(posX, posY) && layer >= 0 && layer < 3)
            {
                Tile newTile = new Tile(this, new Rectangle(tileID * tileSize, 0, tileSize, tileSize), new Rectangle(posX * tileSize, posY * tileSize, tileSize, tileSize), layer == 1);
                if (tileID == -1)
                    _layers[layer][posX, posY] = null;
                else
                    _layers[layer][posX, posY] = newTile;

                IGORR.Protocol.Messages.ChangeTileMessage ctm = (IGORR.Protocol.Messages.ChangeTileMessage)IGORR.Protocol.ProtocolHelper.NewMessage(IGORR.Protocol.MessageTypes.ChangeTile);
                ctm.tileID = tileID;
                ctm.x = posX;
                ctm.y = posY;
                ctm.layer = layer;
                ctm.Encode();
                manager.Server.SendAllMapReliable(this, ctm, true);

                TileModification tmod = new TileModification();
                tmod.layer=layer;
                tmod.Position=new Point(posX,posY);
                tmod.TileID=tileID;
                for (int x = 0; x < _tileMods.Count; x++)
                    if (_tileMods[x].Position == tmod.Position && _tileMods[x].layer == tmod.layer)
                    {
                        _tileMods[x].TileID = tileID;
                        return;
                    }
                _tileMods.Add(tmod);
            }
        }

        public bool GetTrigger(string triggerName)
        {
            if (!_triggers.ContainsKey(triggerName))
            {
                _triggers.Add(triggerName, false);
            }
            return _triggers[triggerName];
        }

        public void SetTrigger(string triggerName, bool val)
        {
            if (!_triggers.ContainsKey(triggerName))
            {
                _triggers.Add(triggerName, val);
            }
            else
                _triggers[triggerName] = val;
        }

        public Rectangle MapBoundaries
        {
            get { return new Rectangle(0, 0, _layers[0].GetLength(0) * tileSize, _layers[0].GetLength(1) * tileSize); }
        }

        private bool isValid(int x, int y)
        {
            return x >= 0 && x < _layers[0].GetLength(0) && y >= 0 && y < _layers[0].GetLength(1);
        }

        public ObjectManager ObjectManager
        {
            get { return manager; }
        }

        public string MapName
        {
            get { return _mapname; }
        }

        public int Players
        {
            get { return manager.Players; }
        }

        public bool TimedOut
        {
            get { return manager.TimedOut; }
        }

        public List<TileModification> TileMods
        {
            get { return _tileMods; }
        }

        public int ID
        {
            get { return _id; }
        }
    }
}
