using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class AttackTrigger : EventObject
    {
        float timeOut = 0;
        float updateTimeout = 1000;
        bool _global = false;
        int val = 0;
        string _triggerName;

        public AttackTrigger(string triggerName, bool global, IMap map, Rectangle rect, int id)
            : base(map, rect, id)
        {
            _triggerName = triggerName;
            _global = global;
            Val = 0;
            _objectType = 11;
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
                    Val = _map.GetTrigger(_triggerName)?1:0;
                updateTimeout = 1000;
            }
            if (timeOut <= 0 && _map.ObjectManager.AttackManager.CheckObject(this) > 0)
            {
                if (_global)
                {
                    Val = 1-val;
                    GlobalTriggers.SetTriggerValue(_triggerName, val);
                }
                else
                {
                    Val = 1-val;
                    _map.SetTrigger(_triggerName, val==1);
                }
                timeOut = 1000;
            }
            timeOut -= ms;
            updateTimeout -= ms;
            base.Update(ms);
        }

        private int Val
        {
            set
            {
                if (val != value)
                {
                    val = value;
                    _map.ChangeTile(2, this.MidPosition, val==1?29:28);
                }
            }
        }

        public override void Event(Player obj)
        {
        }
    }
}
