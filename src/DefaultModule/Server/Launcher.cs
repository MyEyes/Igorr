using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class Pusher : EventObject
    {
        bool _global = false;
        string _triggerName;

        public Pusher(string triggerName, bool global, IMap map, Rectangle rect, int id)
            : base(map, rect, id)
        {
            _triggerName = triggerName;
            _global = global;
            _objectType = 85;
        }

        public override void Update(float ms)
        {
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
            if (_global && GlobalTriggers.GetTriggerValue(_triggerName) >= 1)
            {
                obj.Knockback(new Vector2(120, -160));
                DoEffect(5, new Point((int)MidPosition.X, (int)MidPosition.Y), new Vector2(120, -160), "");
            }
            else if (_map.GetTrigger(_triggerName))
            {
                obj.Knockback(new Vector2(1, -160));
                DoEffect(5, new Point((int)MidPosition.X, (int)MidPosition.Y), new Vector2(120, -160), "");
            }
        }
    }
}
