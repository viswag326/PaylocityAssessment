using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class PayPeriodSchedule
    {
        public PayPeriodSchedule()
        {
            EmployeePayments = new HashSet<EmployeePayment>();
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsExecuted { get; set; }

        public virtual ICollection<EmployeePayment> EmployeePayments { get; set; }
    }
}
