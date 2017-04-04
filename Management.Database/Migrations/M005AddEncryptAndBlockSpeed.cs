using FluentMigrator;

namespace Management.Database.Migrations
{
    [Migration(5)]
    public class M005AddEncryptAndBlockSpeed : Migration
    {
        public override void Up()
        {
            Alter.Table("Votes")
                .AddColumn("EncryptResults").AsBoolean().NotNullable().WithDefaultValue(false)
                .AddColumn("BlockSpeed").AsInt32().NotNullable().WithDefaultValue(30);
        }

        public override void Down()
        {
            Delete.Column("EncryptResults").FromTable("Votes");
            Delete.Column("BlockSpeed").FromTable("Votes");
        }
    }
}