using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR.Server.Logic
{
    public static class GlobalTriggers
    {
        static Dictionary<string, int> _triggers;
        static Dictionary<string, List<Action<int>>> _triggerCallbacks;

        static GlobalTriggers()
        {
            _triggers = new Dictionary<string, int>();
            _triggerCallbacks = new Dictionary<string, List<Action<int>>>();
        }

        public static int GetTriggerValue(string triggerName)
        {
            if (!_triggers.ContainsKey(triggerName))
            {
                _triggers.Add(triggerName, 0);
            }
            return _triggers[triggerName];
        }

        public static void SetTriggerValue(string triggerName, int val)
        {
            if (!_triggers.ContainsKey(triggerName))
            {
                _triggers.Add(triggerName, val);
            }
            else
                _triggers[triggerName] = val;

                if (_triggerCallbacks.ContainsKey(triggerName))
                {
                    for (int x = 0; x < _triggerCallbacks[triggerName].Count; x++)
                        _triggerCallbacks[triggerName][x](val);
                    //_triggerCallbacks[triggerName].Clear();
                }
        }

        public static void RegisterTriggerCallback(string triggerName, Action<int> callback)
        {
            if (!_triggerCallbacks.ContainsKey(triggerName))
            {
                _triggerCallbacks.Add(triggerName, new List<Action<int>>());
            }
            _triggerCallbacks[triggerName].Add(callback);
        }
    }
}
