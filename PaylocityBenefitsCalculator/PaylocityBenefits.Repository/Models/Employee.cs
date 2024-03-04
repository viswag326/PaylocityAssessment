using System;
using System.Collections.Generic;

namespace PaylocityBenefits.Repository.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Dependents = new HashSet<Dependent>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public byte[] LastName { get; set; } = null!;
        public decimal SalaryPerHour { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Dependent> Dependents { get; set; }
    }
}
