using System;
using System.Data;
//using System.Data.SqlClient;
using System.Diagnostics;

using Microsoft.Data.SqlClient;


public class AuthenticateUser
{
    private string connectionString = @"Server=HUYNMEE-LAPTOP-\SQLEXPRESS03;Database=QLSVNhom;Trusted_Connection=True;TrustServerCertificate=True;";

    public (bool, string, string) Login(string manv, string password)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_DangNhap", conn))
            {
                Debug.WriteLine($"MANV: {manv}, Password: {password}");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MANV", manv);
                cmd.Parameters.AddWithValue("@MK", password); // Không mã hóa, để SQL xử lý

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
                    Debug.WriteLine("Error Number: " + ex.Number); // Mã lỗi SQL
                    Debug.WriteLine("Error Procedure: " + ex.Procedure); // Tên stored procedure gây lỗi
                    Debug.WriteLine("Line Number: " + ex.LineNumber); // Dòng lỗi
                    return (false, string.Empty, string.Empty); 
                }

            }
        }
    }
}
