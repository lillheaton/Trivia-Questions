namespace TriviaQuestions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAirDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "AirDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Question", "AirDate");
        }
    }
}
