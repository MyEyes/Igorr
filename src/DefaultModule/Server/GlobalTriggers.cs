using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR.Server.Logic
{
    static class GlobalTriggers
    {
        static Dictionary<string, bool> _triggers;
        static Dictionary<string, List<Action<bool>>> _triggerCallbacks;

        static GlobalTriggers()
        {
            _triggers = new Dictionary<string, bool>();
            _triggerCallbacks = new Dictionary<string, List<Action<bool>>>();
        }

        public static bool GetTriggerValue(string triggerName)
        {
            if (!_triggers.ContainsKey(triggerName))
            {
                _triggers.Add(triggerName, false);
            }
            return _triggers[triggerName];
        }

        public static void SetTriggerValue(string triggerName, bool val)
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

        public static void RegisterTriggerCallback(string triggerName, Action<bool> callback)
        {
            if (!_triggerCallbacks.ContainsKey(triggerName))
            {
                _triggerCallbacks.Add(triggerName, new List<Action<bool>>());
            }
            _triggerCallbacks[triggerName].Add(callback);
        }
    }
}
