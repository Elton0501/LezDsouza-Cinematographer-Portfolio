using System;

namespace DataModels
{
    public class WorkListItemViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Vimeo { get; set; }
        public bool IsActive { get; set; }
        public bool IsAll { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}