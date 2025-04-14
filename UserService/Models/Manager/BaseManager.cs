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



        #region 3rd Party
        public ThirdParty getThirdPartyEndPoint(string code)
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
