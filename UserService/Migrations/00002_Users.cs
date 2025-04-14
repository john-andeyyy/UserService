using FluentMigrator;

namespace UserService.Migrations
{
    [Migration(2)]
    public class _00002_Users : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("LastName").AsString(100).NotNullable()
                .WithColumn("FirstName").AsString(100).NotNullable()
                .WithColumn("MiddleName").AsString(100).Nullable()
                .WithColumn("Email").AsString(255).NotNullable().Unique()
                .WithColumn("Phone").AsString(50).NotNullable()
                .WithColumn("Address").AsCustom("TEXT").NotNullable()
                .WithColumn("Username").AsString(100).NotNullable().Unique()
                .WithColumn("Password").AsString(255).NotNullable()
                .WithColumn("Status").AsFixedLengthString(1).WithDefaultValue("A")
                .WithColumn("CreatedAt").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);

            Insert.IntoTable("users").Row(new
            {
                LastName = "Dela Cruz",
                FirstName = "Juan",
                MiddleName = "Reyes",
                Email = "juan.delacruz@example.com",
                Phone = "09171234567",
                Address = "123 Manila St.",
                Username = "juandelacruz",
                Password = "password123"
            });

            Insert.IntoTable("users").Row(new
            {
                LastName = "Santos",
                FirstName = "Maria",
                MiddleName = "Luz",
                Email = "maria.santos@example.com",
                Phone = "09281234567",
                Address = "456 Makati Ave.",
                Username = "mariasantos",
                Password = "pass456"
            });

            Insert.IntoTable("users").Row(new
            {
                LastName = "Garcia",
                FirstName = "Jose",
                MiddleName = "Luis",
                Email = "jose.garcia@example.com",
                Phone = "09391234567",
                Address = "789 Quezon Blvd.",
                Username = "josegarcia",
                Password = "mysecurepass"
            });

            Insert.IntoTable("users").Row(new
            {
                LastName = "Reyes",
                FirstName = "Anna",
                MiddleName = "",
                Email = "anna.reyes@example.com",
                Phone = "09451234567",
                Address = "101 Pasig Rd.",
                Username = "annareyes",
                Password = "12345678"
            });

            Insert.IntoTable("users").Row(new
            {
                LastName = "Torres",
                FirstName = "Mark",
                MiddleName = "Anthony",
                Email = "mark.torres@example.com",
                Phone = "09561234567",
                Address = "202 Taguig Dr.",
                Username = "marktorres",
                Password = "letmein"
            });
        }

        public override void Down()
        {
            Delete.Table("users");
        }
    }
}
