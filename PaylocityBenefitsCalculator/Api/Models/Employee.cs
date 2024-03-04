using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Dependents = new HashSet<Dependent>();
            EmployeePayments = new HashSet<EmployeePayment>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal SalaryPerHour { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Dependent> Dependents { get; set; }
        public virtual ICollection<EmployeePayment> EmployeePayments { get; set; }
    }
}
