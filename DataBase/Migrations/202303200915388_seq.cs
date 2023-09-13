namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Works", "SeqNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Works", "SeqNo");
        }
    }
}
