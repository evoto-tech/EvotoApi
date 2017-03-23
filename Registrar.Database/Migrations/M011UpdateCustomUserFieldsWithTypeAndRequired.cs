using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(11)]
    public class M011UpdateCustomUserFieldsWithTypeAndRequired : Migration
    {
        public override void Up()
        {
            Create.Column("Type").OnTable("Users_CustomFields").AsString().NotNullable();
            Create.Column("Required").OnTable("Users_CustomFields").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("Type").FromTable("Users_CustomFields");
            Delete.Column("Required").FromTable("Users_CustomFields");
        }
    }
}