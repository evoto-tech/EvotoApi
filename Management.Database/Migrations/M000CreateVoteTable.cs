using FluentMigrator;
using FluentMigrator.Runner.Extensions;

namespace Management.Database.Migrations
{
    [Migration(0)]
    public class M000CreateVoteTable : Migration
    {
        public override void Up()
        {
            Create.Table("Votes")
                .WithColumn("Id").AsInt32().Identity().NotNullable()
                .WithColumn("CreatedBy").AsInt32().NotNullable()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("CreationDate").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("ExpiryDate").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("State").AsString(255).NotNullable()
                .WithColumn("ChainString").AsString(255);

            Create.PrimaryKey("PK_Votes").OnTable("Votes")
                .Columns("Id").Clustered();
        }

        public override void Down()
        {
            Delete.Table("Votes");
        }
    }
}