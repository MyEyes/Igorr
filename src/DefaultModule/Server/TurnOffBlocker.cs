using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class TurnOffBlocker : EventObject
    {
        string triggerName;
        bool wantToBlock;
        bool blocked;
        int tileID;
        float countdown = 1000f;
        bool global;

        public TurnOffBlocker(string triggerName, bool global, IMap map, Rectangle position, int id, int tileID=20)
            : base(map, position, id)
        {
            this.triggerName = triggerName;
            this.tileID = tileID;
            this.global = global;
            _objectType = 100;
            _map.ChangeTile(1, this.MidPosition, tileID);
            if (global)
                GlobalTriggers.RegisterTriggerCallback(triggerName, new Action<int>(FreeBlock));
        }

        public override void Update(float ms)
        {
            if (!global)
            {
                FreeBlock(map.GetTrigger(triggerName) ? 1 : 0);
            }
            if (!blocked && wantToBlock && countdown <= 0)
            {
                _map.ChangeTile(1, this.MidPosition, tileID);
                wantToBlock = false;
            }
            if (wantToBlock)
                countdown -= ms;
            if (blocked && countdown < 1000)
                countdown = 1000;
            blocked = false;
        }

        public void FreeBlock(int val)
        {
            if (val == 1)
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
