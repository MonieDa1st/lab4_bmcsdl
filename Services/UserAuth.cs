using System;
using System.Data;
//using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Data.SqlClient;


public class AuthenticateUser
{
    private string connectionString = @"Server=HUYNMEE-LAPTOP-\SQLEXPRESS03;Database=QLSVNhom;Trusted_Connection=True;TrustServerCertificate=True;";

    public (bool, string, string) Login(string manv, byte[] hashedPassword)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_DangNhap_MaHoa", conn)) // Đổi tên SP
            {
                Debug.WriteLine($"MANV: {manv}, Password (hashed): {BitConverter.ToString(hashedPassword)}");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MANV", manv);
                cmd.Parameters.AddWithValue("@MK", hashedPassword); // Mật khẩu đã băm SHA-256

                cmd.CommandTimeout = 180; // 3 phút

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string userId = reader["MANV"]?.ToString() ?? string.Empty;
                            string pubKey = reader["PUBKEY"]?.ToString() ?? string.Empty;
                            Debug.WriteLine("Đăng nhập thành công. MANV: " + userId);
                            return (true, userId, pubKey);
                        }
                        else
                        {
                            Debug.WriteLine("Đăng nhập thất bại. Không tìm thấy người dùng.");
                            return (false, string.Empty, string.Empty);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error: " + ex.Message);
                    return (false, string.Empty, string.Empty);
                }
            }
        }
    }
}
