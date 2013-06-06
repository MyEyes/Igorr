using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGORR.Server
{
    class Command
    {
        public string Name;
        Action<string> _action;

        public Command(string name, Action<string> act)
        {
            Name = name;
            _action = act;
        }

        public void Invoke(string str)
        {
            _action(str);
        }
    }

    class Program
    {
        static List<Command> _commands;
        static void Main(string[] args)
        {
            try
            {
                _commands = new List<Command>();
                Server server = new Server();
                string input;
                AddCommand(new Command("ListCommands", new Action<string>(ListCommands)));
                AddCommand(new Command("Status", new Action<string>(server.Status)));
                AddCommand(new Command("WriteTypes", new Action<string>(WriteTypeList)));
                while ((input = Console.ReadLine()) != "exit")
                {
                    string[] split = input.Split(new string[] { }, StringSplitOptions.None);
                    for (int x = 0; x < _commands.Count; x++)
                    {
                        if (split[0].ToLower() == _commands[x].Name.ToLower())
                            try
                            {
                                _commands[x].Invoke(input.Substring(split[0].Length));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("There was a problem executing your command" + _commands[x].Name + ":\n" + e.Message); ;
                            }
                    }
                }
                Management.LoginData.Close();
                Management.ClientInfoInterface.StoreInfo();
                server.Exit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
                Management.LoginData.Close();
                Management.ClientInfoInterface.StoreInfo();
            }
        }

        public static void AddCommand(Command c)
        {
            _commands.Add(c);
        }

        static void ListCommands(string str)
        {
            Console.WriteLine("Start Command List:");
            for (int x = 0; x < _commands.Count; x++)
                Console.WriteLine("\t-"+_commands[x].Name);
            Console.WriteLine("End Command List");
        }

        static void WriteTypeList(string fileName)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName);
            List<Modules.ObjectTemplate> templates = Modules.ModuleManager.GetTemplates();
            writer.WriteLine("############################ Objects ################################");
            writer.WriteLine();
            templates.Sort((a,b)=>a.TypeID-b.TypeID);
            for (int x = 0; x < templates.Count; x++)
            {
                writer.WriteLine(templates[x].TypeID.ToString() + "\t" + templates[x].GetType().Name);
            }
            writer.WriteLine();
            List<Modules.AttackTemplate> atemplates = Modules.ModuleManager.GetAttackTemplates();
            writer.WriteLine("############################ Attacks ################################");
            writer.WriteLine();
            atemplates.Sort((a, b) => a.TypeID - b.TypeID);
            for (int x = 0; x < atemplates.Count; x++)
            {
                writer.WriteLine(atemplates[x].TypeID.ToString() + "\t" + atemplates[x].GetType().Name);
            }
            writer.WriteLine();
            List<Modules.EffectTemplate> etemplates = Modules.ModuleManager.GetEffectTemplates();
            writer.WriteLine("############################ Effects ################################");
            writer.WriteLine();
            etemplates.Sort((a, b) => a.EffectID - b.EffectID);
            for (int x = 0; x < etemplates.Count; x++)
            {
                writer.WriteLine(etemplates[x].EffectID.ToString() + "\t" + etemplates[x].GetType().Name);
            }
            writer.Close();
        }
    }
}
