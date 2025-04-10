using MySql.Data.MySqlClient;
using System.Data;
using UserService.Models.Request;
using UserService.Models.Response;


namespace Model.Manager
{

    public class UserManager : BaseManager
    {
        public string IsUnique(UserRegister user)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try
                {
                    string qry = "Select Username, Email from Users Where Username = @Username or Email = @Email";
                    MySqlCommand sql = new MySqlCommand(qry, conn);
                    sql.Parameters.AddWithValue("@Username", user.Username);
                    sql.Parameters.AddWithValue("@Email", user.Email);

                    MySqlDataReader reader = sql.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["Username"].ToString() == user.Username)
                            {
                                return "Username already exists";
                            }
                            else if (reader["Email"].ToString() == user.Email)
                            {
                                return "Email already exists";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Query: {ex.Message}");
                }
                return null;

            }
        }

        
        public List<UserData_WithoutPassword> GetAllUser()
        {
            var Users = new List<UserData_WithoutPassword>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try
                {
                    string query = "SELECT * FROM users WHERE Status = 'A'";
                    MySqlCommand sql = new MySqlCommand(query, conn);
                    MySqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        var user = new UserData_WithoutPassword
                        {
                            Id = reader.GetInt32("Id"),
                            LastName = reader.GetString("LastName"),
                            FirstName = reader.GetString("FirstName"),
                            MiddleName = reader.GetString("MiddleName"),
                            Email = reader.GetString("Email"),
                            Phone = reader.GetString("Phone"),
                            Address = reader.GetString("Address"),
                            Username = reader.GetString("Username")
                        };
                        Users.Add(user);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Query: {ex.Message}");
                }
            }
            return Users;
        }

        public UserData_WithoutPassword GetUserById(int Id)
        {
            UserData_WithoutPassword user = null;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try
                {
                    string query = "SELECT * FROM users WHERE Id = @Id AND Status = 'A'";
                    MySqlCommand sql = new MySqlCommand(query, conn);
                    sql.Parameters.AddWithValue("@Id", Id);
                    MySqlDataReader reader = sql.ExecuteReader();
                    if (reader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        user = new UserData_WithoutPassword
                        {
                            Id = int.Parse(dt.Rows[0]["Id"].ToString()),
                            LastName = dt.Rows[0]["LastName"].ToString().ToString(),
                            FirstName = dt.Rows[0]["FirstName"].ToString(),
                            MiddleName = dt.Rows[0]["MiddleName"].ToString().ToString(),
                            Email = dt.Rows[0]["Email"].ToString(),
                            Phone = dt.Rows[0]["Phone"].ToString(),
                            Address = dt.Rows[0]["Address"].ToString(),
                            Username = dt.Rows[0]["Username"].ToString(),
                        };
                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Query: {ex.Message}");
                }
            }
            return user;
        }

        public bool CreateUser(UserRegister user)
        {
            //UserData_WithoutPassword newUser = null;
           
            var isOK = false;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try
                {
                    string query = "INSERT INTO users (LastName, FirstName, MiddleName, Email, Phone, Address, Username, Password) " +
                                   "VALUES (@LastName, @FirstName, @MiddleName, @Email, @Phone, @Address, @Username, @Password)";
                    MySqlCommand sql = new MySqlCommand(query, conn);
                    sql.Parameters.AddWithValue("@LastName", user.LastName);
                    sql.Parameters.AddWithValue("@FirstName", user.FirstName);
                    sql.Parameters.AddWithValue("@MiddleName", user.MiddleName);
                    sql.Parameters.AddWithValue("@Email", user.Email);
                    sql.Parameters.AddWithValue("@Phone", user.Phone);
                    sql.Parameters.AddWithValue("@Address", user.Address);
                    sql.Parameters.AddWithValue("@Username", user.Username);
                    sql.Parameters.AddWithValue("@Password", user.Password);
                    //int rowsAffected = sql.ExecuteNonQuery();
                    //if (rowsAffected > 0)
                    //{
                    //    newUser = new UserData_WithoutPassword
                    //    {
                    //        LastName = user.LastName,
                    //        FirstName = user.FirstName,
                    //        MiddleName = user.MiddleName,
                    //        Email = user.Email,
                    //        Phone = user.Phone,
                    //        Address = user.Address,
                    //        Username = user.Username
                    //    };
                    //}
                    isOK = true;
                    sql.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Query: {ex.Message}");
                }
            }
            //return newUser;
            return isOK;
        }

        public UserData_WithoutPassword UserLogin(UserLogin user)
        {
            UserData_WithoutPassword loggedInUser = null;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try
                {
                    string query = "SELECT * FROM users WHERE Username = @Username AND Password = @Password AND Status = 'A'";
                    MySqlCommand sql = new MySqlCommand(query, conn);
                    sql.Parameters.AddWithValue("@Username", user.Username);
                    sql.Parameters.AddWithValue("@Password", user.Password);
                    MySqlDataReader reader = sql.ExecuteReader();
                    if (reader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        loggedInUser = new UserData_WithoutPassword
                        {
                            Id = int.Parse(dt.Rows[0]["Id"].ToString()),
                            LastName = dt.Rows[0]["LastName"].ToString(),
                            FirstName = dt.Rows[0]["FirstName"].ToString(),
                            MiddleName = dt.Rows[0]["MiddleName"].ToString(),
                            Email = dt.Rows[0]["Email"].ToString(),
                            Phone = dt.Rows[0]["Phone"].ToString(),
                            Address = dt.Rows[0]["Address"].ToString(),
                            Username = dt.Rows[0]["Username"].ToString()
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Query: {ex.Message}");
                }
            }
            return loggedInUser;
        }

        public bool UpdateUser(UserUpdate User)
        {
            var isOK = false;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try
                {
                    string query = "UPDATE users SET LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName, Email = @Email, Phone = @Phone, Address = @Address, Username = @Username WHERE Id = @Id";
                    MySqlCommand sql = new MySqlCommand(query, conn);
                    sql.Parameters.AddWithValue("@Id", User.Id);
                    sql.Parameters.AddWithValue("@LastName", User.LastName);
                    sql.Parameters.AddWithValue("@FirstName", User.FirstName);
                    sql.Parameters.AddWithValue("@MiddleName", User.MiddleName);
                    sql.Parameters.AddWithValue("@Email", User.Email);
                    sql.Parameters.AddWithValue("@Phone", User.Phone);
                    sql.Parameters.AddWithValue("@Address", User.Address);
                    sql.Parameters.AddWithValue("@Username", User.Username);
                    sql.ExecuteNonQuery();
                    conn.Close();
                    isOK = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Query: {ex.Message}");
                }
            }
            return isOK;


        }

        public bool DeleteUser(int Id)
        {
            var isOK = false;
            using (MySqlConnection conn = GetConnection())
            {
               
                conn.Open();
                string qry = " UPDATE users SET Status = 'D' WHERE Id = @Id";
                MySqlCommand sql = new MySqlCommand(qry, conn);
                sql.Parameters.AddWithValue("@Id", Id);
                try
                {
                    sql.ExecuteNonQuery();
                    isOK = true;
                    conn.Close();
                }catch(Exception ex)
                {
                    Console.WriteLine($"Error Query: {ex.Message}");
                }
                return isOK;
            }
        }
    }
}
