using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(12)]
    public class M012UpdateBlockchainTableAddEncryptKey : Migration
    {
        public override void Up()
        {
            Alter.Table("Blockchains")
                .AddColumn("EncryptKey").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("EncryptKey").FromTable("Blockchains");
        }
    }
}