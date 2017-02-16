using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(4)]
    public class M004UpdateBlockchainTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Blockchains")
                .AddColumn("Port").AsInt16();
        }

        public override void Down()
        {
            Delete.Column("Port").FromTable("Blockchains");
        }
    }
}