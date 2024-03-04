using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class EmployeePayment
    {
        public int Id { get; set; }
        public int PayPeriodScheduleId { get; set; }
        public int EmployeeId { get; set; }
        public int RegularHours { get; set; }
        public int? OverTimeHours { get; set; }
        public decimal BaseBenefits { get; set; }
        public decimal? DependentBenefits { get; set; }
        public decimal? AddOnBenefits { get; set; }
        public decimal? ElderlyBenefits { get; set; }
        public decimal FederalTax { get; set; }
        public decimal StateTax { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal? NetPay { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual PayPeriodSchedule PayPeriodSchedule { get; set; }
    }
}
