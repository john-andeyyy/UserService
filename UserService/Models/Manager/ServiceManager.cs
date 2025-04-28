using Model.Manager;
using MySql.Data.MySqlClient;
using System.Text;
using UserService.Models.Response;

namespace UserService.Models.Manager
{
    public class ServiceManager : BaseManager
    {

        public ThirdParty GetServiceMethodDetails(string serviceName, string partyCode)
        {
            ThirdParty api = null;
            using MySqlConnection conn = GetConnection();
            conn.Open();

            string query = @"SELECT * FROM 3RD_PARTY_ENDPOINT WHERE SERVICE_NAME = @SERVICE_NAME AND PARTY_CODE = @CODE";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SERVICE_NAME", serviceName);
            cmd.Parameters.AddWithValue("@CODE", partyCode);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                api = new ThirdParty()
                {
                    Party_Id = reader["PARTY_ID"].ToString(),
                    Party_API_ENDPOINT = reader["PARTY_API_ENDPOINT"].ToString(),
                    Party_API_NAME = reader["PARTY_API_NAME"].ToString(),
                    Party_METHOD = reader["PARTY_METHOD"].ToString(),
                    Party_CODE = reader["PARTY_CODE"].ToString()
                };
            }

            return api;
        }

        //public string SendDynamicApi(ThirdParty service, object request = null)
        public string SendDynamicApi(int Credential_ID, object request = null)
        {
            var responseStr = "";
            var tp = getThirdPartyEndPoint_TPP(Credential_ID);

            string ENDPOINT = tp.Data?.FirstOrDefault()?.Endpoint ?? "No endpoint available.";
            //Console.WriteLine("Party_API_ENDPOINT: " + ENDPOINT);
            string METHOD = tp.Data?.FirstOrDefault()?.Method ?? "No Method available.";
            //Console.WriteLine("Party_API_METHOD: " + METHOD);


            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            string requestStr = System.Text.Json.JsonSerializer.Serialize(request);
            var content = new StringContent(requestStr, Encoding.UTF8, "application/json");


            //var url = service.Party_API_ENDPOINT; // from the prams
            var url = ENDPOINT;

            HttpResponseMessage response;
            try
            {
                // if GET DELETE ADD THE ID IN THE URL 
                // GET, DELETE
                switch (METHOD.ToUpper())
                {
                    case "POST":
                        response = client.PostAsJsonAsync(url, request).Result;
                        break;
                    case "PUT":
                        response = client.PutAsJsonAsync(url, request).Result;
                        break;
                    case "DELETE":
                        response = client.DeleteAsync(url).Result;
                        break;
                    default: 
                        response = client.GetAsync(url).Result;
                        break;
                }

                response.EnsureSuccessStatusCode();
                responseStr = response.Content.ReadAsStringAsync().Result;
                if (response.Content.Headers.Contains("Content-Type"))
                {
                    var contentType = response.Content.Headers.GetValues("Content-Type").First();
                }
            }
            catch (Exception ex)
            {
                //APILog("", url, requestStr, ex.Message.ToString(), "");
                Console.WriteLine(ex.Message.ToString());
                responseStr = "ERROR: The Other Service maybe Offline please check!";
            }

            return responseStr;
        }

        public string ApiUsingParams(int Credential_ID, int Id, object request = null)
        {
            var responseStr = "";
            var tp = getThirdPartyEndPoint_TPP(Credential_ID);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            string requestStr = System.Text.Json.JsonSerializer.Serialize(request);
            var content = new StringContent(requestStr, Encoding.UTF8, "application/json");

            string ENDPOINT = tp.Data?.FirstOrDefault()?.Endpoint ?? "No endpoint available.";
            string METHOD = tp.Data?.FirstOrDefault()?.Method ?? "No Method available.";

            var url = ENDPOINT + Id;

            HttpResponseMessage response;
            try
            {

                switch (METHOD.ToUpper())
                {
                    case "POST":
                        response = client.PostAsJsonAsync(url, request).Result;
                        break;
                    case "PUT":
                        response = client.PutAsJsonAsync(url, request).Result;
                        break;
                    case "DELETE":
                        response = client.DeleteAsync(url).Result;
                        break;
                    default:
                        response = client.GetAsync(url).Result;
                        break;
                }

                response.EnsureSuccessStatusCode();
                responseStr = response.Content.ReadAsStringAsync().Result;
                if (response.Content.Headers.Contains("Content-Type"))
                {
                    var contentType = response.Content.Headers.GetValues("Content-Type").First();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return responseStr;
        }




    }


}
