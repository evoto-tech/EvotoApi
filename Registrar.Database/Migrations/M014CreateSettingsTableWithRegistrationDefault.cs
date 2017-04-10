using Common;
using FluentMigrator;

namespace Registrar.Database.Migrations
{
    [Migration(14)]
    public class M014CreateSettingsTableWithRegistrationDefault : Migration
    {
        public override void Up()
        {
            Create.Table("Settings")
                .WithColumn("Name").AsString(255).PrimaryKey().NotNullable()
                .WithColumn("Value").AsString(255).NotNullable();

            Insert.IntoTable("Settings")
                .Row(
                    new
                    {
                        Name = RegiSettings.REGISTER_ENABLED,
                        Value = RegiSettings.REGISTER_ENABLED_DEFAULT
                    });
        }

        public override void Down()
        {
            Delete.Table("Settings");
        }
    }
}