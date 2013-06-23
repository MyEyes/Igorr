using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class TriggerPickup : EventObject
    {
        bool _touched=false;
        bool _touchAgain = false;
        bool _global = false;
        string _triggerName;
        Player lastToucher = null;
        string _playerInfoName;
        int _playerInfoValue;
        string _textureName;
        bool setsTrigger;
        bool setsInfo;

        public TriggerPickup(string triggerName, bool global, string playerInfoName, int playerInfoValue, string textureName, IMap map, Rectangle rect, int id)
            : base(map, rect, id)
        {
            _objectType = 89;
            _global = global;
            _triggerName = triggerName;
            setsTrigger = !string.IsNullOrEmpty(_triggerName);
                        _playerInfoName = playerInfoName;
            _playerInfoValue = playerInfoValue;
            setsInfo = !string.IsNullOrEmpty(_playerInfoName);
            _textureName = textureName;
            _info = _textureName;
        }

        public override void Update(float ms)
        {
            if (!_touched)
                _touchAgain = true;
            _touched = false;
            base.Update(ms);
        }

        public override void Event(Player obj)
        {
            if (_touchAgain)
            {
                if (setsTrigger)
                {
                    if (_global)
                        GlobalTriggers.SetTriggerValue(_triggerName, 1);
                    else
                        _map.SetTrigger(_triggerName, true);
                }
                _touched = true;
                _touchAgain = false;
            }
            if (setsInfo && obj != lastToucher)
            {
                _map.SetInfo(obj.Name, _playerInfoName, _playerInfoValue);
                Protocol.Messages.DeSpawnMessage dsm = (Protocol.Messages.DeSpawnMessage)_map.ObjectManager.Server.ProtocolHelper.NewMessage(Protocol.MessageTypes.DeSpawn);
                dsm.id = _id;
                dsm.Encode();
                _map.ObjectManager.Server.SendClient(obj, dsm);
                lastToucher = obj;
            }
        }
    }
}
