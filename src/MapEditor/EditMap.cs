using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using IGORR.Content;
using IGORR.Modules;

namespace MapEditor
{
    public class TeleportPoint
    {
        public int mapID;
        public int X;
        public int Y;
    }

    public class TriggerParams
    {
        public string name;
        public bool global;
    }

    public class SpawnPoint
    {
        public int objectID;
        public int X;
        public int Y;
        public byte[] bytes;
    }

    class Tile
    {
        Texture2D _texture;
        Rectangle _destinationRect;
        Rectangle _sourceRect;

        public Tile(Texture2D texture, Rectangle dest, Rectangle source)
        {
            _texture = texture;
            _destinationRect = dest;
            _sourceRect = source;
        }

        public void SetSource(Rectangle src)
        {
            _sourceRect = src;
        }

        public void Draw(float depth, SpriteBatch batch, Color color)
        {
            batch.Draw(_texture, _destinationRect, _sourceRect, color, 0, Vector2.Zero, SpriteEffects.None, depth);
        }

        public int TileID
        {
            get { return _sourceRect.X / _sourceRect.Width; }
        }
    }

    class Map
    {
        Tile[][,] _layers;
        SpawnPoint[,] _spawns;
        const int tileSize = 16;
        Texture2D tileSet;
        string tileSetName;
        string _fileName;
        SpriteFont font;
        List<TeleportPoint> _teleportPoints = new List<TeleportPoint>();
        int activeLayer = 0;
        IGORR.Server.Logic.DummyMap _dummy;

        public void SetActiveLayer(int layer)
        {
            activeLayer = layer;
        }

        public Map(int sizeX, int sizeY, string tileset)
        {
            _dummy = new IGORR.Server.Logic.DummyMap();
            tileSet = ContentInterface.LoadTexture(tileset);
            font = ContentInterface.LoadFont("font");
            tileSetName = tileset;
            
            _layers = new Tile[3][,]; //0=background, 1=collisionlayer, 2=foreground
            for (int x = 0; x < 3; x++)
            {
                _layers[x] = new Tile[sizeX, sizeY];
            }
            _spawns = new SpawnPoint[sizeX, sizeY];
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    _layers[0][x, y] = new Tile(tileSet, new Rectangle(tileSize * x, tileSize * y, tileSize, tileSize), new Rectangle(0, 0, tileSize, tileSize));
                }
            }
        }

        public void SelectTileSet(string tileSet, ContentManager content)
        {
            try
            {
                this.tileSet = content.Load<Texture2D>(tileSet);
                tileSetName = tileSet;
            }
            catch (ContentLoadException e)
            {
                System.Windows.Forms.MessageBox.Show("Could not find asset: " + tileSet);
            }
        }

        public Map(string fileName)
        {
            _dummy = new IGORR.Server.Logic.DummyMap();
            font = ContentInterface.LoadFont("font");
            _fileName = fileName;
            BinaryReader reader = new BinaryReader(File.OpenRead(_fileName));
            int sizeX = reader.ReadInt32();
            int sizeY = reader.ReadInt32();
            tileSetName = reader.ReadString();
            tileSet = ContentInterface.LoadTexture(tileSetName);
            /*
int tpCount = reader.ReadInt32();
for (int x = 0; x < tpCount; x++)
{
    TeleportPoint point = new TeleportPoint();
    point.mapID = reader.ReadInt32();
    point.X = reader.ReadInt32();
    point.Y = reader.ReadInt32();
    _teleportPoints.Add(point);
}
 */
            _layers = new Tile[3][,];
            for (int layer = 0; layer < 3; layer++)
            {
                _layers[layer] = new Tile[sizeX, sizeY];
                for (int x = 0; x < sizeX; x++)
                    for (int y = 0; y < sizeY; y++)
                    {
                        int tileID=reader.ReadInt32();
                        if (tileID >= 0 && tileID * tileSize < tileSet.Width)
                            _layers[layer][x, y] = new Tile(tileSet, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), new Rectangle(tileID * tileSize, 0, tileSize, tileSize));
                        else
                            _layers[layer][x, y] = null;
                    }
            }
            _spawns = new SpawnPoint[sizeX, sizeY];
            try
            {
                for (int x = 0; x < sizeX; x++)
                    for (int y = 0; y < sizeY; y++)
                    {
                        int objectID = reader.ReadInt32();
                        if (objectID >= 0)
                        {
                            _spawns[x, y] = new SpawnPoint();
                            _spawns[x, y].objectID = objectID;
                            _spawns[x, y].X = x;
                            _spawns[x, y].Y = y;
                            if (objectID > 0)
                            {
                                ObjectControl ctrl =ModuleManager.GetControl(objectID, reader);
                                if (ctrl != null)
                                    _spawns[x, y].bytes = ctrl.GetObjectBytes();
                                //ModuleManager.SpawnByIdServer(_dummy, objectID, 0, Point.Zero, reader);
                            }

                        }
                        else
                            _spawns[x, y] = null;
                    }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error loading objects in the map, you can still continue though");
            }
            reader.Close();
            _teleportPoints.Clear();
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(_fileName))
            {
                System.Windows.Forms.SaveFileDialog diag = new System.Windows.Forms.SaveFileDialog();
                diag.AddExtension = true;
                diag.DefaultExt = "map";
                diag.ShowDialog();
                _fileName = diag.FileName;
            }
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(_fileName));
            writer.Write(_layers[0].GetLength(0));
            writer.Write(_layers[0].GetLength(1));
            writer.Write(tileSetName);
            /*
            writer.Write(_teleportPoints.Count);
            for (int x = 0; x < _teleportPoints.Count; x++)
            {
                writer.Write(_teleportPoints[x].mapID);
                writer.Write(_teleportPoints[x].X);
                writer.Write(_teleportPoints[x].Y);
            }
             */
            for(int layer=0; layer<3; layer++)
                for(int x=0; x<_layers[layer].GetLength(0); x++)
                    for (int y = 0; y < _layers[layer].GetLength(1); y++)
                    {
                        if (_layers[layer][x, y] != null)
                            writer.Write(_layers[layer][x, y].TileID);
                        else
                            writer.Write(-1);
                    }

            for (int x = 0; x < _layers[0].GetLength(0); x++)
                for (int y = 0; y < _layers[0].GetLength(1); y++)
                {
                    if (_spawns[x, y] != null)
                    {
                        writer.Write(_spawns[x, y].objectID);
                        if (_spawns[x, y].bytes != null)
                            writer.Write(_spawns[x, y].bytes);
                    }
                    else writer.Write(-1);
                }
            writer.Close();
        }

        public void SaveAs(string fileName)
        {
            _fileName = fileName;
            Save();
        }

        public void Draw(Camera cam, SpriteBatch batch)
        {
            Rectangle drawRect = cam.ViewSpace;
            int minX = drawRect.X / tileSize;
            int minY = drawRect.Y / tileSize;
            int maxX = (drawRect.Width + drawRect.X) / tileSize + 1;
            int maxY = (drawRect.Height + drawRect.Y) / tileSize + 1;
            for (int layer = 0; layer < 3; layer++)
                for (int x = minX; x < maxX; x++)
                    for (int y = minY; y < maxY; y++)
                        if (isValid(layer, x, y))
                        {
                            if (layer == activeLayer || activeLayer == 3)
                                _layers[layer][x, y].Draw(0.3f + layer / 5.0f, batch, Color.White);
                            else
                                _layers[layer][x, y].Draw(0.3f + layer / 5.0f, batch, Color.Gray);
                        }
            if (activeLayer == 3)
            {
                for (int x = minX; x < maxX; x++)
                    for (int y = minY; y < maxY; y++)
                    {
                        if (isValidOrNull(0, x, y) && _spawns[x, y] != null && _spawns[x, y].objectID != -1)
                            batch.DrawString(font, _spawns[x, y].objectID.ToString(), new Vector2(tileSize * x, tileSize * y), Color.Red, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.6f);
                    }
            }
        }

        public void ChangeTile(int layer, float worldX, float worldY, int tileNum)
        {
            int x = (int)(worldX / tileSize);
            int y = (int)(worldY / tileSize);
            if (isValidOrNull(layer, x, y))
                if (tileNum == -1)
                {
                    _layers[layer][x, y] = null;
                }
                else _layers[layer][x, y] = new Tile(tileSet, new Rectangle(tileSize * x, tileSize * y, tileSize, tileSize), new Rectangle(tileNum * tileSize, 0, tileSize, tileSize));
        }

        public void SetObject(float worldX, float worldY)
        {
            int x = (int)(worldX / tileSize);
            int y = (int)(worldY / tileSize);
            if (isValidOrNull(0, x, y))
            {
				frmObjectDialog od = new frmObjectDialog (x, y, _spawns[x, y]);
                if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    _spawns[x, y] = od.SpawnPoint;
            }
        }

        private bool isValid(int layer,int x, int y)
        {
            return layer >= 0 && layer < _layers.Length && x >= 0 && x < _layers[layer].GetLength(0) && y >= 0 && y < _layers[layer].GetLength(1) && _layers[layer][x, y] != null;
        }

        private bool isValidOrNull(int layer, int x, int y)
        {
            return layer >= 0 && layer < _layers.Length && x >= 0 && x < _layers[layer].GetLength(0) && y >= 0 && y < _layers[layer].GetLength(1);
        }

        public string TileSet
        {
            get { return tileSetName; }
        }
    }
}
