using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class TurnOnBlocker : EventObject
    {
        string triggerName;
        bool wantToBlock;
        bool blocked;

        public TurnOnBlocker(string triggerName, bool global, IMap map, Rectangle position, int id)
            : base(map, position, id)
        {
            _objectType = 100;
            _map.ChangeTile(1, this.MidPosition, -1);
            GlobalTriggers.RegisterTriggerCallback(triggerName, new Action<bool>(FreeBlock));
        }

        public override void Update(float ms)
        {
            if (!blocked && wantToBlock)
            {
                _map.ChangeTile(1, this.MidPosition, 20);
                wantToBlock = false;
            }
            blocked = false;
        }

        public void FreeBlock(bool val)
        {
            if (val == false)
                _map.ChangeTile(1, this.MidPosition, -1);
            else
                wantToBlock = true;
        }

        public override void Event(Player obj)
        {
            blocked = true;
        }
    }
}
