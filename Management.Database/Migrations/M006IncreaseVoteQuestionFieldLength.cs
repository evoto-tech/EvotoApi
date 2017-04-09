using FluentMigrator;

namespace Management.Database.Migrations
{
    [Migration(6)]
    public class M006IncreaseVoteQuestionFieldLength : Migration
    {
        public override void Up()
        {
            Alter.Table("Votes")
                .AlterColumn("Questions").AsString(4000);
        }

        public override void Down()
        {
            Alter.Table("Votes")
                .AlterColumn("Questions").AsString(255);
        }
    }
}