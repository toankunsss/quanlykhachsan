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
    public partial class QuanLyNhanVienForm : Form
    {
        public QuanLyNhanVienForm()
        {
            InitializeComponent();
        }


        public void importData()
        {
            // Lấy dữ liệu từ các điều khiển
            string MaNV = guna2TextBox3.Text;
            string TenNV = guna2TextBox6.Text;

            // Kiểm tra nếu giá trị CCCD hợp lệ (12 số)
            string cccdText = guna2TextBox7.Text;
            if (cccdText.Length != 12 || !cccdText.All(char.IsDigit))
            {
                MessageBox.Show("CCCD không hợp lệ. Vui lòng nhập 12 số.");
                return;
            }
            long CCCD = long.Parse(cccdText);

            // Kiểm tra ngày sinh từ DateTimePicker
            DateTime ngaysinh = guna2DateTimePicker3.Value;

            // Kiểm tra giới tính
            string GioiTinh = comboBox3.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(GioiTinh))
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                return;
            }

            // Kiểm tra số điện thoại hợp lệ (10 số)
            string soDTText = guna2TextBox2.Text;
            if (soDTText.Length != 10 || !soDTText.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập 10 số.");
                return;
            }
            int SoDT = int.Parse(soDTText);

            // Kiểm tra chức vụ từ ComboBox
            string ChucVu = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(ChucVu))
            {
                MessageBox.Show("Vui lòng chọn chức vụ.");
                return;
            }

            // Kiểm tra mật khẩu và yêu cầu mật khẩu phức tạp
            string MatKhau = guna2TextBox4.Text;
            if (string.IsNullOrEmpty(MatKhau) ||
                !MatKhau.Any(char.IsUpper) ||
                !MatKhau.Any(char.IsDigit) ||
                !MatKhau.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("Mật khẩu không hợp lệ. Vui lòng nhập mật khẩu có ít nhất 1 chữ cái in hoa, 1 số và 1 ký tự đặc biệt.");
                return;
            }

            // Mã hóa mật khẩu bằng SHA-256
            string MatKhauHash = Mahoa.ComputeSHA256Hash(MatKhau);

            // Kiểm tra địa chỉ
            string DiaChi = guna2TextBox9.Text;
            if (string.IsNullOrEmpty(DiaChi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.");
                return;
            }

            // Kiểm tra nếu các thông tin cơ bản đều có đầy đủ
            if (string.IsNullOrEmpty(MaNV) || string.IsNullOrEmpty(TenNV))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mã nhân viên và tên nhân viên.");
                return;
            }

            // Kết nối cơ sở dữ liệu
            MySqlConnection connection = ConnectDb.GetConnection();
            if (connection == null)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu. Vui lòng kiểm tra lại kết nối.");
                return;
            }

            try
            {
                // Kiểm tra trùng mã nhân viên
                string queryCheck = "SELECT COUNT(*) FROM nhanvien WHERE MaNV = @MaNV";
                using (var checkCommand = new MySqlCommand(queryCheck, connection))
                {
                    checkCommand.Parameters.AddWithValue("@MaNV", MaNV);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại. Vui lòng nhập mã khác.");
                        return;
                    }
                }

                // Thêm nhân viên vào bảng
                string query = "INSERT INTO nhanvien(MaNV, TenNV, CCCD, NgaySinh, GioiTinh, SoDT, ChucVu, MatKhau, DiaChi) " +
                               "VALUES (@MaNV, @TenNV, @CCCD, @NgaySinh, @GioiTinh, @SoDT, @ChucVu, @MatKhau, @DiaChi)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaNV", MaNV);
                    command.Parameters.AddWithValue("@TenNV", TenNV);
                    command.Parameters.AddWithValue("@CCCD", CCCD);
                    command.Parameters.AddWithValue("@NgaySinh", ngaysinh);
                    command.Parameters.AddWithValue("@GioiTinh", GioiTinh);
                    command.Parameters.AddWithValue("@SoDT", SoDT);
                    command.Parameters.AddWithValue("@ChucVu", ChucVu);
                    command.Parameters.AddWithValue("@MatKhau", MatKhauHash); // Sử dụng mật khẩu đã mã hóa
                    command.Parameters.AddWithValue("@DiaChi", DiaChi);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Dữ liệu đã được nhập thành công.");
                    LoadDataIntoDataGridView();
                }
            }
            catch (MySqlException ex)
            {
                // Xử lý lỗi MySQL
                MessageBox.Show("Lỗi khi nhập dữ liệu vào cơ sở dữ liệu: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
            finally
            {
                // Đóng kết nối
                ConnectDb.CloseConnection(connection);
            }
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
            string query = "SELECT MaNV, TenNV, CCCD, NgaySinh, GioiTinh, SoDT, ChucVu, DiaChi FROM nhanvien";
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

       

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {

        }
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin từ dòng đã chọn
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                // Hiển thị thông tin vào các TextBox
                guna2TextBox3.Text = row.Cells["MaNV"].Value.ToString();   // Mã nhân viên
                guna2TextBox6.Text = row.Cells["TenNV"].Value.ToString();  // Tên nhân viên
                guna2TextBox7.Text = row.Cells["CCCD"].Value.ToString();   // CCCD
                guna2DateTimePicker3.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);  // Ngày sinh
                comboBox3.SelectedItem = row.Cells["GioiTinh"].Value.ToString();  // Giới tính
                guna2TextBox2.Text = row.Cells["SoDT"].Value.ToString();   // Số điện thoại
                comboBox1.SelectedItem = row.Cells["ChucVu"].Value.ToString();  // Chức vụ
                //guna2TextBox4.Text = row.Cells["MatKhau"].Value.ToString();  // Mật khẩu
                guna2TextBox9.Text = row.Cells["DiaChi"].Value.ToString();  // Địa chỉ
            }
        }


        private void editData()
        {
            // Lấy dữ liệu từ các điều khiển
            string MaNV = guna2TextBox3.Text;
            string TenNV = guna2TextBox6.Text;

            // Kiểm tra nếu giá trị CCCD hợp lệ
            int CCCD = 0;
            if (!int.TryParse(guna2TextBox7.Text, out CCCD))
            {
                MessageBox.Show("CCCD không hợp lệ. Vui lòng nhập lại.");
                return;
            }

            // Kiểm tra ngày sinh từ DateTimePicker
            DateTime ngaysinh = guna2DateTimePicker3.Value;

            // Kiểm tra giới tính
            string GioiTinh = comboBox3.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(GioiTinh))
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                return;
            }

            // Kiểm tra số điện thoại hợp lệ
            int SoDT = 0;
            if (!int.TryParse(guna2TextBox2.Text, out SoDT))
            {
                MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập lại.");
                return;
            }

            // Kiểm tra chức vụ từ ComboBox
            string ChucVu = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(ChucVu))
            {
                MessageBox.Show("Vui lòng chọn chức vụ.");
                return;
            }

            // Kiểm tra mật khẩu và mã hóa mật khẩu trước khi cập nhật
            string MatKhau = guna2TextBox4.Text;
            if (string.IsNullOrEmpty(MatKhau))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.");
                return;
            }

            // Mã hóa mật khẩu bằng SHA-256
            string MatKhauHash = Mahoa.ComputeSHA256Hash(MatKhau);

            // Kiểm tra địa chỉ
            string DiaChi = guna2TextBox9.Text;
            if (string.IsNullOrEmpty(DiaChi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.");
                return;
            }

            // Kiểm tra các trường có hợp lệ không
            if (string.IsNullOrEmpty(MaNV) || string.IsNullOrEmpty(TenNV))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mã nhân viên và tên nhân viên.");
                return;
            }

            // Kết nối cơ sở dữ liệu
            MySqlConnection connection = ConnectDb.GetConnection();
            if (connection == null)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                return;
            }

            // Câu lệnh SQL để cập nhật thông tin nhân viên
            string query = "UPDATE nhanvien SET TenNV = @TenNV, CCCD = @CCCD, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, " +
                           "SoDT = @SoDT, ChucVu = @ChucVu, MatKhau = @MatKhau, DiaChi = @DiaChi WHERE MaNV = @MaNV";

            using (var command = new MySqlCommand(query, connection))
            {
                // Thêm tham số vào câu lệnh SQL để tránh SQL injection
                command.Parameters.AddWithValue("@MaNV", MaNV);
                command.Parameters.AddWithValue("@TenNV", TenNV);
                command.Parameters.AddWithValue("@CCCD", CCCD);
                command.Parameters.AddWithValue("@NgaySinh", ngaysinh);
                command.Parameters.AddWithValue("@GioiTinh", GioiTinh);
                command.Parameters.AddWithValue("@SoDT", SoDT);
                command.Parameters.AddWithValue("@ChucVu", ChucVu);
                command.Parameters.AddWithValue("@MatKhau", MatKhauHash);  // Sử dụng mật khẩu đã mã hóa
                command.Parameters.AddWithValue("@DiaChi", DiaChi);

                try
                {
                    // Thực thi câu lệnh SQL để cập nhật dữ liệu vào cơ sở dữ liệu
                    command.ExecuteNonQuery();
                    MessageBox.Show("Dữ liệu đã được cập nhật thành công.");
                    LoadDataIntoDataGridView();  // Tải lại dữ liệu sau khi cập nhật
                }
                catch (MySqlException ex)
                {
                    // Xử lý lỗi MySQL, ví dụ như trùng mã nhân viên, kết nối, v.v.
                    MessageBox.Show("Lỗi khi cập nhật dữ liệu vào cơ sở dữ liệu: " + ex.Message);
                }
                catch (Exception ex)
                {
                    // Xử lý các lỗi khác ngoài MySQL
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
                finally
                {
                    // Đóng kết nối sau khi thực thi
                    ConnectDb.CloseConnection(connection);
                }
            }
        }

        private void updateDatabaseFromDataGridView()
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
                // Danh sách các mã nhân viên đã có để kiểm tra trùng
                HashSet<string> existingMaNV = new HashSet<string>();
                string queryCheck = "SELECT MaNV FROM nhanvien";
                using (var command = new MySqlCommand(queryCheck, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existingMaNV.Add(reader["MaNV"].ToString());
                    }
                }

                // Duyệt qua tất cả các dòng trong DataGridView
                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    if (row.Cells["MaNV"].Value != null)
                    {
                        string MaNV = row.Cells["MaNV"].Value.ToString();
                        string TenNV = row.Cells["TenNV"].Value?.ToString() ?? string.Empty;
                        string CCCD = row.Cells["CCCD"].Value?.ToString() ?? string.Empty;
                        string ngaysinh = row.Cells["NgaySinh"].Value?.ToString() ?? string.Empty;
                        string GioiTinh = row.Cells["GioiTinh"].Value?.ToString() ?? string.Empty;
                        string SoDT = row.Cells["SoDT"].Value?.ToString() ?? string.Empty;
                        string ChucVu = row.Cells["ChucVu"].Value?.ToString() ?? string.Empty;
                        string DiaChi = row.Cells["DiaChi"].Value?.ToString() ?? string.Empty;
                        string MatKhauHash = row.Cells["MatKhau"].Value?.ToString() ?? string.Empty;

                        // Kiểm tra giá trị rỗng
                        if (string.IsNullOrEmpty(MaNV) || string.IsNullOrEmpty(TenNV) || string.IsNullOrEmpty(CCCD) ||
                            string.IsNullOrEmpty(ngaysinh) || string.IsNullOrEmpty(GioiTinh) || string.IsNullOrEmpty(SoDT) ||
                            string.IsNullOrEmpty(ChucVu) || string.IsNullOrEmpty(DiaChi))
                        {
                            MessageBox.Show($"Dòng chứa mã nhân viên {MaNV} có dữ liệu không hợp lệ. Bỏ qua dòng này.");
                            continue;
                        }

                        // Kiểm tra trùng lặp mã nhân viên
                        if (existingMaNV.Contains(MaNV))
                        {
                            continue;
                        }

                        // Thêm hoặc cập nhật cơ sở dữ liệu
                        string query = @"INSERT INTO nhanvien(MaNV, TenNV, CCCD, NgaySinh, GioiTinh, SoDT, ChucVu, DiaChi, MatKhau) 
                                 VALUES(@MaNV, @TenNV, @CCCD, @NgaySinh, @GioiTinh, @SoDT, @ChucVu, @DiaChi, @MatKhau)
                                 ON DUPLICATE KEY UPDATE 
                                    TenNV = @TenNV, 
                                    CCCD = @CCCD, 
                                    NgaySinh = @NgaySinh, 
                                    GioiTinh = @GioiTinh, 
                                    SoDT = @SoDT, 
                                    ChucVu = @ChucVu, 
                                    DiaChi = @DiaChi, 
                                    MatKhau = @MatKhau";

                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@MaNV", MaNV);
                            command.Parameters.AddWithValue("@TenNV", TenNV);
                            command.Parameters.AddWithValue("@CCCD", CCCD);
                            command.Parameters.AddWithValue("@NgaySinh", ngaysinh);
                            command.Parameters.AddWithValue("@GioiTinh", GioiTinh);
                            command.Parameters.AddWithValue("@SoDT", SoDT);
                            command.Parameters.AddWithValue("@ChucVu", ChucVu);
                            command.Parameters.AddWithValue("@DiaChi", DiaChi);
                            command.Parameters.AddWithValue("@MatKhau", MatKhauHash); // Mật khẩu đã mã hóa
                            command.ExecuteNonQuery();
                        }

                        existingMaNV.Add(MaNV); // Cập nhật danh sách mã nhân viên đã tồn tại
                    }
                }

                MessageBox.Show("Dữ liệu đã được cập nhật thành công vào cơ sở dữ liệu.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu vào cơ sở dữ liệu: " + ex.Message);
            }
            finally
            {
                ConnectDb.CloseConnection(connection); // Đóng kết nối sau khi thực hiện
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            editData();
            
        }

        private void QuanLyNhanVienForm_Load(object sender, EventArgs e)
        {
            string[] services = new string[] { "Giặt ủi", "Vệ sinh phòng", "Spa và massage", "Hồ bơi", "Karaoke", "Tắm hơi",


                                               "Tổ chức tiệc cưới, sinh nhật, lễ hội"

                };
            guna2ComboBox1.Items.AddRange(services.ToArray());
            LoadDataIntoDataGridView();
            guna2DataGridView1.CellClick += guna2DataGridView1_CellContentClick;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            importData();
            LoadDataIntoDataGridView();
        }

        private void deleteData()
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Xác nhận người dùng muốn xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa các nhân viên đã chọn?", "Xác nhận", MessageBoxButtons.YesNo);
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
                            // Lấy mã nhân viên từ mỗi dòng được chọn
                            string MaNV = row.Cells["MaNV"].Value.ToString();

                            // Câu lệnh SQL để xóa
                            string query = "DELETE FROM nhanvien WHERE MaNV = @MaNV";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@MaNV", MaNV);

                                // Thực hiện câu lệnh xóa cho mỗi dòng
                                command.ExecuteNonQuery();
                            }
                        }

                        // Hiển thị thông báo và tải lại dữ liệu
                        MessageBox.Show("Các nhân viên đã được xóa thành công.");
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
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa.");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            deleteData();
            
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
        SELECT MaNV, TenNV, CCCD, NgaySinh, GioiTinh, SoDT, ChucVu, DiaChi 
        FROM nhanvien
        WHERE MaNV LIKE @keyword 
           OR TenNV LIKE @keyword 
           OR CCCD LIKE @keyword 
           OR NgaySinh LIKE @keyword 
           OR GioiTinh LIKE @keyword 
           OR SoDT LIKE @keyword 
           OR ChucVu LIKE @keyword 
           OR DiaChi LIKE @keyword";

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


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            searchData();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            exportDataToExcel();
        }



        private void importDataFromExcel(string filePath)
        {
            try
            {
                if (!filePath.EndsWith(".xlsx"))
                {
                    MessageBox.Show("Định dạng file không hợp lệ. Vui lòng chọn file Excel (.xlsx).");
                    return;
                }

                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheets.Worksheet(1);

                    DataTable dataTable = new DataTable();
                    var firstRow = worksheet.FirstRowUsed();
                    foreach (var cell in firstRow.Cells())
                    {
                        dataTable.Columns.Add(cell.Value.ToString());
                    }

                    HashSet<string> maDVSet = new HashSet<string>();

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        DataRow dataRow = dataTable.NewRow();
                        int columnIndex = 0;

                        string maDV = row.Cell(1).GetValue<string>();

                        // Kiểm tra mã dịch vụ trùng hoặc để trống
                        if (string.IsNullOrEmpty(maDV) || maDVSet.Contains(maDV))
                        {
                            MessageBox.Show($"Mã dịch vụ {maDV} bị trùng hoặc để trống");
                            continue;
                        }

                        maDVSet.Add(maDV);

                        foreach (var cell in row.Cells())
                        {
                            if (cell.Address.ColumnNumber == 4) // Giả sử "NgaySinh" nằm ở cột thứ 4
                            {
                                DateTime ngaySinh;
                                // Kiểm tra và chuyển đổi ngày tháng
                                if (DateTime.TryParse(cell.Value.ToString(), out ngaySinh))
                                {
                                    // Đảm bảo định dạng ngày tháng là yyyy-MM-dd
                                    dataRow[columnIndex] = ngaySinh.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    MessageBox.Show($"Ngày sinh không hợp lệ tại dòng {row.RowNumber()}");

                                    continue;
                                }
                            }
                            else
                            {
                                dataRow[columnIndex] = cell.Value.ToString();
                            }

                            columnIndex++;
                        }

                        dataTable.Rows.Add(dataRow);
                    }

                    guna2DataGridView1.DataSource = dataTable;
                }

                updateDatabaseFromDataGridView();
                MessageBox.Show("Dữ liệu đã được nhập thành công từ Excel.");
                LoadDataIntoDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nhập dữ liệu từ Excel: " + ex.Message);
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx"; // Chỉ cho phép chọn file Excel
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                importDataFromExcel(filePath);  // Gọi phương thức để nhập dữ liệu từ Excel
            }
        }
    }
}
