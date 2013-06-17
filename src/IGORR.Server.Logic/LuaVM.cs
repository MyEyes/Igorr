using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;
using Lua511;
using System.Reflection;

namespace IGORR.Server
{
    public static class LuaVM
    {
        static Lua _lua;

        static LuaVM()
        {
            _lua = new Lua();
            MethodBase mbase = typeof(LuaVM).GetMethod("Print").GetBaseDefinition();
            _lua.RegisterFunction("Print", null, mbase);
        }

        public static void DoString(string command)
        {
            _lua.DoString(command);
        }

        public static void DoFile(string file)
        {
            if (System.IO.File.Exists(file))
                _lua.DoFile(file);
        }

        public static void Register(string functionName, object invokee, MethodBase method)
        {
            _lua.RegisterFunction(functionName, invokee, method);
        }

        public static void Print(string text)
        {
            Console.WriteLine(text);
        }

        public static T GetValue<T>(string name, T defaultValue)
        {
            object val=null;
            try
            {
                val = _lua[name];
            }
            catch (LuaException le)
            {
            }
            if (val == null)
                return defaultValue;
            try
            {
                return (T)val;
            }
            catch (InvalidCastException ice)
            {
                return defaultValue;
            }
        }

        public static void SetValue(string name, object value)
        {
            _lua[name] = value;
        }
    }
}
