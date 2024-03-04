using System;
using System.Collections.Generic;

namespace PaylocityBenefits.Repository.Models
{
    public partial class Dependent
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public int RelationShipId { get; set; }
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual RelationShip RelationShip { get; set; } = null!;
    }
}
