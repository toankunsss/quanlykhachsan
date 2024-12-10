using quanlykhachsan.DatabaseConect;
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

namespace quanlykhachsan.Forms
{
    public partial class PhieuDatPhongForm : Form
    {
        public PhieuDatPhongForm()
        {
            InitializeComponent();
            Display();
        }
        private void Display()
        {
            DatPhongData.DisplayAndFill("select phieuthue.MaPhieuThue, phieuthue.MaKH, chitietphieuthue.MaPhong, phieuthue.ngayden, phieuthue.ngaydi from phieuthue, chitietphieuthue, khachhang where khachhang.KhCCCD = phieuthue.MaKH and  chitietphieuthue.MaPhieuThue = phieuthue.MaPhieuThue", guna2DataGridView1);
        }
        private void loadText()
        {
            txtMaDP.Text =string.Empty;
            txtMP.Text = string.Empty;
            txtSoluong.Text = string.Empty;
        }
        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PhieuDatPhongForm_Load(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy hàng hiện tại
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                // Gán dữ liệu từ các ô vào các TextBox
                txtMaDP.Text = row.Cells["MaPhieuThue"].Value?.ToString();
                txtMP.Text = row.Cells["MaPhong"].Value?.ToString();
                txtSoluong.Text = row.Cells["MaKH"].Value?.ToString();
                dateNgayNhan.Value = Convert.ToDateTime(row.Cells["ngayden"].Value);
                dateNgayTra.Value = Convert.ToDateTime(row.Cells["ngaydi"].Value);

            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string id = txtMaDP.Text;
            DatPhongmodel datPhongmodel = new DatPhongmodel();
            datPhongmodel.ngayden = dateNgayNhan.Value;
            datPhongmodel.ngaydi= dateNgayTra.Value;
            PhieuThueData.UpdaStudent(datPhongmodel, id);
            ChiTietPhieuThueModel chiTiet = new ChiTietPhieuThueModel();
            chiTiet.MaPhong = txtMP.Text;
            chitietdata.UpDateChiTiet(chiTiet, id);
            guna2DataGridView1.DataSource = null;
            Display();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            List<string> idsToDelete = new List<string>();

            // Lặp qua các hàng đã chọn
            foreach (DataGridViewRow row in guna2DataGridView1.SelectedRows)
            {
                if (row.Index >= 0)
                {
                    string maPhieuThue = row.Cells["MaPhieuThue"].Value?.ToString();
                    idsToDelete.Add(maPhieuThue);
                }
            }

            // In ra các mã phiếu đã chọn vào console
            Console.WriteLine("Các mã phiếu đã chọn để xóa:");
            foreach (string id in idsToDelete)
            {
                Console.WriteLine(id); // In từng mã phiếu
            }

            // Hỏi người dùng có chắc chắn xóa hay không
            if (idsToDelete.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa các bản ghi đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    foreach (string id in idsToDelete)
                    {
                        // Xóa các bản ghi từ cơ sở dữ liệu
                        PhieuThueData.DeletePhieuThue(id); // Giả sử DeletePhieuThue là phương thức xóa trong PhieuThueData
                        chitietdata.DeleteCTPhieuThue(id); // Giả sử DeleteChiTietPhieuThue là phương thức xóa trong ChiTietPhieuThueData
                    }

                    // Làm mới lại DataGridView sau khi xóa
                    guna2DataGridView1.DataSource = null;
                    Display();

                    // Hiển thị thông báo xóa thành công chỉ một lần sau khi xóa tất cả
                    MessageBox.Show("Xóa thành công", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }
    }
}
