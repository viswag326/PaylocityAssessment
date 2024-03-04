using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class Dependent
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RelationShipId { get; set; }
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual RelationShip RelationShip { get; set; }
    }
}
