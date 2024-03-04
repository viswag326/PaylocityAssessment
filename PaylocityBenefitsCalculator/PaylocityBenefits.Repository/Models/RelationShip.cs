using System;
using System.Collections.Generic;

namespace PaylocityBenefits.Repository.Models
{
    public partial class RelationShip
    {
        public RelationShip()
        {
            Dependents = new HashSet<Dependent>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Dependent> Dependents { get; set; }
    }
}
