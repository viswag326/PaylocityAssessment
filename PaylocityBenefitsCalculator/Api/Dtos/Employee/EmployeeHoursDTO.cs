namespace Api.Dtos.Employee
{
    public class EmployeeHoursDTO : EmployeeDto
    {
        
        public DateTime PayPeriodStartDate { get; set; }

        public DateTime PayPeriodEndDate { get; set; }

        public int RegularHours { get; set; }

        public int? OverTimeHours { get; set; }
    }
}
