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
    internal class chitietdata
    {
        public static void AddChiTiet(ChiTietPhieuThueModel chiTietPhieuThue)
        {
            string sql = "INSERT INTO chitietphieuthue VALUE (@MaPhieuThue,@MaPhong,@MaPhieuDichVu)";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@MaPhieuThue", MySqlDbType.VarChar).Value = chiTietPhieuThue.MaPhieuThue;
            cmd.Parameters.Add("@MaPhong", MySqlDbType.VarChar).Value = chiTietPhieuThue.MaPhong;
            cmd.Parameters.Add("@MaPhieuDichVu", MySqlDbType.VarChar).Value = chiTietPhieuThue.MaPhieuDichVu;


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
        public static void UpDateChiTiet(ChiTietPhieuThueModel chiTietPhieuThue,String id)
        {
            string sql = "UPDATE chitietphieuthue SET MaPhong =@MaPhong WHERE MaPhieuThue= @MaPhieuThue";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@MaPhieuThue", MySqlDbType.VarChar).Value = id;
            cmd.Parameters.Add("@MaPhong", MySqlDbType.VarChar).Value = chiTietPhieuThue.MaPhong;

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
        public static void DeleteCTPhieuThue(String id)
        {
            string sql = "DELETE FROM chitietphieuthue WHERE MaPhieuThue = @MaPhieuThue";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@MaPhieuThue", MySqlDbType.VarChar).Value = id;
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
