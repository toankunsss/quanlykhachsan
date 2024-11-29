using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using quanlykhachsan.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlykhachsan.DatabaseConect
{

    internal class DatPhongData
    {
        public static void DisplayAndFill(string sql, Guna2DataGridView dgv)
        {
            try
            {
                // Lấy kết nối từ lớp ConnectDb
                using (MySqlConnection con = ConnectDb.GetConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adapter.Fill(dt);
                    dgv.DataSource = dt;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error executing query: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static List<string> GetLoaiPhong()
        {
            List<string> loaiPhongList = new List<string>();
            try
            {
                using (MySqlConnection con = ConnectDb.GetConnection())
                {
                    string sql = "SELECT TenLoai FROM loaiphong"; // Cột LoaiPhong từ bảng Phong
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            loaiPhongList.Add(reader.GetString("TenLoai"));
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error fetching TenLoai: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
           
            return loaiPhongList;
        }
        public static List<Phongmodel> GetValuePhong(string loaiphong, DateTime ngayden, DateTime ngaydi)
        {
            List<Phongmodel> Listphong = new List<Phongmodel>();

            string sql = @"SELECT phong.MaPhong
FROM phong
JOIN loaiphong ON phong.MaLoaiPhong = loaiphong.MaLoaiPhong
WHERE NOT EXISTS (
    SELECT 1
    FROM chitietphieuthue
    JOIN phieuthue ON chitietphieuthue.MaPhieuThue = phieuthue.MaPhieuThue
    WHERE chitietphieuthue.MaPhong = phong.MaPhong
      AND @NgayDen <= phieuthue.NgayDi
      AND @NgayDi >= phieuthue.NgayDen
)
AND loaiphong.TenLoai = @LoaiPhong;"; // Filter by room type

            try
            {
                using (MySqlConnection con = ConnectDb.GetConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(sql, con);

                    // Gắn tham số
                    cmd.Parameters.AddWithValue("@NgayDen", ngayden);
                    cmd.Parameters.AddWithValue("@NgayDi", ngaydi);
                    cmd.Parameters.AddWithValue("@LoaiPhong", loaiphong);

                    // In debug SQL và tham số
                    Console.WriteLine("Debug SQL Query:");
                    Console.WriteLine(sql);
                    Console.WriteLine($"@NgayDen = {ngayden}");
                    Console.WriteLine($"@NgayDi = {ngaydi}");
                    Console.WriteLine($"@LoaiPhong = {loaiphong}");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Phongmodel phong = new Phongmodel
                            {
                                maphong = reader["MaPhong"].ToString(),
                            };
                            Listphong.Add(phong);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error fetching data: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Listphong;
        }

        public static LoaiPhongmodel FillMaphong(string maphong)
        {
            LoaiPhongmodel loaiPhongmodel = null;

            try
            {
                using (MySqlConnection con = ConnectDb.GetConnection())
                {
                    string sql = "SELECT loaiphong.TenLoai, loaiphong.songuoi, loaiphong.dongia " +
                                 "FROM loaiphong, phong " +
                                 "WHERE phong.MaLoaiPhong = loaiphong.Maloaiphong AND phong.MaPhong = @maphong;";

                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@maphong", maphong);  

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Check if there's any result
                        {
                            loaiPhongmodel = new LoaiPhongmodel
                            {
                                tenloai = reader["TenLoai"].ToString(),
                                songuoi = Convert.ToInt32(reader["songuoi"]),
                                dongia = Convert.ToDouble(reader["dongia"])
                            };
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error fetching TenLoai: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return loaiPhongmodel;
        }


    }
}
