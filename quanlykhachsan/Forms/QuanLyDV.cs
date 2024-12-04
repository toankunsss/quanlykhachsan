using Guna.UI2.WinForms;
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
using ClosedXML.Excel;
using System.Windows.Forms;
namespace quanlykhachsan.Forms
{
    public partial class QuanLyDV : Form
    {
        public QuanLyDV()
        {
            InitializeComponent();

        }

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        public void importData()
        {
            // Lấy dữ liệu từ các điều khiển
            string MaDV = guna2TextBox5.Text;
            string TenDV = guna2TextBox6.Text;


            // Kiểm tra các giá trị đã được thêm vào ComboBox

            string LoaiDV = string.Empty;
            if (guna2ComboBox2.SelectedItem != null)
            {
                LoaiDV = guna2ComboBox2.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn loại dịch vụ.");
                return;
            }


            // Kiểm tra và chuyển đổi giá trị nhập vào
            double DonGia = 0;
            if (!double.TryParse(guna2TextBox2.Text, out DonGia))
            {
                MessageBox.Show("Giá trị đơn giá không hợp lệ.");
                return;
            }

            int Soluong = 0;
            if (!int.TryParse(guna2TextBox3.Text, out Soluong))
            {
                MessageBox.Show("Số lượng không hợp lệ.");
                return;
            }

            // Kiểm tra các giá trị có hợp lệ không
            if (string.IsNullOrEmpty(MaDV) || string.IsNullOrEmpty(TenDV) || string.IsNullOrEmpty(LoaiDV))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            // Kết nối cơ sở dữ liệu (Cần kiểm tra cách kết nối đúng của bạn)
            MySqlConnection connection = ConnectDb.GetConnection();
            if (connection == null)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                return;
            }

            string query = "INSERT INTO dichvu(MaDV, TenDV, LoaiDV, DonGia, Soluong) VALUES (@MaDV, @TenDV, @LoaiDV, @DonGia, @Soluong)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaDV", MaDV);
                command.Parameters.AddWithValue("@TenDV", TenDV);
                command.Parameters.AddWithValue("@LoaiDV", LoaiDV);
                command.Parameters.AddWithValue("@DonGia", DonGia);
                command.Parameters.AddWithValue("@Soluong", Soluong);

                try
                {
                    command.ExecuteNonQuery(); // Execute the SQL command
                    MessageBox.Show("Dữ liệu đã được nhập thành công.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi nhập dữ liệu: " + ex.Message);
                }
                finally
                {
                    ConnectDb.CloseConnection(connection);  // Close the connection after the operation
                }
            }
        }





        private void LoadDataIntoDataGridView()
        {
            // Get the connection
            MySqlConnection connection = ConnectDb.GetConnection();
            if (connection == null)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                return;
            }

            // Prepare the SQL query to fetch the data
            string query = "SELECT MaDV, TenDV, LoaiDV, DonGia, Soluong FROM dichvu";
            using (var command = new MySqlCommand(query, connection))
            {
                try
                {
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);  // Fill the DataTable with data

                    // Bind the data to the DataGridView
                    guna2DataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
                }
                finally
                {
                    ConnectDb.CloseConnection(connection);  // Close the connection after the operation
                }
            }
        }






        private void guna2Button7_Click(object sender, EventArgs e)
        {
            importData();
            LoadDataIntoDataGridView();
        }
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin từ dòng đã chọn
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                // Hiển thị thông tin vào các TextBox
                guna2TextBox5.Text = row.Cells["MaDV"].Value.ToString();
                guna2TextBox6.Text = row.Cells["TenDV"].Value.ToString();
                guna2TextBox2.Text = row.Cells["DonGia"].Value.ToString();
                guna2TextBox3.Text = row.Cells["Soluong"].Value.ToString();

                // Hiển thị loại dịch vụ vào ComboBox
                string loaiDV = row.Cells["LoaiDV"].Value.ToString();
                if (guna2ComboBox2.Items.Contains(loaiDV))
                {
                    guna2ComboBox2.SelectedItem = loaiDV;
                }
            }
        }
        private void editData()
        {
            string MaDV = guna2TextBox5.Text;
            string TenDV = guna2TextBox6.Text;

            string LoaiDV = string.Empty;
            if (guna2ComboBox2.SelectedItem != null)
            {
                LoaiDV = guna2ComboBox2.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn loại dịch vụ.");
                return;
            }

            // Kiểm tra và chuyển đổi giá trị nhập vào
            double DonGia = 0;
            if (!double.TryParse(guna2TextBox2.Text, out DonGia))
            {
                MessageBox.Show("Giá trị đơn giá không hợp lệ.");
                return;
            }

            int Soluong = 0;
            if (!int.TryParse(guna2TextBox3.Text, out Soluong))
            {
                MessageBox.Show("Số lượng không hợp lệ.");
                return;
            }

            // Kiểm tra các giá trị có hợp lệ không
            if (string.IsNullOrEmpty(MaDV) || string.IsNullOrEmpty(TenDV) || string.IsNullOrEmpty(LoaiDV))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            // Kết nối cơ sở dữ liệu
            MySqlConnection connection = ConnectDb.GetConnection();
            if (connection == null)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                return;
            }

            string query = "UPDATE dichvu SET TenDV = @TenDV, LoaiDV = @LoaiDV, DonGia = @DonGia, Soluong = @Soluong WHERE MaDV = @MaDV";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaDV", MaDV);
                command.Parameters.AddWithValue("@TenDV", TenDV);
                command.Parameters.AddWithValue("@LoaiDV", LoaiDV);
                command.Parameters.AddWithValue("@DonGia", DonGia);
                command.Parameters.AddWithValue("@Soluong", Soluong);

                try
                {
                    command.ExecuteNonQuery(); // Execute the SQL command
                    MessageBox.Show("Dữ liệu đã được cập nhật thành công.");
                    LoadDataIntoDataGridView();  // Tải lại dữ liệu sau khi cập nhật
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message);
                }
                finally
                {
                    ConnectDb.CloseConnection(connection);  // Close the connection after the operation
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            editData();
        }

        private void QuanLyDV_Load(object sender, EventArgs e)
        {
            string[] services = new string[] { "Giặt ủi", "Vệ sinh phòng", "Spa và massage", "Hồ bơi", "Karaoke", "Tắm hơi",


                                               "Tổ chức tiệc cưới, sinh nhật, lễ hội"

                };
            guna2ComboBox2.Items.AddRange(services.ToArray());
            LoadDataIntoDataGridView();
            guna2DataGridView1.CellClick += guna2DataGridView1_CellClick;
        }


        private void deleteData()
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Xác nhận người dùng muốn xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa các dịch vụ đã chọn?", "Xác nhận", MessageBoxButtons.YesNo);
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
                            // Lấy mã dịch vụ từ mỗi dòng được chọn
                            string MaDV = row.Cells["MaDV"].Value.ToString();

                            // Câu lệnh SQL để xóa
                            string query = "DELETE FROM dichvu WHERE MaDV = @MaDV";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@MaDV", MaDV);

                                // Thực hiện câu lệnh xóa cho mỗi dòng
                                command.ExecuteNonQuery();
                            }
                        }

                        // Hiển thị thông báo và tải lại dữ liệu
                        MessageBox.Show("Các dịch vụ đã được xóa thành công.");
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
                MessageBox.Show("Vui lòng chọn một dịch vụ để xóa.");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            deleteData();
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

                    // Danh sách để kiểm tra trùng lặp trong file Excel
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
                            dataRow[columnIndex] = cell.Value.ToString();
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


        private void guna2Button6_Click_1(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx"; // Chỉ cho phép chọn file Excel
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                importDataFromExcel(filePath);  // Gọi phương thức để nhập dữ liệu từ Excel
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
                // Danh sách các mã dịch vụ đã có để kiểm tra trùng
                HashSet<string> existingMaDV = new HashSet<string>();
                string queryCheck = "SELECT MaDV FROM dichvu";
                using (var command = new MySqlCommand(queryCheck, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existingMaDV.Add(reader["MaDV"].ToString());
                    }
                }

                // Duyệt qua tất cả các dòng trong DataGridView
                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    if (row.Cells["MaDV"].Value != null)
                    {
                        string MaDV = row.Cells["MaDV"].Value.ToString();
                        string TenDV = row.Cells["TenDV"].Value?.ToString() ?? string.Empty;
                        string LoaiDV = row.Cells["LoaiDV"].Value?.ToString() ?? string.Empty;
                        double DonGia;
                        int Soluong;

                        // Kiểm tra giá trị rỗng và định dạng số
                        if (string.IsNullOrEmpty(MaDV) || string.IsNullOrEmpty(TenDV) || string.IsNullOrEmpty(LoaiDV) ||
                            !double.TryParse(row.Cells["DonGia"].Value?.ToString(), out DonGia) ||
                            !int.TryParse(row.Cells["Soluong"].Value?.ToString(), out Soluong))
                        {
                            MessageBox.Show($"Dòng chứa mã dịch vụ {MaDV} có dữ liệu không hợp lệ. Bỏ qua dòng này.");
                            continue;
                        }

                        // Kiểm tra trùng lặp mã dịch vụ
                        if (existingMaDV.Contains(MaDV))
                        {
                           
                            continue;
                        }

                        // Thêm hoặc cập nhật cơ sở dữ liệu
                        string query = "INSERT INTO dichvu(MaDV, TenDV, LoaiDV, DonGia, Soluong) " +
                                       "VALUES(@MaDV, @TenDV, @LoaiDV, @DonGia, @Soluong) " +
                                       "ON DUPLICATE KEY UPDATE TenDV = @TenDV, LoaiDV = @LoaiDV, DonGia = @DonGia, Soluong = @Soluong";

                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@MaDV", MaDV);
                            command.Parameters.AddWithValue("@TenDV", TenDV);
                            command.Parameters.AddWithValue("@LoaiDV", LoaiDV);
                            command.Parameters.AddWithValue("@DonGia", DonGia);
                            command.Parameters.AddWithValue("@Soluong", Soluong);
                            command.ExecuteNonQuery();
                        }

                        existingMaDV.Add(MaDV); // Cập nhật danh sách mã dịch vụ đã tồn tại
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
                        var worksheet = workbook.Worksheets.Add("DichVu");

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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            exportDataToExcel();
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
            string query = "SELECT MaDV, TenDV, LoaiDV, DonGia, Soluong " +
                           "FROM dichvu " +
                           "WHERE MaDV LIKE @keyword OR TenDV LIKE @keyword OR LoaiDV LIKE @keyword";

            using (var command = new MySqlCommand(query, connection))
            {
                // Thêm từ khóa tìm kiếm vào câu lệnh SQL (dùng wildcard để tìm kiếm chuỗi con)
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

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }
    }
}
