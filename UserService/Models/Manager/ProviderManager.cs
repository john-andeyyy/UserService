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
                                Party_Id = reader.GetInt32("Party_Id"),
                                Party_API_ENDPOINT = reader.GetString("Party_API_ENDPOINT"),
                                Party_API_NAME = reader.GetString("Party_API_NAME"),
                                Party_METHOD = reader.GetString("Party_API_NAME"),
                                Party_CODE = reader.GetString("Party_CODE")

                                

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
                    string query = "SELECT * FROM 3RD_PARTY_ENDPOINT WHERE Party_Id = @Id AND Status = 'A'";
                    using (var sql = new MySqlCommand(query, conn))
                    {
                        sql.Parameters.AddWithValue("@Id", Id);
                        using (MySqlDataReader reader = sql.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                provider = new ProviderResponse();
                                provider.Party_Id = reader.GetInt32("Party_Id");
                                provider.Party_API_ENDPOINT = reader.GetString("Party_API_ENDPOINT");
                                provider.Party_API_NAME = reader.GetString("Party_API_NAME");
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
                    string query = @"INSERT INTO 3RD_PARTY_ENDPOINT (Party_API_ENDPOINT, Party_API_NAME, PARTY_METHOD,PARTY_CODE) 
VALUES (@endpoint, @name, @Party_METHOD, @Party_CODE)";
                    var sql = new MySqlCommand(query, conn);

                    sql.Parameters.AddWithValue("@endpoint", provider.Party_API_ENDPOINT);
                    sql.Parameters.AddWithValue("@name", provider.Party_API_NAME);
                    sql.Parameters.AddWithValue("@Party_METHOD", provider.Party_METHOD);
                    sql.Parameters.AddWithValue("@Party_CODE", provider.Party_CODE);

                    

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
                    string query = "UPDATE 3RD_PARTY_ENDPOINT SET Status = 'D' WHERE Party_Id = @Id";
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
                    string query = @"UPDATE 3RD_PARTY_ENDPOINT SET Party_API_ENDPOINT = @endpoint, 
                                    Party_API_NAME = @name ,PARTY_METHOD = @PARTY_METHOD,PARTY_CODE = @Party_CODE
                                    WHERE Party_Id = @Id";
                    var sql = new MySqlCommand(query, conn);
                    sql.Parameters.AddWithValue("@endpoint", provider.Party_API_ENDPOINT);
                    sql.Parameters.AddWithValue("@name", provider.Party_API_NAME);
                    sql.Parameters.AddWithValue("@Id", provider.Party_Id);
                    sql.Parameters.AddWithValue("@Party_METHOD", provider.Party_METHOD);
                    sql.Parameters.AddWithValue("@Party_CODE", provider.Party_CODE);


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