using FluentMigrator;

namespace Management.Database.Migrations
{
    [Migration(2)]
    public class M002CreateAuthThings : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsInt32().Identity().NotNullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("PasswordHash").AsString(255).NotNullable();

            Create.PrimaryKey("PK_Users").OnTable("Users").Column("Id");

            Create.Table("Users_Lockout")
                .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("Users", "Id")
                .WithColumn("Attempts").AsInt32()
                .WithColumn("LockEnd").AsDateTime().Nullable();

            Create.PrimaryKey("PK_Users_Locked").OnTable("Users_Lockout").Column("UserId");

            Insert.IntoTable("Users")
                .Row(
                    new
                    {
                        Email = "admin@evoto.tech",
                        PasswordHash = @"AFm/4QwHKpkiyCG7qEAoTq0/xtMZA/MLqZyZYklkuh9wfz6BzTvOi2bJSa7MVsiFYg=="
                    });
        }

        public override void Down()
        {
            Delete.Table("Users_Lockout");
            Delete.Table("Users");
        }
    }
}