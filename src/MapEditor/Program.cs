using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IGORR.Modules;
using System.IO;

namespace MapEditor
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ModuleManager.SetContentDir("Content");
            ModuleManager.LoadAllModules();

            Application.Run(new frmMain());
        }
    }
}
