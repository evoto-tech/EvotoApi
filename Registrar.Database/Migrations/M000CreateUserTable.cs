using FluentMigrator;
using FluentMigrator.Runner.Extensions;

namespace Registrar.Database.Migrations
{
    [Migration(0)]
    public class M000CreateUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsInt32().Identity().NotNullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("PasswordHash").AsString(255).NotNullable();

            Create.PrimaryKey("PK_Users").OnTable("Users")
                .Columns("Id").Clustered();
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}