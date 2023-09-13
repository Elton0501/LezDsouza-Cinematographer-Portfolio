using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class AdminIndexModel
    {
        public int WebVisitCount { get; set; }
        public string ImagesVisitCount { get; set; }
        public List<Category> WorkCategory { get; set; }
    }
}
