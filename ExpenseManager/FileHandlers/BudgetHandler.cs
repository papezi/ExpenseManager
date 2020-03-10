using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using ExpenseManager.BudgetStructure;

namespace ExpenseManager.FileHandlers
{
    class BudgetHandler
    {
        public Budget Budget { get; set; }

        public async Task LoadData(string budgetName)
        {
            string path = $"{budgetName}.json";
            if (File.Exists(path))
            {
                using (StreamReader file = await Task.Run(() => File.OpenText(path)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Budget = await Task.Run(() => (Budget)serializer.Deserialize(file, typeof(Budget)));
                }
                //Console.WriteLine("Budget file loaded.");
            }
        }

        public async Task SaveData(string budgetName)
        {
            string path = $"{budgetName}.json";
            using (StreamWriter file = await Task.Run(() => File.CreateText(path)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Budget);
            }
            //Console.WriteLine("Budget file saved.");
        }

        //Create new budget
        public bool CreateBudget(string budgetName, string pass, string description)
        {
            string path = $"{budgetName}.json";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    Budget = new Budget(budgetName, pass, description);
                    Console.WriteLine("INFO: New budget created.");
                    return true;
                }
            }
            Console.WriteLine("ERROR: Budget exist.");
            return false;
        }

        //Validate budget password
        public Budget Validate(string pass)
        {
            if (Budget != null && Budget.Pass == pass)
            {
                return Budget;
            }
            Console.WriteLine("ERROR: Invalid password.");
            return null;
        }
    }
}
