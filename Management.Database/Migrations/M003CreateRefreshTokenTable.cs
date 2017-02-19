using FluentMigrator;

namespace Management.Database.Migrations
{
    [Migration(3)]
    public class M002CreateRefreshTokenTable : Migration
    {
        public override void Up()
        {
            Create.Table("RefreshTokens")
                .WithColumn("Token").AsString().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("Users", "Id")
                .WithColumn("Ticket").AsCustom("TEXT")
                .WithColumn("Issued").AsDateTime().NotNullable()
                .WithColumn("Expires").AsDateTime().NotNullable();

            Create.PrimaryKey("PK_RefreshTokens").OnTable("RefreshTokens").Column("Token");
        }

        public override void Down()
        {
            Delete.Table("RefreshTokens");
        }
    }
}