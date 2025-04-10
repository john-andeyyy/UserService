using MySql.Data.MySqlClient;
namespace Model.Manager
{
    public class BaseManager
    {
        public static string ConnectionString { get; set; }
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
