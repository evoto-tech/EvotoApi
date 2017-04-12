using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(3)]
    public class M003CreateBlockchainTable : Migration
    {
        public override void Up()
        {
            Create.Table("Blockchains")
                .WithColumn("Name").AsString()
                .WithColumn("ExpiryDate").AsDateTime()
                .WithColumn("ChainString").AsString().PrimaryKey();
        }

        public override void Down()
        {
            Delete.Table("Blockchains");
        }
    }
}