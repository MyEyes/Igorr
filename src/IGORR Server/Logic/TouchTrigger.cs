using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class TouchTrigger : EventObject
    {
        bool _touched=false;
        bool _touchAgain = false;
        bool _global = false;
        string _triggerName;
        Player lastToucher = null;

        public TouchTrigger(string triggerName, bool global, Map map, Rectangle rect, int id)
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
            if (_touchAgain)
            {
                if (_global)
                    GlobalTriggers.SetTriggerValue(_triggerName, true);
                else
                    _map.SetTrigger(_triggerName, true);

                _touched = true;
                _touchAgain = false;
            }
        }
    }
}
