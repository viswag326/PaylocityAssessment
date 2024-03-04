using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class RelationShip
    {
        public RelationShip()
        {
            Dependents = new HashSet<Dependent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Dependent> Dependents { get; set; }
    }
}
