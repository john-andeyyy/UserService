using FluentMigrator;

namespace UserService.Migrations
{
    [Migration(1)]
    public class _00001_3RD_PARTY_ENDPOINT : Migration
    {
        public override void Up()
        {
            Create.Table("3RD_PARTY_ENDPOINT")
                .WithColumn("PARTY_ID").AsInt32().PrimaryKey().Identity()
                .WithColumn("PARTY_API_ENDPOINT").AsString(2700).NotNullable()
                .WithColumn("PARTY_API_NAME").AsString(2700).NotNullable()
                .WithColumn("PARTY_METHOD").AsString(2700).NotNullable()
                .WithColumn("STATUS").AsFixedLengthString(1).WithDefaultValue("A")
                .WithColumn("PARTY_CODE").AsString(100).NotNullable()
                .WithColumn("SERVICE_NAME").AsString(100).NotNullable();

            Insert.IntoTable("3RD_PARTY_ENDPOINT").Row(new
            {
                PARTY_API_ENDPOINT = "http://localhost:5002/api/Todo/UserTodo/",
                PARTY_API_NAME = "Get User Todo by User ID",
                PARTY_METHOD = "GET",
                PARTY_CODE = "GetUserTodos",
                SERVICE_NAME = "TodoService"
            });

            Insert.IntoTable("3RD_PARTY_ENDPOINT").Row(new
            {
                PARTY_API_ENDPOINT = "http://localhost:5002/api/Todo/NewTodo",
                PARTY_API_NAME = "Create User Todo",
                PARTY_METHOD = "POST",
                PARTY_CODE = "NewTodo",
                SERVICE_NAME = "TodoService"
            });

            Insert.IntoTable("3RD_PARTY_ENDPOINT").Row(new
            {
                PARTY_API_ENDPOINT = "http://localhost:5002/api/Todo/Update",
                PARTY_API_NAME = "Update User Todo",
                PARTY_METHOD = "PUT",
                PARTY_CODE = "UpdateTodo",
                SERVICE_NAME = "TodoService"
            });

            Insert.IntoTable("3RD_PARTY_ENDPOINT").Row(new
            {
                PARTY_API_ENDPOINT = "http://localhost:5002/api/Todo/TodoID/",
                PARTY_API_NAME = "Get Todo By Todo Id",
                PARTY_METHOD = "GET",
                PARTY_CODE = "GetTodoById",
                SERVICE_NAME = "TodoService"
            });

            Insert.IntoTable("3RD_PARTY_ENDPOINT").Row(new
            {
                PARTY_API_ENDPOINT = "http://localhost:5002/api/Todo/Delete/",
                PARTY_API_NAME = "Delete User Todo by Todo ID",
                PARTY_METHOD = "DELETE",
                PARTY_CODE = "DeleteTodo",
                SERVICE_NAME = "TodoService"
            });

            Insert.IntoTable("3RD_PARTY_ENDPOINT").Row(new
            {
                PARTY_API_ENDPOINT = "http://localhost:5002/api/Todo/IsCompleted",
                PARTY_API_NAME = "Mark Todo as Completed by Todo id with params",
                PARTY_METHOD = "PUT",
                PARTY_CODE = "MarkAsCompleted",
                SERVICE_NAME = "TodoService"
            });
        }

        public override void Down()
        {
            Delete.Table("3RD_PARTY_ENDPOINT");
        }
    }
}
