using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic
{
    class AttackTrigger : EventObject
    {
        float timeOut = 0;
        float updateTimeout = 1000;
        bool _global = false;
        bool val = true;
        string _triggerName;

        public AttackTrigger(string triggerName, bool global, Map map, Rectangle rect, int id)
            : base(map, rect, id)
        {
            _triggerName = triggerName;
            _global = global;
            Val = false;
        }

        public override void Update(float ms)
        {
            if (updateTimeout<=0)
            {
                if (_global)
                {
                    Val = GlobalTriggers.GetTriggerValue(_triggerName);
                }
                else
                    Val = _map.GetTrigger(_triggerName);
                updateTimeout = 1000;
            }
            if (timeOut <= 0 && _map.ObjectManager.AttackManager.CheckObject(this) > 0)
            {
                if (_global)
                {
                    Val = !val;
                    GlobalTriggers.SetTriggerValue(_triggerName, val);
                }
                else
                {
                    Val = !val;
                    _map.SetTrigger(_triggerName, val);
                }
                timeOut = 1000;
            }
            timeOut -= ms;
            updateTimeout -= ms;
            base.Update(ms);
        }

        private bool Val
        {
            set
            {
                if (val != value)
                {
                    val = value;
                    _map.ChangeTile(2, this.MidPosition, val?29:28);
                }
            }
        }

        public override void Event(Player obj)
        {
        }
    }
}
