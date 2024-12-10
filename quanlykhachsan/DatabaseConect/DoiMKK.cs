using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlykhachsan.DatabaseConect
{
    internal class DoiMKK
    {
        public static bool XacNhanMk(string mk)
        {
            string sql = "SELECT MatKhau FROM nhanvien WHERE MaNV=@MaNV"; // Câu lệnh SQL đúng hơn
            try
            {
                using (MySqlConnection con = ConnectDb.GetConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@MaNV", Login.tendangnhap); // Truyền đúng MaNV
                    MySqlDataReader dr = cmd.ExecuteReader();

                    // Kiểm tra nếu có dữ liệu trả về
                    if (dr.Read())
                    {
                        string matkhau = dr["MatKhau"].ToString();

                        // So sánh mật khẩu trong cơ sở dữ liệu với mật khẩu nhập vào
                        if (matkhau == mk)
                        {
                            return true; // Mật khẩu đúng
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy người dùng với mã nhân viên này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi kết nối: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false; // Mặc định trả về false nếu không tìm thấy mật khẩu đúng
        }
        public static void DoiMk(string mk,string newmk)
        {
            if(!XacNhanMk(mk)) {
                MessageBox.Show("Mật khẩu cũ không khớp! ","Lỗi",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    using (MySqlConnection con = ConnectDb.GetConnection())
                    {
                        // Cập nhật mật khẩu mới
                        string sqlUpdate = "UPDATE nhanvien SET MatKhau=@MatKhau WHERE MaNV=@MaNV";
                        MySqlCommand cmd = new MySqlCommand(sqlUpdate, con);
                        cmd.Parameters.AddWithValue("@MatKhau", newmk); // Cập nhật mật khẩu mới
                        cmd.Parameters.AddWithValue("@MaNV", Login.tendangnhap); // Truyền đúng mã nhân viên

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đổi mật khẩu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không thể thay đổi mật khẩu. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Lỗi kết nối: \n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
