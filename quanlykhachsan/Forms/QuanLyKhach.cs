using ClosedXML.Excel;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using quanlykhachsan.DatabaseConect;
using quanlykhachsan.MaHoa;
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
    public partial class QuanLyKhach : Form
    {
        public QuanLyKhach()
        {
            InitializeComponent();
        }


        private void LoadDataIntoDataGridView()
        {
            // Lấy kết nối đến cơ sở dữ liệu
            MySqlConnection connection = ConnectDb.GetConnection();
            if (connection == null)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                return;
            }

            // Chuẩn bị câu lệnh SQL để lấy dữ liệu
            string query = @"SELECT 
                        KhCCCD, 
                        TenKH, 
                        NgaySinh, 
                        DiaChi, 
                        SoDT, 
                        GioiTinh
                    FROM 
                        khachhang";

            using (var command = new MySqlCommand(query, connection))
            {
                try
                {
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);  // Lấp đầy DataTable với dữ liệu từ cơ sở dữ liệu

                    // Gán dữ liệu cho DataGridView
                    guna2DataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
                }
                finally
                {
                    // Đảm bảo đóng kết nối sau khi thực thi
                    ConnectDb.CloseConnection(connection);
                }
            }
        }



        private void exportDataToExcel()
        {
            try
            {
                // Tạo một đối tượng SaveFileDialog để chọn đường dẫn lưu file Excel
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx"; // Chỉ cho phép chọn file Excel
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    // Tạo một workbook mới và một worksheet mới
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("NhanVien");

                        // Lặp qua các cột của DataGridView và thêm vào worksheet
                        for (int col = 0; col < guna2DataGridView1.Columns.Count; col++)
                        {
                            worksheet.Cell(1, col + 1).Value = guna2DataGridView1.Columns[col].HeaderText; // Thêm tiêu đề cột
                        }

                        // Lặp qua các hàng và thêm dữ liệu vào worksheet
                        for (int row = 0; row < guna2DataGridView1.Rows.Count; row++)
                        {
                            for (int col = 0; col < guna2DataGridView1.Columns.Count; col++)
                            {
                                if (guna2DataGridView1.Rows[row].Cells[col].Value != null)
                                {
                                    worksheet.Cell(row + 2, col + 1).Value = guna2DataGridView1.Rows[row].Cells[col].Value.ToString();
                                }
                            }
                        }

                        // Thêm định dạng bảng Excel
                        var range = worksheet.Range(1, 1, guna2DataGridView1.Rows.Count + 1, guna2DataGridView1.Columns.Count);
                        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                        // Lưu workbook ra file Excel
                        workbook.SaveAs(filePath);
                        MessageBox.Show("Dữ liệu đã được xuất thành công vào file Excel.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất dữ liệu ra Excel: " + ex.Message);
            }
        }

        public void editData()
        {
            try
            {
                // Lấy dữ liệu từ các điều khiển
                string TenKH = guna2TextBox5.Text; // Tên khách hàng
                string KhCCCD = guna2TextBox6.Text; // CCCD

                // Kiểm tra ngày sinh từ DateTimePicker
                DateTime NgaySinh = guna2DateTimePicker3.Value;

                // Kiểm tra giới tính
                string GioiTinh = comboBox3.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(GioiTinh))
                {
                    MessageBox.Show("Vui lòng chọn giới tính.");
                    return;
                }

                // Kiểm tra số điện thoại hợp lệ (10 số)
                string soDTText = guna2TextBox7.Text;
                if (string.IsNullOrEmpty(soDTText) || soDTText.Length != 10 || !soDTText.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập 10 số.");
                    return;
                }

                // Lưu số điện thoại dưới dạng chuỗi
                string SoDT = soDTText;

                // Kiểm tra địa chỉ
                string DiaChi = guna2TextBox9.Text;
                if (string.IsNullOrEmpty(DiaChi))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ.");
                    return;
                }

                // Kiểm tra CCCD và Tên khách hàng
                if (string.IsNullOrEmpty(KhCCCD) || KhCCCD.Length != 12 || !KhCCCD.All(char.IsDigit))
                {
                    MessageBox.Show("Căn cước công dân phải có 12 chữ số.");
                    return;
                }

                // Kết nối cơ sở dữ liệu
                MySqlConnection connection = ConnectDb.GetConnection();
                if (connection == null)
                {
                    MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu. Vui lòng kiểm tra lại kết nối.");
                    return;
                }

                // Cập nhật thông tin khách hàng
                string query = "UPDATE khachhang SET TenKH = @TenKH, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, SoDT = @SoDT, DiaChi = @DiaChi WHERE KhCCCD = @KhCCCD";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Thêm các tham số vào câu lệnh SQL
                    command.Parameters.AddWithValue("@KhCCCD", KhCCCD);
                    command.Parameters.AddWithValue("@TenKH", TenKH);
                    command.Parameters.AddWithValue("@NgaySinh", NgaySinh);
                    command.Parameters.AddWithValue("@GioiTinh", GioiTinh);
                    command.Parameters.AddWithValue("@SoDT", SoDT);  // Lưu số điện thoại dưới dạng chuỗi
                    command.Parameters.AddWithValue("@DiaChi", DiaChi);

                    // Thực thi câu lệnh SQL
                    command.ExecuteNonQuery();
                    MessageBox.Show("Dữ liệu đã được cập nhật thành công.");
                    LoadDataIntoDataGridView();  // Cập nhật lại DataGridView sau khi sửa dữ liệu
                }
            }
            catch (MySqlException ex)
            {
                // Xử lý lỗi MySQL
                MessageBox.Show("Lỗi khi cập nhật dữ liệu vào cơ sở dữ liệu: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
            finally
            {
                // Đóng kết nối
                MySqlConnection connection = ConnectDb.GetConnection();
                if (connection != null)
                {
                    ConnectDb.CloseConnection(connection);
                }
            }
        }



        private void QuanLyKhach_Load(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
            guna2DataGridView1.CellClick += guna2DataGridView1_CellContentClick;
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin từ dòng đã chọn
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                // Hiển thị thông tin vào các TextBox
                guna2TextBox5.Text = row.Cells["TenKH"].Value.ToString();    // Tên khách hàng
                guna2TextBox6.Text = row.Cells["KhCCCD"].Value.ToString();  // CCCD
                guna2TextBox7.Text = row.Cells["SoDT"].Value.ToString();    // Số điện thoại
                guna2TextBox9.Text = row.Cells["DiaChi"].Value.ToString(); // Địa chỉ
                comboBox3.SelectedItem = row.Cells["GioiTinh"].Value.ToString(); // Giới tính
                guna2DateTimePicker3.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value); // Ngày sinh
            }
        }

        private void searchData()
        {
            // Lấy từ khóa tìm kiếm từ TextBox
            string keyword = guna2TextBox1.Text.Trim(); // TextBox để nhập từ khóa tìm kiếm

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm.");
                return;
            }

            // Kết nối cơ sở dữ liệu
            MySqlConnection connection = ConnectDb.GetConnection();
            if (connection == null)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                return;
            }

            // Câu lệnh SQL tìm kiếm
            string query = @"
        SELECT KhCCCD, TenKH, ngaysinh, DiaChi, SoDT, GioiTinh
        FROM khachhang
        WHERE KhCCCD LIKE @keyword 
           OR TenKH LIKE @keyword 
           OR ngaysinh LIKE @keyword 
           OR DiaChi LIKE @keyword 
           OR SoDT LIKE @keyword 
           OR GioiTinh LIKE @keyword 
           ";

            using (var command = new MySqlCommand(query, connection))
            {
                // Thêm từ khóa tìm kiếm vào câu lệnh SQL
                command.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                try
                {
                    // Thực hiện truy vấn và đổ dữ liệu vào DataTable
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Hiển thị kết quả tìm kiếm vào DataGridView
                    guna2DataGridView1.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy kết quả phù hợp.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm dữ liệu: " + ex.Message);
                }
                finally
                {
                    ConnectDb.CloseConnection(connection); // Đóng kết nối sau khi thực hiện
                }
            }
        }


        private void deleteData()
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Xác nhận người dùng muốn xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa các khách hàng đã chọn?", "Xác nhận", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Kết nối cơ sở dữ liệu
                    MySqlConnection connection = ConnectDb.GetConnection();
                    if (connection == null)
                    {
                        MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                        return;
                    }

                    try
                    {
                        foreach (DataGridViewRow row in guna2DataGridView1.SelectedRows)
                        {
                            // Lấy mã khách hàng từ mỗi dòng được chọn
                            string MaKH = row.Cells["KhCCCD"].Value.ToString();

                            // Xóa dữ liệu liên quan trong bảng chitietphieuthue
                            string queryDeleteChiTietPhieuThue = "DELETE FROM chitietphieuthue WHERE MaPhieuThue IN (SELECT MaPhieuThue FROM phieuthue WHERE MaKH = @MaKH)";
                            using (var command = new MySqlCommand(queryDeleteChiTietPhieuThue, connection))
                            {
                                command.Parameters.AddWithValue("@MaKH", MaKH);
                                command.ExecuteNonQuery();
                            }

                            // Xóa dữ liệu trong bảng phieuthue
                            string queryDeletePhieuThue = "DELETE FROM phieuthue WHERE MaKH = @MaKH";
                            using (var command = new MySqlCommand(queryDeletePhieuThue, connection))
                            {
                                command.Parameters.AddWithValue("@MaKH", MaKH);
                                command.ExecuteNonQuery();
                            }

                            // Xóa dữ liệu trong bảng khachhang
                            string queryDeleteKhachHang = "DELETE FROM khachhang WHERE KhCCCD = @KhCCCD";
                            using (var command = new MySqlCommand(queryDeleteKhachHang, connection))
                            {
                                command.Parameters.AddWithValue("@KhCCCD", MaKH);
                                command.ExecuteNonQuery();
                            }
                        }

                        // Hiển thị thông báo và tải lại dữ liệu
                        MessageBox.Show("Xoá thành công khách hàng.");
                        LoadDataIntoDataGridView();  // Tải lại dữ liệu sau khi xóa
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message);
                    }
                    finally
                    {
                        ConnectDb.CloseConnection(connection);  // Đóng kết nối sau khi thực hiện
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.");
            }
        }



        private void guna2Button2_Click(object sender, EventArgs e)
        {
            exportDataToExcel();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            editData();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            searchData(); 
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            deleteData();
        }
    }
    }

