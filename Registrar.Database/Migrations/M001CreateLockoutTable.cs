using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(1)]
    public class M001CreateLockoutTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users_Locked")
                .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("Users", "Id")
                .WithColumn("Attempts").AsInt32()
                .WithColumn("LockEnd").AsDateTime().Nullable();

            Create.PrimaryKey("PK_Users_Locked").OnTable("Users_Locked").Column("UserId");
        }

        public override void Down()
        {
            Delete.Table("Users_Locked");
        }
    }
}