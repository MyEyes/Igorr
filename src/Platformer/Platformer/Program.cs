using System;

namespace IGORR.Client
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                IGORR.Modules.ModuleManager.SetContentDir("Content");
                IGORR.Modules.ModuleManager.LoadAllModules();
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                if (!(e is System.Threading.ThreadAbortException))
                    System.Windows.Forms.MessageBox.Show(e.ToString(), "Error: " + e.Message);
            }
        }
    }
#endif
}

