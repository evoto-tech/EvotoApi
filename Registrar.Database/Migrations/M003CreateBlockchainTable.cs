using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(3)]
    public class M003CreateBlockchainTable : Migration
    {
        public override void Up()
        {
            Create.Table("Blockchains")
                .WithColumn("Name").AsString().PrimaryKey()
                .WithColumn("ExpiryDate").AsDateTime()
                .WithColumn("ChainString").AsString().Unique();
        }

        public override void Down()
        {
            Delete.Table("Blockchains");
        }
    }
}