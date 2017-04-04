using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(13)]
    public class M013UpdateBlockchainTableAddEncryptKey : Migration
    {
        public override void Up()
        {
            Alter.Table("Blockchains")
                .AddColumn("EncryptKey").AsString(int.MaxValue).Nullable();
        }

        public override void Down()
        {
            Delete.Column("EncryptKey").FromTable("Blockchains");
        }
    }
}