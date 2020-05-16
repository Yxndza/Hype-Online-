namespace HypestoreFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(nullable: false),
                        Source = c.Int(nullable: false),
                        SourceOther = c.String(),
                        Reason = c.String(),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Subscriptions");
        }
    }
}
