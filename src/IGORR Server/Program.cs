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
                server.Exit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
                Management.LoginData.Close();
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
    }
}
