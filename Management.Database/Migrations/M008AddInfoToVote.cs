using FluentMigrator;

namespace Management.Database.Migrations
{
    [Migration(8)]
    public class M008AddInfoToVote : Migration
    {
        public override void Up()
        {
            Alter.Table("Votes").AddColumn("Info").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Info").FromTable("Votes");
        }
    }
}