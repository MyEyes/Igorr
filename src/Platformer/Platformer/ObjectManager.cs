using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using System.Collections.Concurrent;

namespace IGORR.Game
{
    class ObjectManager
    {
        Map _map;
        LightMap _light;
        List<GameObject> _objects;
        List<GameObject> _draw;
        Player _player = null;
        int playerID = -1;
        ContentManager _content;
        const int standardSize = 16;
        Semaphore _sem;
        Dictionary<int, AttackInfo> _attackTypes;

        public ObjectManager(Map map, ContentManager content)
        {
            _map = map;
            _sem = new Semaphore(1, 1);
            _content = content;
            _objects = new List<GameObject>();
            _draw = new List<GameObject>();
            SetUpAttacks();
        }

        private void SetUpAttacks()
        {
            _attackTypes = new Dictionary<int, AttackInfo>();
            AttackInfo info = new AttackInfo();

            info.CollisionDespawn = true;
            info.LifeTime = 0.1f;
            _attackTypes.Add(1, info);

            info.CollisionDespawn = false;
            info.LifeTime = 0.2f;
            _attackTypes.Add(2, info);

            info.CollisionDespawn = false;
            info.Physics = true;
            info.bounceFactor = 0.4f;
            info.LifeTime = 2.5f;
            _attackTypes.Add(3, info);
            _attackTypes.Add(4, info);
            info.Physics = false;
            info.CollisionDespawn = true;
            info.LifeTime = 0.1f;
            _attackTypes.Add(5003, info);

            info.CollisionDespawn = true;
            info.LifeTime = 0.4f;
            _attackTypes.Add(5006, info);

            info.CollisionDespawn = true;
            info.LifeTime = 0.5f;
            _attackTypes.Add(5004, info);

            info.CollisionDespawn = true;
            info.LifeTime = 2.2f;
            _attackTypes.Add(5005, info);
        }

        public void Clear()
        {
            _sem.WaitOne();
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x] is EventObject)
                    (_objects[x] as EventObject).Remove();
            }
            _objects.Clear();
            _draw.Clear();
            _sem.Release();
        }

        public void SetMap(Map map)
        {
            _map = map;
        }

        public void SetLight(LightMap light)
        {
            _light = light;
        }

        public void SpawnObject(Vector2 position,Vector2 move, int objectType,int groupID, int id, string name, string charName)
        {
            if (_map == null)
                return;
            _sem.WaitOne();
            switch (objectType)
            {
                case 't' - 'a': Lava lava = new Lava(_map, _content.Load<Texture2D>("Lava"), new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize), id); _map.AddObject(lava); _objects.Add(lava); break;
                case 'b' - 'a': PartPickup pp = new PartPickup(new Legs(_content.Load<Texture2D>("FeetIcon")), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize), id); _map.AddObject(pp); _objects.Add(pp); break;
                case 'd' - 'a': pp = new PartPickup(new Striker(_content.Load<Texture2D>("Attack")), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize), id); _map.AddObject(pp); _objects.Add(pp); break;
                case 15: pp = new PartPickup(new Striker(_content.Load<Texture2D>("healthOrb")), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize), id); _map.AddObject(pp); _objects.Add(pp); break;
                case 20: pp = new PartPickup(new Striker(_content.Load<Texture2D>("expOrb")), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize), id); _map.AddObject(pp); _objects.Add(pp); break;
                case 9: pp = new PartPickup(new Striker(_content.Load<Texture2D>("Attack")), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize), id); _map.AddObject(pp); _objects.Add(pp); break;
                case 'c' - 'a': Exit exit = new Exit(_content.Load<Texture2D>("Exit"), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize), id); _map.AddObject(exit); _objects.Add(exit); break;
                //case 'a' - 'a': Player player = new Player(_content.Load<Texture2D>("blob"), new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), 2*standardSize, 2*standardSize-1), id);
                case 'a' - 'a': Player player;
                    if (string.IsNullOrEmpty(charName))
                        player = new Player(_content.Load<Texture2D>("blob"), new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize - 1), id);
                    else
                        player = new Player(charName, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize - 1), id);
                    player.Name = name;
                    if (playerID != -1 && playerID == id)
                        _player = player;
                    _objects.Add(player); 
                    _draw.Add(player); 
                    player.SetGroup(groupID);
                    break;
                case 'e' - 'a': pp = new PartPickup(new Wings(_content.Load<Texture2D>("Wings")), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize), id); _map.AddObject(pp); _objects.Add(pp); break;
                case 'g' - 'a': exit = new Exit(_content.Load<Texture2D>("Exit"), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize - 1), id); _map.AddObject(exit); _objects.Add(exit); break;
                case 22: ScorePost sp = new ScorePost(_map, _content.Load<Texture2D>("ScorePost"), new Rectangle((int)(position.X - 12), (int)(position.Y - 56), 24, 64), id); _map.AddObject(sp); _objects.Add(sp); sp.SetColor(ScorePost.ScoreColor.Red); break;
                case 23: sp = new ScorePost(_map, _content.Load<Texture2D>("ScorePost"), new Rectangle((int)(position.X - 12), (int)(position.Y - 56), 24, 64), id); _map.AddObject(sp); _objects.Add(sp); sp.SetColor(ScorePost.ScoreColor.Blue); break;
                case 29: sp = new ScorePost(_map, _content.Load<Texture2D>("ScorePost"), new Rectangle((int)(position.X - 12), (int)(position.Y - 56), 24, 64), id); _map.AddObject(sp); _objects.Add(sp); sp.SetColor(ScorePost.ScoreColor.Neutral); break;
                case 26: exit = new Exit(_content.Load<Texture2D>("RedTel"), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize - 1), id); _map.AddObject(exit); _objects.Add(exit); break;
                case 27: exit = new Exit(_content.Load<Texture2D>("BlueTel"), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize - 1), id); _map.AddObject(exit); _objects.Add(exit); break;
                case 28: exit = new Exit(_content.Load<Texture2D>("Exit"), _map, new Rectangle((int)(position.X - standardSize / 2), (int)(position.Y - standardSize / 2), standardSize, standardSize - 1), id); _map.AddObject(exit); _objects.Add(exit); break;
            }
            _sem.Release();
        }

        public void SpawnAttack(Vector2 pos, Vector2 mov, int type)
        {
            if (_map == null)
                return;
            if (!_attackTypes.ContainsKey(type))
                return;
            Texture2D tex;
            switch (type)
            {
                case 3: tex = _content.Load<Texture2D>("Grenade"); break;
                case 4: WorldController.Particles.Boom(_light,pos + new Vector2(20, 20)); return;
                default: tex = _content.Load<Texture2D>("Attack"); break;
            }
            Attack attack = new Attack(tex,new Rectangle((int)(pos.X), (int)(pos.Y), standardSize, standardSize - 4),mov,-2, _attackTypes[type]);
            _sem.WaitOne();
            _objects.Add(attack);
            _draw.Add(attack);
            _sem.Release();
        }

        public void GetObjectInfo(int id, string info)
        {
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x].ID == id)
                {
                    _objects[x].GetInfo(info);
                    break;
                }
            }
        }

        public Player GetPlayer(int id)
        {
            Player p = null;
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x].ID == id && _objects[x] is Player)
                {
                    p = _objects[x] as Player;
                    break;
                }
            }
            return p;
        }

        public void Update(float ms)
        {
            for (int x = 0; x < _draw.Count; x++)
            {
                if (_draw[x] is Player)
                    (_draw[x] as Player).Update(_map, ms / 1000.0f);
                else if (_draw[x] is Attack)
                    if (!(_draw[x] as Attack).Update(_map, ms / 1000.0f))
                    {
                        _objects.Remove(_draw[x]);
                        _draw.RemoveAt(x);
                        x--;
                    }
            }
        }

        public void SelectPlayer(int id)
        {
            playerID = id;
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x].ID == id && _objects[x] is Player)
                {
                    _player = _objects[x] as Player;
                    return;
                }
            }
        }

        public void SetPosition(Vector2 position, Vector2 newmove, int id, long updateTime)
        {
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x].ID == id)
                {
                    if (updateTime < _objects[x].LastUpdate)
                        return;
                    _objects[x].SetUpdateTime(updateTime);
                    _objects[x].SetPosition(position);
                    if (_objects[x] is Player)
                    {
                        (_objects[x] as Player).SetMove(newmove);
                    }
                    return;
                }
            }
        }

        public void DealDamage(int damage,Vector2 pos, int id)
        {
            bool found = false;
            Player target = null;
            for (int x = 0; x < _objects.Count; x++)
            {
                if (_objects[x].ID == id)
                {
                    if (_objects[x] is Player)
                    {
                        target=(_objects[x] as Player);
                        target.GetDamage(damage);
                        found = true;
                    }
                    break;
                }
            }
            if (found && target.HP <= 0)
                WorldController.Particles.Splat(pos, target.LastSpeed);
            else if(!found)
                WorldController.Particles.Splat(pos, Vector2.Zero);

            if (damage > 0)
            {
                TextManager.AddText(pos, 100, damage.ToString(), 1500, Color.Red, Color.Transparent);
            }
            else if (damage < 0)
                TextManager.AddText(pos, 100, (-damage).ToString(), 1500, Color.DarkGreen, Color.Transparent);
        }

        public void RemoveObject(int id)
        {
            _sem.WaitOne();
            if (id == playerID && _player!=null)
            {
                _objects.Remove(_player);
                _draw.Remove(_player);
                _player = null;
                playerID = -1;
            }
            else
            {
                for (int x = 0; x < _objects.Count; x++)
                {
                    if (_objects[x].ID == id)
                    {
                        if (_objects[x] is EventObject)
                            (_objects[x] as EventObject).Remove();
                        _objects.RemoveAt(x);
                        break;
                    }
                }

                for (int x = 0; x < _draw.Count; x++)
                {
                    if (_draw[x].ID == id)
                    {
                        _draw.RemoveAt(x);
                        _sem.Release();
                        return;
                    }
                }
            }
            _sem.Release();
        }

        public void Draw(SpriteBatch batch)
        {
            for (int x = 0; x < _draw.Count; x++)
                _draw[x].Draw(batch);
        }

        public Player Player
        {
            get { return _player; }
        }

        public Map Map
        {
            get { return _map; }
        }
    }
}
