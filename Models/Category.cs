using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Category : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isDescription { get; set; }
        public virtual List<Work> Works { get; set; }

        [NotMapped]
        public string VisitCount { get; set; }
    }
}
