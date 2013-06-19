using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class StepOnButton : EventObject
    {
        bool _touched=false;
        bool _touchAgain = false;
        bool _global = false;
        string _triggerName;

        public StepOnButton(string triggerName, bool global, IMap map, Rectangle rect, int id)
            : base(map, rect, id)
        {
            _triggerName = triggerName;
            _global = global;
            _objectType = 84;
        }

        public override void Update(float ms)
        {
            if (!_touched && !_touchAgain)
            {
                _touchAgain = true;
                UpdateState();
            }
            _touched = false;
            base.Update(ms);
        }

        public void UpdateState()
        {
            Protocol.Messages.ObjectInfoMessage oim = (Protocol.Messages.ObjectInfoMessage)_map.ObjectManager.Server.ProtocolHelper.NewMessage(Protocol.MessageTypes.ObjectInfo);
            oim.objectID = _id;
            oim.info = "" + (char)(_touched ? 1 : 0);
            oim.Encode();
            _map.ObjectManager.Server.SendAllMap(_map, oim, false);
        }

        public override void Event(Player obj)
        {
            //Trigger should only be set one frame when someone steps on it.
            if (!obj.OnGround)
                return;
            _touched = true;
            if (_touchAgain)
            {
                if (_global)
                    GlobalTriggers.SetTriggerValue(_triggerName, 1);
                else
                    _map.SetTrigger(_triggerName, true);

                _touchAgain = false;
                UpdateState();
            }
            else
            {
                if (_global)
                    GlobalTriggers.SetTriggerValue(_triggerName, 0);
                else
                    _map.SetTrigger(_triggerName, false);

                _touchAgain = false;
            }
        }
    }
}
