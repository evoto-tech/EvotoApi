using System;
using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(9)]
    public class M009UpdateUserTokenTableWithCreated : Migration
    {
        public override void Up()
        {
            Create.Column("Created").OnTable("Users_Tokens").AsDateTime().WithDefaultValue(DateTime.UtcNow);
        }

        public override void Down()
        {
            Delete.Column("Created").FromTable("Users_Tokens");
        }
    }
}