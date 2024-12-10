using MySql.Data.MySqlClient;
using quanlykhachsan.DatabaseConect;
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
    public partial class DoiMatKhau : Form
    {
        public DoiMatKhau()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string oldPass = textOldPass.Text;
            string newPass = textNewPass.Text;
            string relplay = textReplay.Text;
            if(newPass != relplay)
            {
                MessageBox.Show("Vui lòng nhập lại chính xác mật khẩu mới! ", "Lỗi ",MessageBoxButtons.OK);
                textNewPass.Text = "";
                textReplay.Text = "";
                return;
            }
            else
            {
                DoiMKK.DoiMk(oldPass, newPass);
                MessageBox.Show("Đổi mật khẩu thành công", "Thông báo ", MessageBoxButtons.OK);
                Application.Exit();

                // Mở lại form đăng nhập
                Login login = new Login();
                login.ShowDialog();



            }
        }

        private void DoiMatKhau_Load(object sender, EventArgs e)
        {

        }
    }
}
