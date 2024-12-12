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

namespace quanlykhachsan.Forms
{
    public partial class LoaiPhongForm : Form
    {
        public LoaiPhongForm()
        {
            InitializeComponent();
        }



        private void importData()
        {
            // Lấy dữ liệu từ các điều khiển
            string Maloaiphong = guna2TextBox1.Text;
            string TenLoai = guna2TextBox4.Text;

            // Kiểm tra số người (songuoi) phải là một số nguyên dương
            string songuoiText = guna2TextBox2.Text;  // Giả sử số người nằm trong TextBox 2
            if (!int.TryParse(songuoiText, out int songuoi) || songuoi <= 0)
            {
                MessageBox.Show("Số người không hợp lệ. Vui lòng nhập số nguyên dương.");
                return;
            }

            // Kiểm tra đơn giá (dongia) phải là một số dương
            string dongiaText = guna2TextBox5.Text;  // Giả sử đơn giá nằm trong TextBox 5
            if (!double.TryParse(dongiaText, out double dongia) || dongia <= 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ. Vui lòng nhập số dương.");
                return;
            }

            // Kiểm tra nếu các thông tin cơ bản đều có đầy đủ
            if (string.IsNullOrEmpty(Maloaiphong) || string.IsNullOrEmpty(TenLoai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mã loại phòng và tên loại phòng.");
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
                // Kiểm tra trùng mã loại phòng
                string queryCheck = "SELECT COUNT(*) FROM loaiphong WHERE Maloaiphong = @Maloaiphong";
                using (var checkCommand = new MySqlCommand(queryCheck, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Maloaiphong", Maloaiphong);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Mã loại phòng đã tồn tại. Vui lòng nhập mã khác.");
                        return;
                    }
                }

                // Thêm loại phòng vào bảng
                string query = "INSERT INTO loaiphong(Maloaiphong, TenLoai, songuoi, dongia) " +
                               "VALUES (@Maloaiphong, @TenLoai, @songuoi, @dongia)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Maloaiphong", Maloaiphong);
                    command.Parameters.AddWithValue("@TenLoai", TenLoai);
                    command.Parameters.AddWithValue("@songuoi", songuoi);
                    command.Parameters.AddWithValue("@dongia", dongia);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Dữ liệu đã được nhập thành công.");
                    LoadDataIntoDataGridView();  // Giả sử bạn có phương thức này để load lại dữ liệu sau khi nhập
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
            string query = "SELECT Maloaiphong, TenLoai, songuoi, dongia FROM loaiphong";
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

        private void LoaiPhongForm_Load(object sender, EventArgs e)
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
                guna2TextBox1.Text = row.Cells["Maloaiphong"].Value.ToString();  // Mã loại phòng
                guna2TextBox4.Text = row.Cells["TenLoai"].Value.ToString();     // Tên loại phòng
                guna2TextBox2.Text = row.Cells["songuoi"].Value.ToString();     // Số người
                guna2TextBox5.Text = row.Cells["dongia"].Value.ToString();      // Đơn giá
            }

        }
        private void editData()
        {
            // Lấy dữ liệu từ các điều khiển
            string Maloaiphong = guna2TextBox1.Text;
            string TenLoai = guna2TextBox4.Text;

            // Kiểm tra số người hợp lệ
            int songuoi = 0;
            if (!int.TryParse(guna2TextBox2.Text, out songuoi))
            {
                MessageBox.Show("Số người không hợp lệ. Vui lòng nhập lại.");
                return;
            }

            // Kiểm tra đơn giá hợp lệ
            double dongia = 0;
            if (!double.TryParse(guna2TextBox5.Text, out dongia))
            {
                MessageBox.Show("Đơn giá không hợp lệ. Vui lòng nhập lại.");
                return;
            }

            // Kiểm tra các trường có hợp lệ không
            if (string.IsNullOrEmpty(Maloaiphong) || string.IsNullOrEmpty(TenLoai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mã loại phòng và tên loại phòng.");
                return;
            }

            // Kết nối cơ sở dữ liệu
            MySqlConnection connection = ConnectDb.GetConnection();
            if (connection == null)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                return;
            }

            // Câu lệnh SQL để cập nhật thông tin loại phòng
            string query = "UPDATE loaiphong SET TenLoai = @TenLoai, songuoi = @songuoi, dongia = @dongia WHERE Maloaiphong = @Maloaiphong";

            using (var command = new MySqlCommand(query, connection))
            {
                // Thêm tham số vào câu lệnh SQL để tránh SQL injection
                command.Parameters.AddWithValue("@Maloaiphong", Maloaiphong);
                command.Parameters.AddWithValue("@TenLoai", TenLoai);
                command.Parameters.AddWithValue("@songuoi", songuoi);
                command.Parameters.AddWithValue("@dongia", dongia);

                try
                {
                    // Thực thi câu lệnh SQL để cập nhật dữ liệu vào cơ sở dữ liệu
                    command.ExecuteNonQuery();
                    MessageBox.Show("Dữ liệu đã được cập nhật thành công.");
                    LoadDataIntoDataGridView();  // Tải lại dữ liệu sau khi cập nhật
                }
                catch (MySqlException ex)
                {
                    // Xử lý lỗi MySQL
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            importData();
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
                // Danh sách các mã loại phòng đã có để kiểm tra trùng
                HashSet<string> existingMaloaiphong = new HashSet<string>();
                string queryCheck = "SELECT Maloaiphong FROM loai_phong";
                using (var command = new MySqlCommand(queryCheck, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existingMaloaiphong.Add(reader["Maloaiphong"].ToString());
                    }
                }

                // Duyệt qua tất cả các dòng trong DataGridView
                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    if (row.Cells["Maloaiphong"].Value != null)
                    {
                        string Maloaiphong = row.Cells["Maloaiphong"].Value.ToString();
                        string TenLoai = row.Cells["TenLoai"].Value?.ToString() ?? string.Empty;
                        int songuoi;
                        if (!int.TryParse(row.Cells["songuoi"].Value?.ToString(), out songuoi))
                        {
                            MessageBox.Show($"Dòng chứa mã loại phòng {Maloaiphong} có số người không hợp lệ. Bỏ qua dòng này.");
                            continue;
                        }
                        double dongia;
                        if (!double.TryParse(row.Cells["dongia"].Value?.ToString(), out dongia))
                        {
                            MessageBox.Show($"Dòng chứa mã loại phòng {Maloaiphong} có đơn giá không hợp lệ. Bỏ qua dòng này.");
                            continue;
                        }

                        // Kiểm tra giá trị rỗng
                        if (string.IsNullOrEmpty(Maloaiphong) || string.IsNullOrEmpty(TenLoai))
                        {
                            MessageBox.Show($"Dòng chứa mã loại phòng {Maloaiphong} có dữ liệu không hợp lệ. Bỏ qua dòng này.");
                            continue;
                        }

                        // Kiểm tra trùng lặp mã loại phòng
                        if (existingMaloaiphong.Contains(Maloaiphong))
                        {
                            continue;
                        }

                        // Thêm hoặc cập nhật cơ sở dữ liệu
                        string query = @"INSERT INTO loai_phong(Maloaiphong, TenLoai, songuoi, dongia) 
                                 VALUES(@Maloaiphong, @TenLoai, @songuoi, @dongia)
                                 ON DUPLICATE KEY UPDATE 
                                    TenLoai = @TenLoai, 
                                    songuoi = @songuoi, 
                                    dongia = @dongia";

                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Maloaiphong", Maloaiphong);
                            command.Parameters.AddWithValue("@TenLoai", TenLoai);
                            command.Parameters.AddWithValue("@songuoi", songuoi);
                            command.Parameters.AddWithValue("@dongia", dongia);
                            command.ExecuteNonQuery();
                        }

                        existingMaloaiphong.Add(Maloaiphong); // Cập nhật danh sách mã loại phòng đã tồn tại
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


        private void deleteData()
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Xác nhận người dùng muốn xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa các loại phòng đã chọn?", "Xác nhận", MessageBoxButtons.YesNo);
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
                            // Lấy mã loại phòng từ mỗi dòng được chọn
                            string Maloaiphong = row.Cells["Maloaiphong"].Value.ToString();

                            // Câu lệnh SQL để xóa
                            string query = "DELETE FROM loaiphong WHERE Maloaiphong = @Maloaiphong";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@Maloaiphong", Maloaiphong);

                                // Thực hiện câu lệnh xóa cho mỗi dòng
                                command.ExecuteNonQuery();
                            }
                        }

                        // Hiển thị thông báo và tải lại dữ liệu
                        MessageBox.Show("Các loại phòng đã được xóa thành công.");
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
                MessageBox.Show("Vui lòng chọn một loại phòng để xóa.");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            deleteData();
        }
    }

}
