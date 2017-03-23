using FluentMigrator;
using FluentMigrator.Runner.Extensions;

namespace Registrar.Database.Migrations
{
    [Migration(10)]
    public class M010CreateCustomUserFields : Migration
    {
        public override void Up()
        {
            Create.Table("Users_CustomFields")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString()
                .WithColumn("Validation").AsString();

            Create.Table("Users_CustomValues")
                .WithColumn("UserId").AsInt32().ForeignKey("Users", "Id")
                .WithColumn("CustomFieldId").AsInt32().ForeignKey("Users_CustomFields", "Id")
                .WithColumn("Value").AsString();

            Create.PrimaryKey().OnTable("Users_CustomValues").Columns("UserId", "CustomFieldId").Clustered();
        }

        public override void Down()
        {
            Delete.Table("Users_CustomValues");
            Delete.Table("Users_CustomFields");
        }
    }
}