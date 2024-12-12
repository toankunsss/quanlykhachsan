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
    class KhachHangdb
    {
        public static KhachHangModel SearchIdKhachHang(string id)
        {
            KhachHangModel khachHang = null; // Khởi tạo là null
            string sql = "SELECT * FROM khachhang WHERE khCCCD = @KhCCCD";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@KhCCCD", MySqlDbType.VarChar).Value = id;

            try
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        khachHang = new KhachHangModel
                        {
                            khCCCd = reader["khCCCD"].ToString(),
                            tenKH = reader["TenKH"].ToString(),
                            diaChi = reader["DiaChi"].ToString(),
                            gioiTinh = reader["GioiTinh"].ToString(),
                            soDt = Convert.ToString(reader["SoDt"]),
                            ngaySinh = Convert.ToDateTime(reader["ngaySinh"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return khachHang; // Trả về null nếu không tìm thấy
        }

        public static void AddKhachHang(KhachHangModel khachHangModel)
        {
            string sql = "INSERT INTO khachhang VALUE (@KhCCCD,@TenKH,@ngaysinh,@DiaChi,@SoDT,@GioiTinh)";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@KhCCCD", MySqlDbType.VarChar).Value = khachHangModel.khCCCd;
            cmd.Parameters.Add("@TenKH", MySqlDbType.VarChar).Value = khachHangModel.tenKH;
            cmd.Parameters.Add("@ngaysinh", MySqlDbType.Date).Value = khachHangModel.ngaySinh;
            cmd.Parameters.Add("@DiaChi", MySqlDbType.VarChar).Value = khachHangModel.diaChi;
            cmd.Parameters.Add("SoDT", MySqlDbType.Int64).Value = khachHangModel.soDt;
            cmd.Parameters.Add("@GioiTinh", MySqlDbType.VarChar).Value = khachHangModel.gioiTinh;

            try
            {
                cmd.ExecuteNonQuery();

            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Error executing query: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            con.Close();

        }
        public static void UpdaKhachHang(KhachHangModel khachHangModel, String id)
        {
            string sql = "UPDATE khachhang SET TenKH = @TenKH,ngaysinh = @ngaysinh,DiaChi = @DiaChi,SoDT = @SoDT,GioiTinh = @GioiTinh WHERE KhCCCD = @KhCCCD";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@KhCCCD", MySqlDbType.VarChar).Value = id;
            cmd.Parameters.Add("@TenKH", MySqlDbType.VarChar).Value = khachHangModel.tenKH;
            cmd.Parameters.Add("@ngaysinh", MySqlDbType.Date).Value = khachHangModel.ngaySinh;
            cmd.Parameters.Add("@DiaChi", MySqlDbType.VarChar).Value = khachHangModel.diaChi;
            cmd.Parameters.Add("SoDT", MySqlDbType.Int64).Value = khachHangModel.soDt;
            cmd.Parameters.Add("@GioiTinh", MySqlDbType.VarChar).Value = khachHangModel.gioiTinh;

            try
            {
                cmd.ExecuteNonQuery();

            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Error executing query: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            con.Close();
        }
        public static void DeletePhieuThue(String id)
        {
            string sql = "DELETE FROM khachhang WHERE  KhCCCD = @KhCCCD";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@KhCCCD", MySqlDbType.VarChar).Value = id;
            try
            {
                cmd.ExecuteNonQuery();

            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Phiếu thuê cập nhật thất bại: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            con.Close();
        }

    }
}
