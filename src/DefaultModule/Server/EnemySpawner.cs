using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Server.Logic;
using Microsoft.Xna.Framework;

namespace DefaultModule.Server
{
    class EnemySpawner:EventObject
    {
        List<NPC> _enemies;
        int _targetNum = 0;
        int _enemyID;
        float _respawnTime = 5000;
        float _spawnCountdown = 0;

        public EnemySpawner(IMap map, Rectangle position, int id, int enemyID, int numberOfEnemies, float RespawnTime)
            : base(map, position, id)
        {
            _objectType = 86;
            _targetNum = numberOfEnemies;
            _enemyID = enemyID;
            _respawnTime = RespawnTime;
            _enemies = new List<NPC>();
        }

        public override void Update(float ms)
        {
            if (_enemies.Count < _targetNum)
            {
                _spawnCountdown -= ms;
                if (_spawnCountdown <= 0)
                {
                    GameObject obj = IGORR.Modules.ModuleManager.SpawnByIdServer(_map, _enemyID, _map.ObjectManager.getID(), new Point((int)this.Position.X, (int)this.Position.Y), null);
                    NPC newNPC = obj as NPC;
                    if (newNPC != null)
                    {
                        _map.ObjectManager.Add(newNPC);
                        _enemies.Add(newNPC);
                    }
                    _spawnCountdown = _respawnTime;
                }
            }
            for (int x = 0; x < _enemies.Count; x++)
            {
                if (_enemies[x].HP <= 0)
                {
                    _enemies.RemoveAt(x);
                    x--;
                }
            }
        }


    }
}
