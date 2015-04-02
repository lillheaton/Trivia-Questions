namespace TriviaQuestions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferenceToQuestions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "Category_Id", c => c.Int());
            CreateIndex("dbo.Question", "Category_Id");
            AddForeignKey("dbo.Question", "Category_Id", "dbo.Category", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Question", "Category_Id", "dbo.Category");
            DropIndex("dbo.Question", new[] { "Category_Id" });
            DropColumn("dbo.Question", "Category_Id");
        }
    }
}
