using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace IGORR.Modules
{
    public static class ModuleManager
    {
        static List<IGORR.Modules.ObjectModule> _modules = new List<Modules.ObjectModule>();
        static string _rootDir = "";

        public static void SetContentDir(string dir)
        {
            _rootDir = dir;
        }


        public static void LoadModule(string name, bool Reload)
        {
            string curDir = Directory.GetCurrentDirectory();
            if (File.Exists(_rootDir + "/modules/" + name + ".dll"))
            {
                Assembly asm = Assembly.LoadFile(curDir + "/" + _rootDir + "/modules/" + name + ".dll");
                Type[] types = asm.GetTypes();
                for (int x = 0; x < types.Length; x++)
                {
                    if (types[x].IsSubclassOf(typeof(IGORR.Modules.ObjectModule)))
                    {
                        Type t = types[x];
                        IGORR.Modules.ObjectModule module = (IGORR.Modules.ObjectModule)t.GetConstructor(new Type[0]).Invoke(new object[0]);
                        if ((!Reload && !_modules.Contains(module)) || Reload)
                            _modules.Add(module);
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Could not find module named " + name + " in modules folder");
                //   throw new FileNotFoundException("Could not find module named " + name + " in modules folder");
            }
        }

        public static void LoadAllModules()
        {
            string path = _rootDir + "/modules/";
            string[] files = Directory.GetFiles(path);
            for (int x = 0; x < files.Length; x++)
            {
                files[x] = Path.GetFileNameWithoutExtension(files[x]);
                Console.WriteLine("Trying to load module: " + files[x]);
                LoadModule(files[x], true);
            }
        }

        public static List<IGORR.Modules.ObjectModule> Modules
        {
            get { return _modules; }
        }
    }
}
