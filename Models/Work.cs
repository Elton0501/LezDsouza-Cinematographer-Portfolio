using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class Work : Base
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Image { get; set; }
        public bool isAll { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string Vimeo { get; set; }
        public int WorkId { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public string oldimg { get; set; }
        public int SeqNo { get; set; }
    }
}
