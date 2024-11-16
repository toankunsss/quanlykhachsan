using System;
using MySql.Data.MySqlClient;

namespace quanlykhachsan.DatabaseConect
{
    internal class ConnectDb
    {
        private MySqlConnection connection;

        public ConnectDb()
        {
            string server = "localhost";
            string database = "hotelmanage";
            string user = "root";
            string password = "toan@16804"; // Để trống nếu không có mật khẩu
            string connectionString = $"Server={server};Database={database};Uid={user};Pwd={password};";

            connection = new MySqlConnection(connectionString);
        }

        // Hàm mở kết nối
        public void OpenConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                    Console.WriteLine("Kết nối thành công!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi mở kết nối: {ex.Message}");
            }
        }

        // Hàm đóng kết nối
        public void CloseConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Đã đóng kết nối!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đóng kết nối: {ex.Message}");
            }
        }

        // Lấy đối tượng MySqlConnection
        public MySqlConnection GetConnection()
        {
            return connection;
        }

        public bool TestConnection()
        {
            try
            {
                OpenConnection(); // Mở kết nối
                Console.WriteLine("Kiểm tra kết nối: Thành công!");
                CloseConnection(); // Đóng kết nối
                return true; // Kết nối thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kiểm tra kết nối thất bại: {ex.Message}");
                return false; // Kết nối thất bại
            }
        }
    }

}
