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
        Player lastToucher = null;

        public StepOnButton(string triggerName, bool global, IMap map, Rectangle rect, int id)
            : base(map, rect, id)
        {
            _triggerName = triggerName;
            _global = global;
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
            //Trigger should only be set one frame when someone steps on it.
            _touched = true;
            if (_touchAgain)
            {
                if (_global)
                    GlobalTriggers.SetTriggerValue(_triggerName, 1);
                else
                    _map.SetTrigger(_triggerName, true);

                _touchAgain = false;
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
