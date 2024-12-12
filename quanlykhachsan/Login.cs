using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using quanlykhachsan.DatabaseConect;
using quanlykhachsan.Forms;
using quanlykhachsan.MaHoa;
using quanlykhachsan.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlykhachsan
{
    public partial class Login : Form
    {
        public static String tendangnhap;
        public static NhanVienmodel nhanVienmodel;
        public Login()
        {
            InitializeComponent();
        }
        public static bool CheckLogin(string username, string password)
        {
            tendangnhap = username;

            // Câu lệnh SQL kiểm tra tài khoản và mật khẩu và lấy thông tin chức vụ
            string query = "SELECT MaNV, ChucVu FROM nhanvien WHERE MaNV = @username AND MatKhau = @password";

            // Lấy kết nối đến cơ sở dữ liệu
            MySqlConnection con = ConnectDb.GetConnection();

            // Thực thi câu lệnh SQL
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);


            // Sử dụng MySqlDataReader để đọc dữ liệu trả về từ cơ sở dữ liệu
            MySqlDataReader reader = cmd.ExecuteReader();

            // Kiểm tra nếu có kết quả trả về
            if (reader.HasRows)
            {
                reader.Read();  // Đọc dữ liệu từ kết quả trả về

                // Tạo đối tượng NhanVienmodel chứa thông tin nhân viên
                nhanVienmodel = new NhanVienmodel
                {
                    MaNV = reader.GetString("MaNV"),
                    chucvu = reader.GetString("ChucVu")  // Lấy thông tin chức vụ
                };

                reader.Close();  // Đóng MySqlDataReader
                ConnectDb.CloseConnection(con);  // Đóng kết nối

                return true;  // Đăng nhập thành công
            }
            else
            {
                reader.Close();  // Đóng MySqlDataReader
                ConnectDb.CloseConnection(con);  // Đóng kết nối

                return false;  // Đăng nhập thất bại
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string username = txtLogin.Text;
            string pass = txtPass.Text;
            string passToSha = Mahoa.ComputeSHA256Hash(pass);

            if (CheckLogin(username, passToSha))
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Ẩn form Login trước khi mở form Dashbar
                this.Hide();  // Ẩn form Login

                // Mở form Dashbar dưới dạng modal, nghĩa là chờ Dashbar đóng
                Dashbar datPhong = new Dashbar();
                datPhong.ShowDialog();  // Dùng ShowDialog để chờ form Dashbar

                // Đóng form Login sau khi Dashbar đã được đóng
                this.Close();  // Đóng form Login
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
