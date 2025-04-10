using Model.Manager;
using MySql.Data.MySqlClient;
using UserService.Models.Request;
using UserService.Models.Response;

namespace UserService.Models.Manager
{
    public class ProviderManager : BaseManager
    {

        public List<ProviderResponse> Providers()
        {
            List<ProviderResponse> providerList = new List<ProviderResponse>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM 3RD_PARTY_ENDPOINT";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProviderResponse provider = new ProviderResponse
                            {
                                PARTY_ID = reader.GetInt32("PARTY_ID"),
                                PARTY_API_ENDPOINT = reader.GetString("PARTY_API_ENDPOINT"),
                                PARTY_API_NAME = reader.GetString("PARTY_API_NAME")
                            };
                            providerList.Add(provider);
                        }
                    }
                }
            }
            return providerList;



        }
        public ProviderResponse GetProviderById(int Id)
        {
            ProviderResponse provider = null;

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM 3RD_PARTY_ENDPOINT WHERE PARTY_ID = @Id AND Status = 'A'";
                    using (var sql = new MySqlCommand(query, conn))
                    {
                        sql.Parameters.AddWithValue("@Id", Id);
                        using (MySqlDataReader reader = sql.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                provider = new ProviderResponse();
                                provider.PARTY_ID = reader.GetInt32("PARTY_ID");
                                provider.PARTY_API_ENDPOINT = reader.GetString("PARTY_API_ENDPOINT");
                                provider.PARTY_API_NAME = reader.GetString("PARTY_API_NAME");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetProviderById: " + ex.Message);
                }
            }

            return provider;
        }

        public bool InsertProvider(ProviderRequest provider)
        {
            var IsOkay = false;
            using (MySqlConnection conn = GetConnection())
            {
                try
                {

                    conn.Open();
                    string query = "INSERT INTO 3RD_PARTY_ENDPOINT (PARTY_API_ENDPOINT, PARTY_API_NAME) VALUES (@endpoint, @name)";
                    var sql = new MySqlCommand(query, conn);

                    sql.Parameters.AddWithValue("@endpoint", provider.PARTY_API_ENDPOINT);
                    sql.Parameters.AddWithValue("@name", provider.PARTY_API_NAME);
                    sql.ExecuteNonQuery();
                    IsOkay = true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return IsOkay;

        }

        public bool DeleteProvider(int Id)
        {
            var IsOkay = false;

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE 3RD_PARTY_ENDPOINT SET Status = 'D' WHERE PARTY_ID = @Id";
                    var sql = new MySqlCommand(query, conn);
                    sql.Parameters.AddWithValue("@Id", Id);
                    sql.ExecuteNonQuery();
                    IsOkay = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

            }
            return IsOkay;
        }

        public bool UpdateProvider(ProviderRequest provider)
        {
            var IsOkay = false;
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE 3RD_PARTY_ENDPOINT SET PARTY_API_ENDPOINT = @endpoint, PARTY_API_NAME = @name WHERE PARTY_ID = @Id";
                    var sql = new MySqlCommand(query, conn);
                    sql.Parameters.AddWithValue("@endpoint", provider.PARTY_API_ENDPOINT);
                    sql.Parameters.AddWithValue("@name", provider.PARTY_API_NAME);
                    sql.Parameters.AddWithValue("@Id", provider.PARTY_ID);
                    sql.ExecuteNonQuery();
                    IsOkay = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return IsOkay;
        }

      

    }
}