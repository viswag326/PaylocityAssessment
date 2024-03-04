namespace Api.Dtos.Employee
{
    public class EmployeePaymentDTO : EmployeeDto
    {
        
        public int PayPeriodScheduleDTO { get; set; }

        public decimal BaseBenifits { get; set; }

        public decimal DependentBenefits { get; set; }

        public decimal AddOnBenefits { get; set; }

        public decimal ElederyBenefits { get; set; }

        public decimal FederalTax { get; set; }

        public decimal StateTax { get; set; }

        public decimal TotalEarnings { get; set; }

        public decimal TotalDeductions { get; set; }

        public decimal NetPay { get; set; }

        public int NumberOfDependents { get; set; }

    }
}
