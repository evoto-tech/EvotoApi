using FluentMigrator;
using FluentMigrator.Runner.Extensions;

namespace Registrar.Database.Migrations
{
    [Migration(7)]
    public class M007CreateUserTokenTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users_Tokens")
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("Purpose").AsString().NotNullable()
                .WithColumn("Token").AsString().NotNullable()
                .WithColumn("Expires").AsDateTime().NotNullable();

            Create.PrimaryKey("PK_Users_Tokens").OnTable("Users_Tokens").Columns("UserId", "Purpose").Clustered();
        }

        public override void Down()
        {
            Delete.Table("Users_Tokens");
        }
    }
}