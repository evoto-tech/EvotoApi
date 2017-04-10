using FluentMigrator;

namespace Management.Database.Migrations
{
    [Migration(7)]
    public class M007AddPublishedDateToVote : Migration
    {
        public override void Up()
        {
            Alter.Table("Votes").AddColumn("PublishedDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("PublishedDate").FromTable("Votes");
        }
    }
}