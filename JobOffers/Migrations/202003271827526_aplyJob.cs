namespace JobOffers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aplyJob : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplyForJobs", "ApplyDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplyForJobs", "ApplyDate", c => c.String());
        }
    }
}
