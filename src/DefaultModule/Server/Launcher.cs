using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class Launcher : EventObject
    {
        bool _touched=false;
        bool _touchAgain = false;
        bool _global = false;
        string _triggerName;
        Player lastToucher = null;

        public Launcher(string triggerName, bool global, IMap map, Rectangle rect, int id)
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

            if (_global && GlobalTriggers.GetTriggerValue(_triggerName) >= 1)
            {
                DoEffect(5, new Point((int)MidPosition.X, (int)MidPosition.Y), new Vector2(120, -160), "");
            }
            else if (_map.GetTrigger(_triggerName))
            {
                DoEffect(5, new Point((int)MidPosition.X, (int)MidPosition.Y), new Vector2(120, -160), "");
            }

            base.Update(ms);
        }

        public override void Event(Player obj)
        {
            if (_touchAgain)
            {
                if (_global && GlobalTriggers.GetTriggerValue(_triggerName) >= 1)
                {
                    obj.Knockback(new Vector2(120, -160));
                    DoEffect(5, new Point((int)MidPosition.X, (int)MidPosition.Y), new Vector2(120, -160), "");
                }
                else if (_map.GetTrigger(_triggerName))
                {
                    obj.Knockback(new Vector2(120, -160));
                    DoEffect(5, new Point((int)MidPosition.X, (int)MidPosition.Y), new Vector2(120, -160), "");
                }

                _touched = true;
                _touchAgain = false;
            }
        }
    }
}
