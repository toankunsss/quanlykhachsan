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

namespace quanlykhachsan.Forms
{
    public partial class LapHoaDon : Form
    {
        public LapHoaDon()
        {
            InitializeComponent();
            display();
        }
        private void display()
        {
            DatPhongData.DisplayAndFill("select phieuthue.MaKH, phieuthue.MaPhieuThue, khachhang.TenKH from phieuthue , khachhang where phieuthue.MaKH=khachhang.KhCCCD;", guna2DataGridView1);

        }
        private void TinhTongTien()
        {
            decimal tongTien = 0;

            // Tính tổng tiền từ DataGridView3
            foreach (DataGridViewRow row in guna2DataGridView3.Rows)
            {
                if (row.Cells["ThanhTien1"].Value != null)
                {
                    tongTien += Convert.ToDecimal(row.Cells["ThanhTien1"].Value);
                }
            }

            // Tính tổng tiền từ DataGridView2
            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    tongTien += Convert.ToDecimal(row.Cells["ThanhTien"].Value);
                }
            }

            // Hiển thị tổng tiền lên TextBox với định dạng số tiền và đơn vị VNĐ
            txtTongTien.Text = tongTien.ToString("N0") + " VNĐ"; // Định dạng tiền tệ và thêm đơn vị
        }

        private void UpdateSTT()
        {
            for (int i = 0; i < guna2DataGridView2.Rows.Count; i++)
            {
                guna2DataGridView2.Rows[i].Cells["STT"].Value = (i + 1).ToString(); 
            }
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void LapHoaDon_Load(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Lấy giá trị MaPhieuThue từ hàng được chọn
                string maPhieuThue = guna2DataGridView1.Rows[e.RowIndex].Cells["MaPhieuThue"].Value.ToString();

                // Câu lệnh SQL mới dựa trên MaPhieuThue
                string query = $@"
            SELECT phong.MaPhong, loaiphong.DonGia, phieuthue.NgayDen, phieuthue.NgayDi
            FROM chitietphieuthue
            INNER JOIN phong ON chitietphieuthue.MaPhong = phong.MaPhong
            INNER JOIN loaiphong ON phong.MaLoaiPhong = loaiphong.MaLoaiPhong
            INNER JOIN phieuthue ON chitietphieuthue.MaPhieuThue = phieuthue.MaPhieuThue
            WHERE chitietphieuthue.MaPhieuThue = '{maPhieuThue}';
        ";

                // Hiển thị dữ liệu vào DataGridView hoặc thực hiện các thao tác khác
                DatPhongData.DisplayAndFill(query, guna2DataGridView3); // guna2DataGridView2 là DataGridView khác để hiển thị kết quả
                string queryDV = $@"SELECT 
    dichvu.TenDV, 
    dichvu.DonGia, 
    phieudichvu.ThanhTien, 
    phieudichvu.SoLuong
FROM 
    chitietphieuthue
INNER JOIN 
    phieudichvu ON chitietphieuthue.MaPhieuDichVu = phieudichvu.MaDV
INNER JOIN 
    dichvu ON phieudichvu.MaDV = dichvu.MaDV
WHERE 
    chitietphieuthue.MaPhieuThue = '{maPhieuThue}';";
                DatPhongData.DisplayAndFill(queryDV, guna2DataGridView2);
                UpdateSTT();
                TinhThanhTien();
                TinhTongTien();
            }



        }
        private void TinhThanhTien()
        {
            foreach (DataGridViewRow row in guna2DataGridView3.Rows)
            {
                // Lấy dữ liệu ngày đến, ngày đi và đơn giá
                DateTime ngayDen = Convert.ToDateTime(row.Cells["NgayDen"].Value);
                DateTime ngayDi = Convert.ToDateTime(row.Cells["NgayDi"].Value);
                decimal donGia = Convert.ToDecimal(row.Cells["DonGia"].Value);

                // Tính số ngày và thành tiền
                int soNgay = (ngayDi - ngayDen).Days;
                decimal thanhTien = soNgay * donGia;

                // Thêm vào cột mới hoặc hiển thị
                row.Cells["ThanhTien1"].Value = thanhTien;
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            List<string> danhSachDichVu = new List<string>();
            List<int> soLuongDichVu = new List<int>();
            List<double> thanhTienDichVu = new List<double>();
            double totalAmount = 0;

            // Lấy thông tin dịch vụ từ DataGridView2 (Dịch vụ và thành tiền)
            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                if (row.Cells["TenDV"].Value != null && row.Cells["SoLuong"].Value != null && row.Cells["ThanhTien"].Value != null)
                {
                    string tenDV = row.Cells["TenDV"].Value.ToString();  // Lấy tên dịch vụ
                    int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);  // Lấy số lượng dịch vụ
                    double thanhTien = Convert.ToDouble(row.Cells["ThanhTien"].Value);  // Lấy thành tiền dịch vụ

                    // Thêm vào danh sách
                    danhSachDichVu.Add(tenDV);
                    soLuongDichVu.Add(soLuong);
                    thanhTienDichVu.Add(thanhTien);

                    // Cộng thành tiền vào tổng tiền
                    totalAmount += thanhTien;
                }
            }
            // Lấy thông tin phòng từ DataGridView3 (Phòng)
            double tienphong=0;
            foreach (DataGridViewRow row in guna2DataGridView3.Rows)
            {
                if (row.Cells["ThanhTien1"].Value != null)
                {
                    tienphong = Convert.ToDouble(row.Cells["ThanhTien1"].Value);
                    totalAmount += Convert.ToDouble(row.Cells["ThanhTien1"].Value);  // Thêm thành tiền phòng vào tổng tiền
                }
            }

            // Hiển thị tổng tiền lên TextBox với định dạng tiền tệ và đơn vị VNĐ
            txtTongTien.Text = totalAmount.ToString("N0") + " VNĐ"; // Định dạng tiền tệ

            // Truyền dữ liệu vào form Hóa Đơn và hiển thị
            hoadon hoaDonForm = new hoadon(danhSachDichVu, soLuongDichVu, thanhTienDichVu, totalAmount,tienphong);
            hoaDonForm.Show();

            // Thông báo thanh toán thành công (hoặc thực hiện các thao tác tiếp theo)
            MessageBox.Show("Hóa đơn đã được thanh toán thành công!", "Thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void SearchInDataGridView(string keyword)
        {
            // Nếu không có từ khóa tìm kiếm, hiển thị tất cả dữ liệu
            if (string.IsNullOrEmpty(keyword))
            {
                display();  // Gọi lại hàm display để hiển thị tất cả dữ liệu
            }
            else
            {
                // Xây dựng câu lệnh SQL để tìm kiếm
                string query = $@"
            SELECT phieuthue.MaKH, phieuthue.MaPhieuThue, khachhang.TenKH 
            FROM phieuthue, khachhang 
            WHERE phieuthue.MaKH = khachhang.KhCCCD
            AND (phieuthue.MaPhieuThue LIKE '%{keyword}%' OR khachhang.TenKH LIKE '%{keyword}%')";

                // Thực hiện truy vấn và cập nhật DataGridView
                DatPhongData.DisplayAndFill(query, guna2DataGridView1);
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string keyword = ten.Text;
            SearchInDataGridView(keyword);
        }
    }
}
