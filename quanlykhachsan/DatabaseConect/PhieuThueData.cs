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
     class PhieuThueData
    {
        public static void AddDatPhong(DatPhongmodel datPhongmodel)
        {
            string sql = "INSERT INTO phieuthue VALUE (@MaPhieuThue,@MaKH,@ngayden,@ngaydi)";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@MaPhieuThue", MySqlDbType.VarChar).Value = datPhongmodel.MaPhieuThue;
            cmd.Parameters.Add("@MaKH", MySqlDbType.VarChar).Value = datPhongmodel.MaKh;
            cmd.Parameters.Add("@ngayden", MySqlDbType.Date).Value = datPhongmodel.ngayden;
            cmd.Parameters.Add("@ngaydi", MySqlDbType.Date).Value = datPhongmodel.ngaydi;
            try
            {
                cmd.ExecuteNonQuery();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi truy vấn: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            con.Close();

        }
        public static void UpdaStudent(DatPhongmodel datPhongmodel, String id)
        {
            string sql = "UPDATE phieuthue SET MaPhieuThue = @MaPhieuThue, MaKH= @MaKH,ngayden = @ngayden,@ngaydi WHERE MaPhieuThue = @MaPhieuThue";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@MaKH", MySqlDbType.VarChar).Value = datPhongmodel.MaKh;
            cmd.Parameters.Add("@ngayden", MySqlDbType.Date).Value = datPhongmodel.ngayden;
            cmd.Parameters.Add("@ngaydi", MySqlDbType.Date).Value = datPhongmodel.ngaydi;
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Add Successfully. ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Phiếu thuê cập nhật thất bại: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            con.Close();
        }
        public static void DeletePhieuThue(String id)
        {
            string sql = "DELETE FROM phieuthu WHERE MaPhieuThue = @MaPhieuThue";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@MaPhieuThu", MySqlDbType.VarChar).Value = id;
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Add Successfully. ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Phiếu thuê cập nhật thất bại: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            con.Close();
        }
        public static List<DatPhongmodel> SelectAll()
        {
            List<DatPhongmodel> datPhongmodels = new List<DatPhongmodel>();

            string sql = "SELECT * FROM phieuthue";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Retrieving all necessary fields from the database
                    string maPhieuThue = reader["MaPhieuThue"].ToString();
                    string tenKH = "";  // Assuming you have this column in your database
                    string maKh = reader["MaKH"].ToString();
                    DateTime ngayDen = Convert.ToDateTime(reader["ngayden"]);
                    DateTime ngayDi = Convert.ToDateTime(reader["ngaydi"]);

                    // Creating the DatPhongmodel instance with all the required parameters
                    DatPhongmodel datPhongmodel = new DatPhongmodel(maPhieuThue, tenKH, maKh, ngayDen, ngayDi);

                    // Adding the created model to the list
                    datPhongmodels.Add(datPhongmodel);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error executing query: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            return datPhongmodels;
        }


    }
}
