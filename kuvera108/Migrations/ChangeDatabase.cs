namespace kuvera108.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity;
    using System.Reflection;
    using MySql.Data.Entity;
    using System.Data.Entity.Migrations.Sql;
    using System.Data.Entity.Migrations.Model;


    public partial class ChangeDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "logs",
                c => new
                {
                    Login = c.String(),
                    Ip = c.String(),
                    Url = c.String(),
                    Dt = c.DateTime(),
                    Desc = c.String(),
                    Typ = c.String( maxLength:100)
                })
                .Index(t => new { t.Dt, t.Typ});

        }

        public override void Down()
        {
            DropTable("logs");
        }
    }
    public static class StartpdateDb
    {
        public static void RunMigration(this DbContext context, DbMigration migration)
        {
            var prop = migration.GetType().GetProperty("Operations", BindingFlags.NonPublic | BindingFlags.Instance);
            if (prop != null)
            {
                IEnumerable<MigrationOperation> operations = prop.GetValue(migration) as IEnumerable<MigrationOperation>;
                var generator = new MySqlMigrationSqlGenerator();
                var statements = generator.Generate(operations, "2008");
                foreach (MigrationStatement item in statements)
                    context.Database.ExecuteSqlCommand(item.Sql);
            }
        }
    }
}