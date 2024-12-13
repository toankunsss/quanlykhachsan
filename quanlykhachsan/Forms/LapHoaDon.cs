using MySql.Data.MySqlClient;
using quanlykhachsan.DatabaseConect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
            DatPhongData.DisplayAndFill("SELECT phieuthue.MaKH, phieuthue.MaPhieuThue, khachhang.TenKH\r\nFROM phieuthue\r\nJOIN khachhang ON phieuthue.MaKH = khachhang.KhCCCD\r\nJOIN chitietphieuthue ON chitietphieuthue.MaPhieuThue = phieuthue.MaPhieuThue\r\nWHERE chitietphieuthue.trangthai IS NULL;", guna2DataGridView1);

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
                string queryDV = $@"
SELECT 
    dichvu.TenDV, 
    dichvu.DonGia, 
    phieudichvu.ThanhTien, 
    phieudichvu.SoLuong
FROM 
    phieudichvu
JOIN 
    dichvu ON dichvu.MaDV = phieudichvu.MaDV
JOIN 
    phieuthue ON phieudichvu.MaPhieuThue = phieuthue.MaPhieuThue
WHERE 
    phieudichvu.MaPhieuThue = '{maPhieuThue}';";
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
            string maPhieuThue = ""; // Khai báo maPhieuThue trước

            if (guna2DataGridView1.SelectedRows.Count > 0) // Kiểm tra xem có hàng nào được chọn không
            {
                maPhieuThue = guna2DataGridView1.SelectedRows[0].Cells["MaPhieuThue"].Value.ToString();
                string updateQuery = "UPDATE chitietphieuthue SET trangthai = 'OK' WHERE MaPhieuThue = @MaPhieuThue";

                using (MySqlConnection connection = ConnectDb.GetConnection())
                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@MaPhieuThue", maPhieuThue);
                    command.ExecuteNonQuery();
                }

                display(); // Làm mới DataGridView sau khi cập nhật
            }

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
            double tienphong = 0;
            foreach (DataGridViewRow row in guna2DataGridView3.Rows)
            {
                if (row.Cells["ThanhTien1"].Value != null)
                {
                    tienphong = Convert.ToDouble(row.Cells["ThanhTien1"].Value);
                    totalAmount += tienphong;  // Thêm thành tiền phòng vào tổng tiền
                }
            }

            ThemHoaDon(maPhieuThue, totalAmount); // Gọi phương thức với maPhieuThue đã khai báo

            // Hiển thị tổng tiền lên TextBox với định dạng tiền tệ và đơn vị VNĐ
            txtTongTien.Text = totalAmount.ToString("N0") + " VNĐ"; // Định dạng tiền tệ

            // Truyền dữ liệu vào form Hóa Đơn và hiển thị
            if (guna2CheckBox1.Checked)
            {
                hoadon hoaDonForm = new hoadon(danhSachDichVu, soLuongDichVu, thanhTienDichVu, totalAmount, tienphong);
                hoaDonForm.Show();
            }

            // Thông báo thanh toán thành công
            MessageBox.Show("Hóa đơn đã được thanh toán thành công!", "Thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SearchInDataGridView(string keyword)
        {

            if (string.IsNullOrEmpty(keyword))
            {
                display();  // Gọi lại hàm display để hiển thị tất cả dữ liệu
            }
            else
            {
                // Xây dựng câu lệnh SQL để tìm kiếm
                string query = $@"SELECT 
    phieuthue.MaKH, 
    phieuthue.MaPhieuThue, 
    khachhang.TenKH
FROM 
    phieuthue
JOIN 
    khachhang ON phieuthue.MaKH = khachhang.KhCCCD
JOIN 
    chitietphieuthue ON chitietphieuthue.MaPhieuThue = phieuthue.MaPhieuThue
WHERE 
    chitietphieuthue.trangthai IS NULL
    AND (phieuthue.MaPhieuThue LIKE '%{keyword}%' OR khachhang.TenKH LIKE '%{keyword}%');
";

                // Thực hiện truy vấn và cập nhật DataGridView
                DatPhongData.DisplayAndFill(query, guna2DataGridView1);
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string keyword = ten.Text;
            SearchInDataGridView(keyword);
        }
        private void ThemHoaDon(string maPhieuThue, double tongTien)
        {
            using (MySqlConnection con = ConnectDb.GetConnection())
            {
                try
                {
                    // Câu truy vấn lấy thời gian từ bảng phieuthue
                    string queryLayThoiGian = "SELECT NgayDi FROM phieuthue WHERE MaPhieuThue = @MaPhieuThue";

                    // Biến lưu thời gian
                    DateTime thoiGian;

                    // Lấy thời gian
                    using (MySqlCommand cmdLayThoiGian = new MySqlCommand(queryLayThoiGian, con))
                    {
                        cmdLayThoiGian.Parameters.AddWithValue("@MaPhieuThue", maPhieuThue);
                        object result = cmdLayThoiGian.ExecuteScalar();
                        if (result != null)
                        {
                            thoiGian = Convert.ToDateTime(result);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thời gian NgayDi cho mã phiếu thuê này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Câu truy vấn chèn dữ liệu vào bảng hoadon
                    string queryInsert = "INSERT INTO hoadon (MaPhieuThue, TongTien, ngayThanhtoan) VALUES (@MaPhieuThue, @TongTien, @ThoiGian)";

                    // Thực thi chèn dữ liệu
                    using (MySqlCommand cmdInsert = new MySqlCommand(queryInsert, con))
                    {
                        cmdInsert.Parameters.AddWithValue("@MaPhieuThue", maPhieuThue);
                        cmdInsert.Parameters.AddWithValue("@TongTien", tongTien);
                        cmdInsert.Parameters.AddWithValue("@ThoiGian", thoiGian);
                        cmdInsert.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chèn hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }


}
