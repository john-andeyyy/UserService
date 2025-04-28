using MySql.Data.MySqlClient;
using System.Data;
using UserService.Models.Response;
namespace Model.Manager
{
    public class BaseManager
    {
        public static string ConnectionString { get; set; }
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }


        //private string TPP_Username = Environment.GetEnvironmentVariable("TPP_Username"); // USER1
        //private string TPP_Password = Environment.GetEnvironmentVariable("TPP_Password"); // USER1
        //private string TPP_URL = Environment.GetEnvironmentVariable("TPP_URL"); // "http://localhost:5000/v2/";


        private string TPP_Username = "USER1";
        private string TPP_Password = "USER1";
        private string TPP_URL = "http://localhost:5000/v2/";


        #region 3rd Party

        public ApiResponse getThirdPartyEndPoint_TPP(int ID)
        {
            Console.WriteLine("Calling TPP");

            // Initialize ApiResponse to return the data
            ApiResponse apiResponse = new ApiResponse();

            string username = this.TPP_Username;
            string password = this.TPP_Password;

            var url = $"{TPP_URL}credential/8/{ID}";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            var authenticationString = $"{username}:{password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);

            try
            {
                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                var responseStr = response.Content.ReadAsStringAsync().Result;
                //Console.WriteLine("Response JSON: " + responseStr);

                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(responseStr, options);

                if (apiResponse != null && apiResponse.Data != null && apiResponse.Data.Count > 0)
                {
                    var firstItem = apiResponse.Data[0];

                    // Optionally, you can print out or use the values in firstItem
                    //Console.WriteLine("Endpoint: " + firstItem.Endpoint);
                    //Console.WriteLine("Method: " + firstItem.Method);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }

            return apiResponse;
        }





        public ThirdParty getThirdPartyEndPointV1(string code)
        {
            ThirdParty api = new ThirdParty();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM 3RD_PARTY_ENDPOINT WHERE PARTY_CODE = @CODE", conn);
                cmd.Parameters.AddWithValue("@CODE", code);

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    reader.Close();

                    api.Party_Id = dt.Rows[0]["PARTY_ID"].ToString();
                    api.Party_API_ENDPOINT = dt.Rows[0]["PARTY_API_ENDPOINT"].ToString();
                    api.Party_API_NAME = dt.Rows[0]["PARTY_API_NAME"].ToString();
                    api.Party_METHOD = dt.Rows[0]["PARTY_METHOD"].ToString();
                    api.Party_CODE = dt.Rows[0]["PARTY_CODE"].ToString();
                }
                conn.Close();
            }

            return api;

        }

        #endregion

    }
}
