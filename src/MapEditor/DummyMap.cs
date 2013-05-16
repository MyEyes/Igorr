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
    class SpawnCountdown
    {
        public float countDown;
        public int id;
        public Vector2 position;
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

    public class DummyMap:IMap
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
        List<IGORR.Modules.ObjectModule> _modules;

        Random random;
        ContentManager _content;

        string _mapname;
        int _id;

        public DummyMap()
        {
            random = new Random();
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
            Console.WriteLine("Loading Map " + file);
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
                            _spawnIDs[x, y] = objectID;
                            if (!_spawnPoints.ContainsKey(objectID))
                                _spawnPoints.Add(objectID, new List<Point>());
                            _spawnPoints[objectID].Add(new Point(x * tileSize, y * tileSize));
                            SpawnItem(objectID,new Vector2(x*tileSize,y*tileSize), reader);

                        }
                        else
                            _spawnIDs[x, y] = -1;
                    }
            }
            catch (Exception e)
            {
            }
            reader.Close();
            Console.WriteLine();
        }

        public Point getRandomSpawn()
        {
            return Point.Zero;
        }

        public void SpawnItem(int typeId, Vector2 position, BinaryReader reader)
        {
        }

        public void SpawnItem(int id, BinaryReader reader)
        {
        }

        public void TimeSpawn(int id, Vector2 position, float countdown)
        {
        }
         


        public void Update(float ms)
        {
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

        public void FloodFill(int layer, Vector2 position, int tileID)
        {
            int posX = (int)(position.X / tileSize);
            int posY = (int)(position.Y / tileSize);
            if (isValid(posX, posY) && layer >= 0 && layer < 3)
            {
                int previousTileID = _layers[layer][posX, posY].TileID;
                if (previousTileID == tileID)
                    return;
                List<Point> _toDO = new List<Point>();
                List<Point> _done = new List<Point>();
                _toDO.Add(new Point(posX, posY));
                while (_toDO.Count > 0)
                {
                    Point cur = _toDO[0];
                    cur.X += 1;
                    if (isValid(cur.X, cur.Y) && !_done.Contains(new Point(cur.X, cur.Y)) && !_toDO.Contains(new Point(cur.X, cur.Y)) && _layers[layer][cur.X,cur.Y].TileID==previousTileID)
                        _toDO.Add(new Point(cur.X, cur.Y));
                    cur.X -= 2;
                    if (isValid(cur.X, cur.Y) && !_done.Contains(new Point(cur.X, cur.Y)) && !_toDO.Contains(new Point(cur.X, cur.Y)) && _layers[layer][cur.X, cur.Y].TileID == previousTileID)
                        _toDO.Add(new Point(cur.X, cur.Y));
                    cur.X += 1;
                    cur.Y += 1;
                    if (isValid(cur.X, cur.Y) && !_done.Contains(new Point(cur.X, cur.Y)) && !_toDO.Contains(new Point(cur.X, cur.Y)) && _layers[layer][cur.X, cur.Y].TileID == previousTileID)
                        _toDO.Add(new Point(cur.X, cur.Y));
                    cur.Y -= 2;
                    if (isValid(cur.X, cur.Y) && !_done.Contains(new Point(cur.X, cur.Y)) && !_toDO.Contains(new Point(cur.X, cur.Y)) && _layers[layer][cur.X, cur.Y].TileID == previousTileID)
                        _toDO.Add(new Point(cur.X, cur.Y));
                    ChangeTile(layer, 16 * new Vector2(cur.X, cur.Y), tileID);
                    cur.Y += 1;
                    _done.Add(cur);
                }
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
            return false;
        }

        public IObjectManager ObjectManager
        {
            get { return null; }
        }

        public string MapName
        {
            get { return _mapname; }
        }

        public int Players
        {
            get { return 0; }
        }

        public bool TimedOut
        {
            get { return true; }
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
