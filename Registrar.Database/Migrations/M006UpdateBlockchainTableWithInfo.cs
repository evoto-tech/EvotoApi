using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(6)]
    public class M006UpdateBlockchainTableWithInfo : Migration
    {
        public override void Up()
        {
            Alter.Table("Blockchains")
                .AddColumn("Info").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Info").FromTable("Blockchains");
        }
    }
}