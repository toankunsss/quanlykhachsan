using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlykhachsan.Forms
{
    public partial class hoadon : Form
    {
        public hoadon(List<string> danhSachDichVu, List<int> soLuongDichVu, List<double> thanhTienDichVu, double tongTien,double tienphong)
        {
            InitializeComponent();
            dgvHoaDon.Rows.Add("phong", "1", tienphong);
            for (int i = 0; i < danhSachDichVu.Count; i++)
            {
                dgvHoaDon.Rows.Add(danhSachDichVu[i], soLuongDichVu[i], thanhTienDichVu[i]);
            }
            txtTongTien.Text = tongTien.ToString("N0") + " VNĐ";

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel6_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel7_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel6_Click_1(object sender, EventArgs e)
        {

        }
    }
}
