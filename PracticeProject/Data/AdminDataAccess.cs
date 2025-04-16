using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace PracticeProject.Data
{
    public class AdminDataAccess : DataAccess
    {
        public AdminDataAccess(IConfiguration configuration) : base(configuration)
        {
        }

        public bool ValidateAdmin(string username, string password)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT COUNT(*) FROM Admins WHERE Username = @Username AND Password = @Password", con);
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
