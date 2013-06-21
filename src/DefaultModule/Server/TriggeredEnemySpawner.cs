using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IGORR.Server.Logic;
using Microsoft.Xna.Framework;

namespace DefaultModule.Server
{
    class TriggeredEnemySpawner:EventObject
    {
        List<NPC> _enemies;
        int _targetNum = 0;
        int _enemyID;
        float _respawnTime = 5000;
        float _spawnCountdown = 0;

        bool _globalActivation;
        bool _activationTriggered;
        bool _globalClear;
        bool _clearTriggered;
        bool _active = true;
        string _activationTrigger;
        string _clearedTrigger;

        public TriggeredEnemySpawner(IMap map, Rectangle position, int id, int enemyID, int numberOfEnemies, float RespawnTime,bool globalActivation, string activationTrigger, bool globalClear, string clearedTrigger)
            : base(map, position, id)
        {
            _objectType = 87;
            _targetNum = numberOfEnemies;
            _enemyID = enemyID;
            _respawnTime = RespawnTime;
            _enemies = new List<NPC>();

            _activationTrigger = activationTrigger;
            _activationTriggered = !string.IsNullOrEmpty(_activationTrigger);
            _clearedTrigger = clearedTrigger;
            _clearTriggered = !string.IsNullOrEmpty(_clearedTrigger);
            _globalActivation = globalActivation;
            _globalClear = globalClear;
            if (_activationTriggered)
            {
                if (_globalActivation)
                {
                    GlobalTriggers.RegisterTriggerCallback(activationTrigger, new Action<int>(CheckActive));
                    CheckActive(GlobalTriggers.GetTriggerValue(activationTrigger));
                }
                else
                    _active = _map.GetTrigger(activationTrigger);
            }
        }

        public void CheckActive(int triggerValue)
        {
            _active = triggerValue > 0;
        }

        public override void Update(float ms)
        {
            if (_enemies.Count < _targetNum && _active)
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
                        //Unset cleared trigger
                        if (_enemies.Count == 1 && _clearTriggered)
                        {
                            if (_globalClear)
                                GlobalTriggers.SetTriggerValue(_clearedTrigger, 0);
                            else
                                _map.SetTrigger(_clearedTrigger, false);
                        }
                    }
                    _spawnCountdown = _respawnTime;
                }
            }

            if (!_globalActivation && _activationTriggered)
            {
                _active = _map.GetTrigger(_activationTrigger);
            }

            if (_enemies.Count == 0 && _clearTriggered)
            {
                //Set cleared trigger
                if (_globalClear)
                    GlobalTriggers.SetTriggerValue(_clearedTrigger, 1);
                else
                    _map.SetTrigger(_clearedTrigger, true);
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
