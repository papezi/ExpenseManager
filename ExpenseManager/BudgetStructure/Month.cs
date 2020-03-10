using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManager.BudgetStructure
{
    class Month
    {
        public int Id { get; set; }
        public List<Payment> Incomes { get; set; } = new List<Payment>();
        public List<Payment> Expences { get; set; } = new List<Payment>();

        public Month(int id)
        {
            Id = id;
        }
    }
}
