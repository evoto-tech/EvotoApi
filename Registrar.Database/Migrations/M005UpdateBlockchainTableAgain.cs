using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(5)]
    public class M005UpdateBlockchainTableAgain : Migration
    {
        public override void Up()
        {
            Alter.Table("Blockchains")
                .AddColumn("WalletId").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("WalletId").FromTable("Blockchains");
        }
    }
}