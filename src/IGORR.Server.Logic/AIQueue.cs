using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR.Server.Logic.AI
{
    class AICommand
    {
        protected LuaNPC _parent;

        public AICommand(LuaNPC parent)
        {
            _parent = parent;
        }

        public virtual bool Update(float ms)
        {
            return false;
        }
    }

    class WaitCommand : AICommand
    {
        float _time;
        public WaitCommand(LuaNPC parent, float time):base(parent)
        {
            _time = time;
        }

        public override bool Update(float ms)
        {
            _time -= ms;
            return _time > 0;
        }
    }

    class MoveCommand : AICommand
    {
        float _time;
        float _dir;
                public MoveCommand(LuaNPC parent, float time, float dir):base(parent)
        {
            _time = time;
            _dir = dir;
        }

        public override bool Update(float ms)
        {
            _parent.Move(_dir,0);
            _time -= ms;
            return _time > 0;
        }
    }

    class JumpCommand : AICommand
    {
        public JumpCommand(LuaNPC parent)
            : base(parent)
        {
        }

        public override bool Update(float ms)
        {
            _parent.Jump();
            return false;
        }
    }

    class AIQueue
    {
        LuaNPC _parent;
        List<AICommand> _queue;

        public AIQueue(LuaNPC parent)
        {
            _parent = parent;
            _queue = new List<AICommand>();
        }

        public void Update(float ms)
        {
            if(_queue.Count==0)
                return;
            if (!_queue[0].Update(ms))
                _queue.RemoveAt(0);
        }

        public void Add(AICommand cmd)
        {
            _queue.Add(cmd);
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public bool Empty
        {
            get { return _queue.Count == 0; }
        }
    }
}
