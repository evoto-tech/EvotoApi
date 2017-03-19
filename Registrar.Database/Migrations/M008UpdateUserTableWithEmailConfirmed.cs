using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(8)]
    public class M008UpdateUserTableWithEmailConfirmed : Migration
    {
        public override void Up()
        {
            Alter.Table("Users")
                .AddColumn("EmailConfirmed").AsBoolean().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("EmailConfirmed").FromTable("Users");
        }
    }
}