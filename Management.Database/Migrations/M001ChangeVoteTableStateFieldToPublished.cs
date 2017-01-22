using FluentMigrator;
using FluentMigrator.Runner.Extensions;

namespace Management.Database.Migrations
{
    [Migration(1)]
    public class M001ChangeVoteTableStateFieldToPublished : Migration
    {
        public override void Up()
        {

            Delete.Column("State").FromTable("Votes");
            Alter.Table("Votes").AddColumn("Published").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("Published").FromTable("Votes");
            Alter.Table("Votes").AddColumn("State").AsString(255).NotNullable().WithDefaultValue("draft");
        }
    }
}