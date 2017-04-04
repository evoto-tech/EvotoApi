using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(12)]
    public class M0M012CreateUsersView : Migration
    {
        public override void Up()
        {
            Execute.Sql("CREATE VIEW UsersView AS Select * FROM Users");
        }

        public override void Down()
        {
            Execute.Sql("DROP VIEW IF EXISTS UsersView");
        }
    }
}