using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManager.BudgetStructure
{
    class Budget
    {
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Description { get; set; }
        public int PaymentCount { get; set; }
        public int Sum { get; set; }
        public List<Year> Years { get; set; } = new List<Year>();

        public Budget(string name, string pass, string description)
        {
            Name = name;
            Pass = pass;
            Description = description;
            PaymentCount = 0;
            Sum = 0;
            Years.Add(new Year(DateTime.Now.Year));
        }

        //Menu
        public void Open(string userName)
        {
            Console.WriteLine($"INFO: Budget '{Name}' opened.");
            PrintHead();
            while (true)
            {
                Console.Write($"[{userName}]: ");
                string cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "0": //CLOSE
                        Console.Clear();
                        Console.WriteLine("INFO: Budget closed.");
                        return;
                    case "1": //SHOW DETAILS
                        ShowDetails();
                        continue;
                    case "2": //NEW PAYMENT
                        AddPayment(userName);
                        break;
                    case "3": //PERIOD REPORT
                        PeriodReport();
                        break;
                    case "4": //COMPARE YEARS
                        CompareYears();
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
            Console.WriteLine("0: Close budget.");
            Console.WriteLine("1: Show details.");
            Console.WriteLine("2: Add new payment.");
            Console.WriteLine("3: Show period report.");
            Console.WriteLine("4: Compare years.");
            Console.WriteLine("====================================");
            Console.WriteLine($"[{Name}] Balance: {Sum}");
            Console.WriteLine("====================================");
        }

        //Print budget details
        public void ShowDetails()
        {
            Console.WriteLine("DETAILS:");
            Console.WriteLine($"Budget name: {Name}");
            Console.WriteLine($"Password: {Pass}");
            Console.WriteLine($"Registered payments: {PaymentCount}");
            Console.WriteLine($"Description: {Description}");
        }

        //Get, parse and range-validate int input
        public bool GetIntInput(ref int result, string description, string variable, bool all = false)
        {
            int min = all ? 0 : 1; 
            Console.Write(description);
            if (!int.TryParse(Console.ReadLine(), out result) ||
                (variable == "month" && (result < min || result > 12)) ||
                (variable == "detail" && (result < 0 || result > 1)) ||
                (variable == "amount" && result == 0))
            {
                Console.Clear();
                Console.WriteLine($"ERROR: Incorrect {variable}.");
                return false;
            }
            return true;
        }

        //Add new payment
        public void AddPayment(string userName)
        {
            int idY = 0, idM = 0, amount = 0; //idY - id year, idM - id month
            if (!GetIntInput(ref idY, "Year: ", "year") ||
                !GetIntInput(ref idM, "Month (1 - 12): ", "month") ||
                !GetIntInput(ref amount, "Amount (not 0): ", "amount"))
            {
                return;
            }
            Console.Write("Description: ");
            string description = Console.ReadLine();
            Console.Clear();

            if (Years.Where(y => y.Id == idY).Count() == 0) //Create new year in budget if does not exist.
            {
                Years.Add(new Year(idY));
                Years.Sort();
                Console.WriteLine($"INFO: Year {idY} added.");
            }
            PaymentCount++;
            Sum += amount;
            var target = Years.Where(y => y.Id == idY).First()
                .Months.Where(m => m.Id == idM)
                .Select(m => amount > 0 ? m.Incomes : m.Expences).First();
            target.Add(new Payment(PaymentCount, userName, amount, description));
            Console.WriteLine($"INFO: {(amount > 0 ? "Income" : "Expence")} added.");
        }

        //Check if year exist in budget.
        private bool IsYearRegistered (int idY, bool all = false)
        {
            if (Years.Where(y => y.Id == idY).Count() != 0 || (all == true && idY == 0))
            {
                return true;
            }
            Console.Clear();
            Console.WriteLine($"ERROR: Year {idY} is not registered.");
            return false;
        }

        //Print registered years.
        private void PrintRegYears()
        {
            Console.Write("Registered years:");
            foreach (Year y in Years)
            {
                Console.Write($" {y.Id}");
            }
            Console.WriteLine("");
        }

        //Print payments in a specified period, 0 = select all
        private void PeriodReport() 
        {
            PrintRegYears();
            int idY = 0, idM = 0, detail = 0, from, to;
            if (!GetIntInput(ref idY, "Year ('0' for all): ", "year", true) ||
                !IsYearRegistered(idY, true) ||
                !GetIntInput(ref idM, "Month (1 - 12, '0' for all): ", "month", true) ||
                !GetIntInput(ref detail, "Show all payments? ('0'-No, '1'-Yes): ", "detail"))
            {
                return;
            }
            from = idY == 0 ? Years.OrderBy(y => y.Id).First().Id : idY;
            to   = idY == 0 ? Years.OrderBy(y => y.Id).Last().Id : idY;
            Console.Clear();
            for (int i = from; i < to + 1; i++) //Iterating years
            {
                Console.WriteLine($"=========================================");
                Console.WriteLine($"YEAR: {i}");
                YearResults(i, idM, detail);
            }
            Console.WriteLine("====================================");
            Console.WriteLine($"[{Name}] Balance: {Sum}");
            Console.WriteLine("====================================");
            Console.Write("Press enter...");
            Console.ReadLine();
            Console.Clear();
        }

        //Print payments in a specified year
        private void YearResults(int idY, int idM, int detail) 
        {
            int from, to, yBalance = 0;
            from = idM == 0 ? 1  : idM;
            to   = idM == 0 ? 13 : idM + 1;
            for (int i = from; i < to; i++) //Iterating months
            {
                var target = Years.Where(y => y.Id == idY).First().Months.Where(m => m.Id == i).First();
                Console.WriteLine("_________________________________________");
                Console.WriteLine($"= {(EMonth)i} =");
                if (detail == 1)
                {
                    Console.WriteLine(" Id | USER     | AMOUNT     | Description");
                }
                int mBalance = PrintPayments(target.Incomes, "INCOMES:", detail);
                mBalance += PrintPayments(target.Expences, "EXPENCES:", detail);
                if (detail == 1)
                {
                    Console.WriteLine("____________________");
                }
                Console.WriteLine($"Month sum: {mBalance}");
                yBalance += mBalance;
            }
            if (idM == 0)
            {
                Console.WriteLine($"=========================================");
                Console.WriteLine($"Year sum: {yBalance}");
            }
        }

        //Print payments
        private int PrintPayments(List<Payment> data, string part, int detail)
        {
            int sum = 0;
            if (detail == 1)
            {
                Console.WriteLine($"{part}");
            }
            foreach (Payment p in data) //Iterating incomes or expences
            {
                sum += p.Amount;
                if (detail == 1)
                {
                    Console.WriteLine(string.Format("{0,4}|{1,10}|{2,12}|{3,0}", p.Id, p.User, p.Amount, p.Description));
                }  
            }
            return sum;
        }

        //Compare 2 years
        private void CompareYears()
        {
            PrintRegYears();
            int idY1 = 0, idY2 = 0, total1 = 0, total2 = 0;
            if (!GetIntInput(ref idY1, "Year1: ", "year") ||
                !IsYearRegistered(idY1) ||
                !GetIntInput(ref idY2, "Year2: ", "year") ||
                !IsYearRegistered(idY2))
            {
                return;
            }
            Console.Clear();
            Console.WriteLine($"         |   {idY1}   |   {idY2}   |Difference|");
            Console.WriteLine("-------------------------------------------");
            for (int i = 1; i < 13; i++) //Iterating months
            {
                var target1 = Years.Where(y => y.Id == idY1).First().Months.Where(m => m.Id == i).First();
                var target2 = Years.Where(y => y.Id == idY2).First().Months.Where(m => m.Id == i).First();
                int sum1 = target1.Expences.Concat(target1.Incomes).Sum(p => p.Amount);
                int sum2 = target2.Expences.Concat(target2.Incomes).Sum(p => p.Amount);
                total1 += sum1;
                total2 += sum2;
                Console.WriteLine(string.Format("{0,9}|{1,10}|{2,10}|{3,10}|", (EMonth)i, sum1, sum2, sum1 - sum2));
            }
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(string.Format("{0,9}|{1,10}|{2,10}|{3,10}|", "Total", total1, total2, total1 - total2));
            Console.WriteLine("-------------------------------------------");
            Console.Write("Press enter...");
            Console.ReadLine();
            Console.Clear();
        }
    }
}