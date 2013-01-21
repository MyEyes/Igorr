using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IGORR.Server.Management
{

    class Login
    {
        public string Name;
        public string Password;
    }

    static class LoginData
    {
        static List<Login> _logins;
        const bool fastCreate = true;

        static LoginData()
        {
            _logins = new List<Login>();
            StreamReader reader = new StreamReader("logins.lst");
            string[] lines = reader.ReadToEnd().Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            reader.Close();
            for (int x = 0; x < lines.Length; x+=2)
            {
                Login login = new Login();
                login.Name = lines[x];
                login.Password = lines[x + 1];
                _logins.Add(login);
            }

            Program.AddCommand(new Command("ListAccounts", new Action<string>(PrintList)));
        }

        static void PrintList(string str)
        {
            Console.WriteLine("Start Accounts list:");
            for (int x = 0; x < _logins.Count; x++)
            {
                Console.WriteLine("\tName: " + _logins[x].Name + "\t Password: " + _logins[x].Password);
            }
            Console.WriteLine("End Accounts list");
        }

        public static bool CheckLogin(string name, string password)
        {
            for (int x = 0; x < _logins.Count; x++)
            {
                if (_logins[x].Name == name)
                {
                    if (_logins[x].Password == password)
                        return true;
                    return false;
                }
            }
            if (fastCreate)
            {
                Login newLogin = new Login();
                newLogin.Name = name;
                newLogin.Password = password;
                Console.WriteLine("Created new Account: Name: " + name + " Password: " + password);
                _logins.Add(newLogin);
                return true;
            }
            return false;
        }

        public static void Close()
        {
            StreamWriter writer = new StreamWriter("logins.lst");
            for (int x = 0; x < _logins.Count; x++)
            {
                writer.WriteLine(_logins[x].Name);
                writer.WriteLine(_logins[x].Password);
            }
            writer.Close();
        }
    }
}
