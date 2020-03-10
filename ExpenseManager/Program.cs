using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManager.FileHandlers;

namespace ExpenseManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var Manager = new Manager();
            Manager.LogIn();

            //Console.WriteLine("End. Press any key.");
            //string end = Console.ReadLine();
        }
    }
}
