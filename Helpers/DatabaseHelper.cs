using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System;


//using QLSVNhom.Helpers

public static class DatabaseHelper
{
    private static readonly string connectionString = @"Server=HUYNMEE-LAPTOP-\SQLEXPRESS03;Database=QLSVNhom;Trusted_Connection=True;TrustServerCertificate=True;";

    public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable result = new DataTable();
                    adapter.Fill(result); // Đảm bảo kết nối đã mở
                    return result;
                }
            }
        }

    }

    public static void ExecuteNonQuery(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            conn.Open();
            cmd.Parameters.AddRange(parameters);
            cmd.ExecuteNonQuery();
        }
    }

    public static object ExecuteScalar(string query, params SqlParameter[] parameters)
    {
        object result = null;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                result = cmd.ExecuteScalar();
            }
        }
        return result;
    }

}