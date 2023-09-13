using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class ImagesDataModel
    {
        public List<Images> Images { get; set; }
        public int? pageNo { get; set; }
        public Pager Pager { get; set; }
    }
}
