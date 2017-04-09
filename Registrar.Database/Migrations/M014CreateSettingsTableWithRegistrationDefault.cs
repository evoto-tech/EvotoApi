﻿using FluentMigrator;

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

            Create.PrimaryKey("PK_Settings").OnTable("Settings")
                .Columns("Name");

            Insert.IntoTable("Settings")
                .Row(
                    new
                    {
                        Name = "User Registration",
                        Value = "true"
                    });
        }

        public override void Down()
        {
            Delete.Table("Settings");
        }
    }
}