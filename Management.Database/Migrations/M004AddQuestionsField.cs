using FluentMigrator;
using FluentMigrator.Runner.Extensions;

namespace Management.Database.Migrations
{
    [Migration(4)]
    public class M004AddQuestionsField : Migration
    {
        public override void Up()
        {
            Alter.Table("Votes").AddColumn("Questions").AsString().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("Questions").FromTable("Votes");
        }
    }
}