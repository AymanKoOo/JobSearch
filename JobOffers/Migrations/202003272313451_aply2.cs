namespace JobOffers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aply2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplyForJobs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ApplyForJobs", new[] { "UserId" });
            AlterColumn("dbo.ApplyForJobs", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplyForJobs", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ApplyForJobs", "UserId");
            AddForeignKey("dbo.ApplyForJobs", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
