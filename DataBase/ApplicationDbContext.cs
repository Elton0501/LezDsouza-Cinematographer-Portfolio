using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("PhotoGraphyDB")
        {

        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Key> Keys { get; set; }
        public virtual DbSet<Newsletter> Newsletters { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Work> Work { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<PageVisitCount> PageVisitCounts { get; set; }
        public virtual DbSet<WebVisitCount> WebVisitCounts { get; set; }
        public virtual DbSet<HomeBanner> HomeBanner { get; set; }

    }
}
