using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class WebCharts
    {
        public string Key { get; set; }
        public int Value { get; set; }
    }
    public class ImagesCount
    {
        public string day { get; set; }
        public int Count { get; set; }
    }
    public class WVCountPerDay
    {
        public string day { get; set; }
        public int Count { get; set; }
    }
    public class UserOrdersCount
    {
        public int UId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string City { get; set; }
        public int Count { get; set; }
    }
    public class OrderItemsCount
    {
        public int PId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
