using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManager.BudgetStructure
{
    class Payment
    {
        public int Id { get; set; }
        public string User { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }

        public Payment(int id, string user, int amount, string description)
        {
            Id = id;
            User = user;
            Amount = amount;
            Description = description;
        }
    }
}
