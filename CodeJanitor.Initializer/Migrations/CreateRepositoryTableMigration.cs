using FluentMigrator;

namespace CodeJanitor.Initializer.Migrations;

[Migration(1)]
public sealed class CreateRepositoryTableMigration : Migration
{
    public override void Up()
    {
        Create
            .Table("repositories")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("url").AsString()
            .WithColumn("access_token").AsString();
    }

    public override void Down()
    {
        Delete.Table("repositories");
    }
}