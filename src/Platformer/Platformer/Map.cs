using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using IGORR.Content;
using IGORR.Client.Logic;
namespace IGORR.Client
{

    public class Map:IMap
    {
        Semaphore _sem = new Semaphore(1,1);
        Tile[][,] _layers;
        //Tile[,] _tiles;
        EventObject[,] _events;
        Texture2D tileSet;
        const int tileSize = 16;

        Random random;

        public Map(string fileName, GraphicsDevice device)
        {
            random = new Random();
            int maxX = 30;
            int maxY = 30;
            
            LoadNew(fileName);
            /*
            #if WINDOWS
            StreamReader reader = new StreamReader(fileName+".txt");
            string fileContent = reader.ReadToEnd();
            reader.Close();
            #elif XBOX
             */
            /*
            string fileContent = content.Load<string>(fileName);
            //#endif

            _spawnPoints = new Dictionary<int, List<Point>>();
            string[] lines = fileContent.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int sizeX = int.Parse(lines[0]);
            int sizeY = int.Parse(lines[1]);
            _tiles = new Tile[sizeX, sizeY];
            tileSet = content.Load<Texture2D>(lines[2]);
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    int tileID = lines[4 + y][x] - 'a';
                    _tiles[x, y] = new Tile(new Rectangle(tileID * tileSize, 0, tileSize, tileSize), new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), tileSet, tileID > 1);
                }
            }
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    int powerID = lines[4 + y + sizeY][x] - 'a';
                    switch (powerID)
                    {
                        //case ('t' - 'a'): _tiles[x, y].SetChild(new Lava(this, content.Load<Texture2D>("Lava"), new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize))); break;
                        default:
                            if (powerID >= 0 && powerID <= 's' - 'a')
                            {
                                if (!_spawnPoints.ContainsKey(powerID))
                                    _spawnPoints.Add(powerID, new List<Point>());
                                _spawnPoints[powerID].Add(new Point(x * tileSize, y * tileSize));
                            }   
                            break;
                    }

                }
            }
             */
            //SpawnItem(1);
            //SpawnItem(2);
        }

        public void LoadNew(string file)
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(file + ".map"));
            int sizeX = reader.ReadInt32();
            int sizeY = reader.ReadInt32();
            tileSet = ContentInterface.LoadTexture(reader.ReadString());
            /*
            int tpCount = reader.ReadInt32();
            for (int x = 0; x < tpCount; x++)
            {
                reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadInt32();
            }
             */
            _layers = new Tile[3][,];
            for (int layer = 0; layer < 3; layer++)
            {
                int count = 0;
                _layers[layer] = new Tile[sizeX, sizeY];
                for (int x = 0; x < sizeX; x++)
                    for (int y = 0; y < sizeY; y++)
                    {
                            int tileID = reader.ReadInt32();
                            if (tileID >= 0)
                            {
                                _layers[layer][x, y] = new Tile(new Rectangle(tileID * tileSize, 0, tileSize, tileSize), new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), tileSet, layer == 1);
                                count++;
                            }
                            else
                                _layers[layer][x, y] = null;
                    }
                Console.WriteLine(count.ToString() + " tiles in layer " + layer.ToString());
            }
            _events = new EventObject[sizeX, sizeY];
            reader.Close();
        }

        /*
        public void SpawnItem(int id)
        {
            if (_spawnPoints.ContainsKey(id))
            {
                Point p;
                p = _spawnPoints[id][random.Next(0, _spawnPoints[id].Count)];
                Tile targetTile = _tiles[p.X / tileSize, p.Y / tileSize];
                switch (id)
                {
                    case 'b' - 'a':  targetTile.SetChild(new PartPickup(new Legs(_content.Load<Texture2D>("FeetIcon")), this, new Rectangle(p.X, p.Y, tileSize, tileSize))); break;
                    case 'c' - 'a': targetTile.SetChild(new Exit(_content.Load<Texture2D>("Exit"), this, new Rectangle(p.X, p.Y, tileSize, tileSize))); break;
                }
            }
        }

        public void TimeSpawn(int id, float countdown)
        {
            SpawnCountdown cd = new SpawnCountdown();
            cd.countDown = countdown;
            cd.id = id;
            _spawns.Add(cd);
        }
         */

        public void Draw(SpriteBatch batch, Camera cam)
        {
            Rectangle drawRect = cam.ViewSpace;
            int minX = drawRect.X / tileSize;
            int minY = drawRect.Y / tileSize;
            int maxX = (drawRect.Width + drawRect.X) / tileSize + 1;
            int maxY = (drawRect.Height + drawRect.Y) / tileSize + 1;
            for (int layer = 0; layer < 3; layer++)
                for (int x = minX; x < maxX; x++)
                    for (int y = minY; y < maxY; y++)
                        if (isValid(x, y) && _layers[layer][x, y] != null)
                        {
                            _layers[layer][x, y].Draw(0.6f - layer / 10.0f, batch);
                        }
            
                for (int x = minX; x < maxX; x++)
                    for (int y = minY; y < maxY; y++)
                    {
                        if (isValid(x, y) && _events[x, y] != null)
                        _events[x, y].Draw(batch);
                    }
        }

        public bool hasDirectLineToPosition(int x, int y, Vector2 position)
        {
            Vector2 center = new Vector2((x + 0.5f) * tileSize, (y + 0.5f) * tileSize);
            Vector2 dirToPos = position - center;
            float steps = dirToPos.X!=0?dirToPos.X:dirToPos.Y;
            if (dirToPos.X > dirToPos.Y)
                dirToPos *= 0.5f*tileSize / dirToPos.X;
            else
                dirToPos *= 0.5f*tileSize / dirToPos.Y;
            steps = steps / (dirToPos.X != 0 ? dirToPos.X : dirToPos.Y);
            Vector2 pos = center+dirToPos;
            for (int z = 0; z < steps-2; z++)
            {
                pos+=dirToPos;
                x=(int)(pos.X/tileSize);
                y=(int)(pos.Y/tileSize);
                if (isValid(x, y) && _layers[1][x, y]!=null)
                    return false;
            }
            return true;
        }

        

        public void Update(float ms, Camera cam)
        {
            Rectangle drawRect = cam.ViewSpace;
            int minX = drawRect.X / tileSize;
            int minY = drawRect.Y / tileSize;
            int maxX = (drawRect.Width + drawRect.X) / tileSize + 1;
            int maxY = (drawRect.Height + drawRect.Y) / tileSize + 1;
            for (int x = minX; x < maxX; x++)
                for (int y = minY; y < maxY; y++)
                    if (isValid(x, y))
                         if (_events[x, y] != null) _events[x, y].Update(ms);
        }

        public bool Collides(GameObject obj)
        {
            int posX = (int)(obj.MidPosition.X / tileSize);
            int posY = (int)(obj.MidPosition.Y / tileSize);
            int sizeX = (obj.Rect.Width / tileSize) / 2 + 1;
            int sizeY = (obj.Rect.Height / tileSize) / 2 + 1;
            bool result = false;

            for (int x = -sizeX; x <= sizeX; x++)
                for (int y = -sizeY; y <= sizeY; y++)
                    if (isValid(posX + x, posY + y) && _layers[1][posX + x, posY + y]!=null)
                        result = result || _layers[1][posX + x, posY + y].Collides(obj);

            return result;
        }

        public bool Collides(Vector2 position)
        {
            int posX = (int)(position.X / tileSize);
            int posY = (int)(position.Y / tileSize);
            if (isValid(posX, posY) && _layers[1][posX, posY] != null)
                return true;

            return false;
        }

        public int CreateShadowGeometry(Vector2 pos, Rectangle viewSpace, VertexBuffer shadowVB)
        {
            Vector2 center = pos;
            Rectangle drawRect = viewSpace;
            int minX = (int)(pos.X / tileSize)-drawRect.Width/2;
            minX = minX >= 0 ? minX : 0;
            int minY = (int)(pos.Y / tileSize)-drawRect.Height/2;
            minY = minY >= 0 ? minY : 0;
            int maxX = (drawRect.Width + drawRect.X) / tileSize + 1;
            maxX = maxX < _layers[1].GetLength(0) ? maxX : _layers[1].GetLength(0);
            int maxY = (drawRect.Height + drawRect.Y) / tileSize + 1;
            maxY = maxY < _layers[1].GetLength(1) ? maxY : _layers[1].GetLength(1);
            int vertexCount = 0;
            for (int x = minX; x < maxX; x++)
                for (int y = minY; y < maxY; y++)
                    if (_layers[1][x, y] != null)
                    {
                        Vector3 diff = new Vector3(_layers[1][x, y].MidPosition - center, 0);
                        Vector3 edge1, edge2, edge3, edge4;
                        if (diff.X > diff.Y)
                        {
                            if (diff.X > -diff.Y)
                            {

                                edge1 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(-tileSize / 2, -tileSize / 2), 0);//1
                                edge2 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(tileSize / 2, -tileSize / 2), 0);//2
                                edge3 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(tileSize / 2, tileSize / 2), 0);//3
                                edge4 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(-tileSize / 2, tileSize / 2), 0);//4
                            }
                            else
                            {
                                edge1 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(-tileSize / 2, tileSize / 2), 0);//4
                                edge2 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(-tileSize / 2, -tileSize / 2), 0);//1
                                edge3 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(tileSize / 2, -tileSize / 2), 0);//2
                                edge4 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(tileSize / 2, tileSize / 2), 0);//3
                            }
                        }
                        else
                        {
                            if (diff.X > -diff.Y)
                            {
                                edge1 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(tileSize / 2, -tileSize / 2), 0);//2
                                edge2 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(tileSize / 2, tileSize / 2), 0);//3
                                edge3 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(-tileSize / 2, tileSize / 2), 0);//4
                                edge4 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(-tileSize / 2, -tileSize / 2), 0);//1
                            }
                            else
                            {
                                edge1 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(tileSize / 2, tileSize / 2), 0);//3
                                edge2 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(-tileSize / 2, tileSize / 2), 0);//4
                                edge3 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(-tileSize / 2, -tileSize / 2), 0);//1
                                edge4 = new Vector3(_layers[1][x, y].MidPosition + new Vector2(tileSize / 2, -tileSize / 2), 0);//2
                            }
                        }
                        Vector3 dir1;
                        Vector3 dir2;
                        float stretch = 300;

                        if (vertexCount < shadowVB.VertexCount && Vector3.Dot((edge1 + edge2) / 2 - new Vector3(center, 0), new Vector3(-(edge2 - edge1).Y, (edge2 - edge1).X, 0)) < 0)
                        {
                            dir1 = edge1 - new Vector3(center, 0);
                            dir1.Normalize();
                            dir2 = edge2 - new Vector3(center, 0);
                            dir2.Normalize();
                            shadowVB.SetData<VertexPositionColor>(vertexCount * VertexPositionColor.VertexDeclaration.VertexStride, new VertexPositionColor[]{
                            new VertexPositionColor(edge1+dir1*stretch,Color.White),
                            new VertexPositionColor(edge1+Vector3.Transform(dir1,LightMap.rotateRight)*stretch, Color.Black),
                            new VertexPositionColor(edge2+Vector3.Transform(dir2,LightMap.rotateLeft)*stretch, Color.Black),
                            new VertexPositionColor(edge2+dir2*stretch,Color.White),
                            new VertexPositionColor(edge1,Color.Black),
                            new VertexPositionColor(edge2,Color.Black)},
                            0, 6, VertexPositionColor.VertexDeclaration.VertexStride);
                            vertexCount += 6;
                        }

                        if (vertexCount < shadowVB.VertexCount)// && Vector3.Dot((edge2+edge3)/2 - new Vector3(center, 0), new Vector3(-(edge3 - edge2).Y, (edge3 - edge2).X, 0)) < 0)
                        {
                            dir1 = edge2 - new Vector3(center, 0);
                            dir1.Normalize();
                            dir2 = edge3 - new Vector3(center, 0);
                            dir2.Normalize();
                            shadowVB.SetData<VertexPositionColor>(vertexCount * VertexPositionColor.VertexDeclaration.VertexStride, new VertexPositionColor[]{
                            new VertexPositionColor(edge2+dir1*stretch,Color.White),
                            new VertexPositionColor(edge2+Vector3.Transform(dir1,LightMap.rotateRight)*stretch, Color.Black),
                            new VertexPositionColor(edge3+Vector3.Transform(dir2,LightMap.rotateLeft)*stretch, Color.Black),
                            new VertexPositionColor(edge3+dir2*stretch,Color.White),
                            new VertexPositionColor(edge2,Color.Black),
                            new VertexPositionColor(edge3,Color.Black)},
                            0, 6, VertexPositionColor.VertexDeclaration.VertexStride);
                            vertexCount += 6;
                        }

                        if (vertexCount < shadowVB.VertexCount && Vector3.Dot((edge3 + edge4) / 2 - new Vector3(center, 0), new Vector3(-(edge4 - edge3).Y, (edge4 - edge3).X, 0)) < 0)
                        {
                            dir1 = edge3 - new Vector3(center, 0);
                            dir1.Normalize();
                            dir2 = edge4 - new Vector3(center, 0);
                            dir2.Normalize();
                            shadowVB.SetData<VertexPositionColor>(vertexCount * VertexPositionColor.VertexDeclaration.VertexStride, new VertexPositionColor[]{
                            new VertexPositionColor(edge3+dir1*stretch,Color.White),
                            new VertexPositionColor(edge3+Vector3.Transform(dir1,LightMap.rotateRight)*stretch, Color.Black),
                            new VertexPositionColor(edge4+Vector3.Transform(dir2,LightMap.rotateLeft)*stretch, Color.Black),
                            new VertexPositionColor(edge4+dir2*stretch,Color.White),
                            new VertexPositionColor(edge3,Color.Black),
                            new VertexPositionColor(edge4,Color.Black),
                            },
                            0, 6, VertexPositionColor.VertexDeclaration.VertexStride);
                            vertexCount += 6;
                        }


                    }
            return vertexCount;
        }

        public void DrawTileHighlight(Camera cam, SpriteBatch batch)
        {
            Rectangle drawRect = cam.ViewSpace;
            int minX = drawRect.X / tileSize;
            int minY = drawRect.Y / tileSize;
            int maxX = (drawRect.Width + drawRect.X) / tileSize + 1;
            int maxY = (drawRect.Height + drawRect.Y) / tileSize + 1;
            for (int x = minX; x < maxX; x++)
                for (int y = minY; y < maxY; y++)
                    if (isValid(x, y) && _layers[1][x, y] != null)
                        _layers[1][x, y].Draw(0.5f, batch);
        }

        public void DrawForegroundTileHighlight(Camera cam, SpriteBatch batch, Effect effect)
        {

            effect.CurrentTechnique = effect.Techniques["ForegroundTile"];
            effect.Parameters["TextureSize"].SetValue(new Vector2(tileSet.Width, tileSet.Height));
            effect.Parameters["ScreenTexture"].SetValue(tileSet);
            effect.CurrentTechnique.Passes[0].Apply();
            Rectangle drawRect = cam.ViewSpace;
            int minX = drawRect.X / tileSize;
            int minY = drawRect.Y / tileSize;
            int maxX = (drawRect.Width + drawRect.X) / tileSize + 1;
            int maxY = (drawRect.Height + drawRect.Y) / tileSize + 1;
            for (int x = minX; x < maxX; x++)
                for (int y = minY; y < maxY; y++)
                    if (isValid(x, y) && _layers[2][x, y] != null)
                        _layers[2][x, y].Draw(0.4f, batch);
        }

        public void ChangeTile(int x, int y, int layer, int tileID)
        {
            _sem.WaitOne();
            if (isValid(x, y) && layer >= 0 && layer < 3 && tileID >= 0)
            {
                if (_layers[layer][x, y] != null)
                    _layers[layer][x, y].SetTile(tileID);
                else
                    _layers[layer][x, y] = new Tile(new Rectangle(tileID * tileSize, 0, tileSize, tileSize), new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), tileSet, layer == 1);
            }
            else if (isValid(x, y) && layer >= 0 && layer < 3)
                _layers[layer][x, y] = null;
            _sem.Release();
        }

        public EventObject GetEvent(GameObject obj)
        {
            int posX = (int)(obj.MidPosition.X / tileSize);
            int posY = (int)(obj.MidPosition.Y / tileSize);

            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    if (isValid(posX + x, posY + y))
                        if (_events[posX + x, posY + y] != null && _events[posX + x, posY + y].Collides(obj))
                            return _events[posX + x, posY + y];
            return null;
        }

        public void RemoveEvent(EventObject obj)
        {
            _sem.WaitOne();
            int posX = (int)(obj.MidPosition.X / tileSize);
            int posY = (int)(obj.MidPosition.Y / tileSize);
            if (obj == _events[posX, posY])
                _events[posX, posY] = null;
            _sem.Release();
        }

        public void AddObject(EventObject obj)
        {
            _sem.WaitOne();
            int posX = (int)(obj.MidPosition.X / tileSize);
            int posY = (int)(obj.MidPosition.Y / tileSize);
            if (isValid(posX, posY) && _events[posX, posY] == null)
                _events[posX, posY]=obj;
            _sem.Release();                                                
        }

        public void SetGlow(int id, Vector2 position, Color color, float radius, bool shadows)
        {
            LightMap.LightReference.SetGlow(id, position, color, radius, shadows);
        }

        public void SetGlow(int id, Vector2 position, Color color, float radius, bool shadows, float timeout)
        {
            LightMap.LightReference.SetGlow(id, position, color, radius, shadows,timeout);
        }

        public Rectangle MapBoundaries
        {
            get { return new Rectangle(0, 0, _layers[0].GetLength(0) * tileSize, _layers[0].GetLength(1) * tileSize); }
        }

        private bool isValid(int x, int y)
        {
            return (uint)x < _layers[0].GetLength(0) && (uint)y < _layers[0].GetLength(1);
        }

        public ParticleManager Particles
        {
            get { return WorldController.Particles; }
        }
    }
}
