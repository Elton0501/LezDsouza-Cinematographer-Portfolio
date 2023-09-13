using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Images : Base
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        [NotMapped]
        public string TypeName { get; set; }
        [NotMapped]
        public string oldimg { get; set; }
    }
}
