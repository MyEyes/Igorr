using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace IGORR.Game
{
    static class Settings
    {
        public static string ServerAddress = "localhost";
        public static string LoginName = "TestName";
        public static string LoginPassword = "TestPassword";

        public static void LoadSettings()
        {
            Type type = typeof(Settings);
            FieldInfo[] fields = type.GetFields();

            StreamReader reader = new StreamReader("settings.cfg");
            string[] lines=null;
            if (reader != null)
                lines = reader.ReadToEnd().Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if(lines!=null)
                for (int x = 0; x < lines.Length; x++)
                {
                    string[] split = lines[x].Split(new string[] { "=" }, StringSplitOptions.None);
                    for (int y = 0; y < fields.Length; y++)
                        if (fields[y].Name == split[0])
                            fields[y].SetValue(null, split[1]);
                }
        }
    }
}
