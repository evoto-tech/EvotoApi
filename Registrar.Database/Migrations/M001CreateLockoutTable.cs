using FluentMigrator;
using FluentMigrator.Runner.Extensions;

namespace Registrar.Database.Migrations
{
    [Migration(1)]
    public class M001CreateLockoutTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users_Locked")
                .WithColumn("UserId").AsInt32().ForeignKey("Users", "Id")
                    .NotNullable().PrimaryKey("PK_Users_Locked")
                .WithColumn("Attempts").AsInt32().NotNullable()
                .WithColumn("LockEnd").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("Users_Locked");
        }
    }
}