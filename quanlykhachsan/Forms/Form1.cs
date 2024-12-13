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
using System.Windows.Forms.DataVisualization.Charting;

namespace quanlykhachsan.Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            soNV.Text = DemNV().ToString();
            soKH.Text = DemKhachHang().ToString();
            soPhong.Text = DemPhong().ToString();
            Load();
            cbxtg.SelectedIndexChanged += new EventHandler(cbxtg_SelectedIndexChanged); // Thêm sự kiện thay đổi lựa chọn
        }

        private void cbxtg_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateChart(); // Gọi hàm cập nhật biểu đồ khi thay đổi lựa chọn
        }

        private void UpdateChart()
        {
            string query = "";
            // Kiểm tra lựa chọn của ComboBox (cbxtg)
            if (cbxtg.SelectedIndex == 0) // Tháng
            {
                query = @"
            SELECT MONTH(ngaythanhtoan) AS Thang, SUM(tongtien) AS TongTien
            FROM hotelmanage.hoadon
            WHERE YEAR(ngaythanhtoan) = 2024  -- Lọc theo năm 2024
            GROUP BY MONTH(ngaythanhtoan)
            ORDER BY Thang;";
            }
            else if (cbxtg.SelectedIndex == 1) // Năm
            {
                query = @"
            SELECT YEAR(ngaythanhtoan) AS Nam, SUM(tongtien) AS TongTien
            FROM hotelmanage.hoadon
            GROUP BY YEAR(ngaythanhtoan)
            ORDER BY Nam;";
            }

            using (MySqlConnection con = ConnectDb.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                chart1.Series["hoadon"].Points.Clear(); // Xóa các điểm cũ trên biểu đồ

                // Khởi tạo mảng để chứa dữ liệu
                Dictionary<int, double> data = new Dictionary<int, double>();

                // Đọc dữ liệu từ cơ sở dữ liệu
                while (reader.Read())
                {
                    if (cbxtg.SelectedIndex == 0) // Tháng
                    {
                        int thang = Convert.ToInt32(reader["Thang"]);
                        double tongTien = Convert.ToDouble(reader["TongTien"]);
                        data[thang] = tongTien;
                    }
                    else if (cbxtg.SelectedIndex == 1) // Năm
                    {
                        int nam = Convert.ToInt32(reader["Nam"]);
                        double tongTien = Convert.ToDouble(reader["TongTien"]);
                        data[nam] = tongTien;
                    }
                }

                // Thêm dữ liệu vào biểu đồ
                if (cbxtg.SelectedIndex == 0) // Hiển thị dữ liệu theo tháng
                {
                    for (int i = 1; i <= 12; i++) // Duyệt qua tất cả 12 tháng
                    {
                        if (data.ContainsKey(i)) // Nếu có dữ liệu cho tháng này
                        {
                            chart1.Series["hoadon"].Points.AddXY($"Tháng {i}", data[i]);
                        }
                        else
                        {
                            // Nếu không có dữ liệu cho tháng này, hiển thị giá trị 0
                            chart1.Series["hoadon"].Points.AddXY($"Tháng {i}", 0);
                        }
                    }
                }
                else if (cbxtg.SelectedIndex == 1) // Hiển thị dữ liệu theo năm
                {
                    // Duyệt qua dữ liệu của các năm
                    foreach (var yearData in data)
                    {
                        chart1.Series["hoadon"].Points.AddXY(yearData.Key.ToString(), yearData.Value);
                    }
                }

                // Thiết lập khoảng cách trên trục Y từ 1 triệu đến 100 triệu, bước nhảy là 10 triệu
                chart1.ChartAreas[0].AxisY.Minimum = 0;  // Giá trị nhỏ nhất là 0
                chart1.ChartAreas[0].AxisY.Maximum = 1000000000;  // Giá trị lớn nhất là 100 triệu
                chart1.ChartAreas[0].AxisY.Interval = 100000000;  // Bước nhảy là 10 triệu
            }
        }

        private void Load()
        {
            // Cấu hình biểu đồ khi form load
            chart1.ChartAreas[0].AxisY.Minimum = 1000000; // Đặt giá trị min cho trục Y
            chart1.ChartAreas[0].AxisY.Maximum = 1000000000; // Đặt giá trị max cho trục Y
            chart1.ChartAreas[0].AxisY.Interval = 10000000; // Interval cho trục Y
            chart1.Series["hoadon"].ChartType = SeriesChartType.Column; // Dạng cột cho biểu đồ
        }
        private int DemNV()
        {
            int soLuongNhanVien = 0; // Biến lưu trữ số lượng nhân viên

            // Kết nối đến cơ sở dữ liệu
            using (MySqlConnection con = ConnectDb.GetConnection())
            {
                try
                {
                    string query = "SELECT COUNT(MaNV) AS SoLuongNhanVien FROM hotelmanage.nhanvien";

                    // Tạo command để thực thi câu lệnh SQL
                    MySqlCommand command = new MySqlCommand(query, con);

                    // Lấy kết quả và chuyển đổi sang kiểu số nguyên
                    soLuongNhanVien = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi xảy ra
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

            return soLuongNhanVien; // Trả về số lượng nhân viên
        }
        private int DemPhong()
        {
            int soLuongPhong = 0; // Biến lưu trữ số lượng phòng

            // Kết nối đến cơ sở dữ liệu
            using (MySqlConnection con = ConnectDb.GetConnection())
            {
                try
                {
                    // Câu truy vấn đếm số lượng phòng
                    string query = "SELECT COUNT(MaPhong) AS SoLuongPhong FROM hotelmanage.phong";

                    // Tạo command để thực thi câu lệnh SQL
                    MySqlCommand command = new MySqlCommand(query, con);

                    // Lấy kết quả và chuyển đổi sang kiểu số nguyên
                    soLuongPhong = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi xảy ra
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

            return soLuongPhong; // Trả về số lượng phòng
        }
        private int DemKhachHang()
        {
            int soLuongKhachHang = 0; // Biến lưu trữ số lượng khách hàng

            // Kết nối đến cơ sở dữ liệu
            using (MySqlConnection con = ConnectDb.GetConnection())
            {
                try
                {
                    // Câu truy vấn đếm số lượng khách hàng
                    string query = "SELECT COUNT(KhCCCD) AS SoLuongKhachHang FROM hotelmanage.khachhang";

                    // Tạo command để thực thi câu lệnh SQL
                    MySqlCommand command = new MySqlCommand(query, con);

                    // Lấy kết quả và chuyển đổi sang kiểu số nguyên
                    soLuongKhachHang = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi xảy ra
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

            return soLuongKhachHang; // Trả về số lượng khách hàng
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
