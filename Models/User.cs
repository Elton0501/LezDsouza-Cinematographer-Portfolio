﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User : Base
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Varified { get; set; }
        public Guid RoleId { get; set; }

    }
}
