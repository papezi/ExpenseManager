using ExpenseManager.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManager
{
    class Manager
    {
        //Menu
        public async void LogIn()
        {
            LogInHandler logInHandler = new LogInHandler();
            Task loadDataTask = logInHandler.LoadData();
            Task saveDataTask = Task.CompletedTask;

            Console.WriteLine("Welcome to Expence Manager. Please log in or register.");
            PrintHead();
            while (true)
            {
                Console.Write("[USER]: ");
                string cmd = Console.ReadLine();
                string name = "";
                string pass = "";
                switch (cmd)
                {
                    case "0": //CLOSE
                        await saveDataTask;
                        return;
                    case "1": //LOGIN
                        GetUserData(ref name, ref pass);
                        Console.Clear();
                        await loadDataTask;
                        User current = logInHandler.Validate(name, pass);
                        if (current != null)
                        {
                            current.Run();
                        }
                        break;
                    case "2": //REGISTER
                        GetUserData(ref name, ref pass);
                        Console.Clear();
                        await loadDataTask;
                        if (logInHandler.Register(name, pass))
                        {
                            saveDataTask = logInHandler.SaveData();
                        }
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("ERROR: Invalid command.");
                        break;
                }
                PrintHead();
            }
        }

        //Print available commands
        public void PrintHead()
        {
            Console.WriteLine("====================================");
            Console.WriteLine("Commands: (type number of command)");
            Console.WriteLine("0: End program");
            Console.WriteLine("1: Log in");
            Console.WriteLine("2: Register");
            Console.WriteLine("====================================");
        }

        //Get input
        public void GetUserData(ref string name, ref string pass)
        {
            Console.Write("> User name: ");
            name = Console.ReadLine();
            Console.Write("> Password: ");
            pass = Console.ReadLine();
        }
    }
}
