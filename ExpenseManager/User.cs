using ExpenseManager.BudgetStructure;
using ExpenseManager.FileHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManager
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public User(string name, string pass)
        {
            Name = name;
            Password = pass;
        }

        //Menu
        public async void Run()
        {
            BudgetHandler budgetHandler = new BudgetHandler();
            Task loadDataTask = Task.CompletedTask;
            Task saveDataTask = Task.CompletedTask;

            Console.Clear();
            Console.WriteLine($"Welcome {Name}!");
            PrintHead();

            while (true)
            {
                string budgetName = "";
                string pass = "";
                Console.Write($"[{Name}]: ");
                string cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "0": //LOG OUT
                        Console.Clear();
                        await saveDataTask;
                        Console.WriteLine($"INFO: All changes saved.");
                        Console.WriteLine($"INFO: User: '{Name}' logged out.");
                        return;
                    case "1": //OPEN BUDGET
                        if (GetAccessName(ref budgetName, true))
                        {
                            await saveDataTask;
                            loadDataTask = budgetHandler.LoadData(budgetName);
                            Console.Write("> Password: ");
                            pass = Console.ReadLine();
                            Console.Clear();
                            await loadDataTask;
                            Budget budget = budgetHandler.Validate(pass);
                            if (budget != null)
                            {
                                budget.Open(Name);
                                saveDataTask = budgetHandler.SaveData(budgetName);
                            }
                        }
                        break;
                    case "2": //CREATE BUDGET
                        if (GetAccessName(ref budgetName, false))
                        {
                            Console.Write("> Password: ");
                            pass = Console.ReadLine();
                            Console.Write("> Description: ");
                            string description = Console.ReadLine();
                            Console.Clear();
                            if (budgetHandler.CreateBudget(budgetName, pass, description))
                            {
                                saveDataTask = budgetHandler.SaveData(budgetName);
                            }
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
            Console.WriteLine("0: Log out.");
            Console.WriteLine("1: Open budget. (TIP: name='budget' pass='1234')");
            Console.WriteLine("2: Create budget.");
            Console.WriteLine("====================================");
        }

        //Get and check if budget file name is valid, open mode: true - opening file, false - creating file
        public bool GetAccessName(ref string budgetName, bool openMode)
        {
            Console.Write("> Budget name: ");
            budgetName = Console.ReadLine();
            if (File.Exists($"{budgetName}.json") ^ openMode)
            {
                Console.Clear();
                Console.WriteLine($"ERROR: Budget '{budgetName}' {(openMode ? "not " : "")}found.");
                return false;
            }
            return true;
        }
    }
}
