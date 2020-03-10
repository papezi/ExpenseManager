using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManager.BudgetStructure
{
    class Year : IComparable
    {
        public int Id { get; set; }
        public List<Month> Months { get; set; } = new List<Month>();

        public Year(int id)
        {
            Id = id;
            for (int i = 1; i < 13; i++)
            {
                Months.Add(new Month(i));
            }
        }

        public int CompareTo(object obj)
        {
            Year Other = (Year)obj;
            return Id.CompareTo(Other.Id);
        }
    }
}
