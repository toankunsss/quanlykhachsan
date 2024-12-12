using DocumentFormat.OpenXml.InkML;
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
    public partial class QuanLyPhongForm : Form
    {
        public QuanLyPhongForm()
        {
            InitializeComponent();
        }

        private void guna2VSeparator1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {




        }

        private void LoadMaLoaiPhongIntoComboBox()
        {
            MySqlConnection connection = ConnectDb.GetConnection();
            if (connection == null)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                return;
            }

            try
            {
                string query = "SELECT MaLoaiPhong FROM loaiphong";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    guna2ComboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        guna2ComboBox1.Items.Add(reader["MaLoaiPhong"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách loại phòng: " + ex.Message);
            }
            finally
            {
                ConnectDb.CloseConnection(connection);
            }
        }

        private void importData()
        {
            // Lấy dữ liệu từ các điều khiển
            string MaPhong = guna2TextBox1.Text.Trim();
            string MaLoaiPhong = guna2ComboBox1.SelectedItem?.ToString();
            string TinhTrang = guna2ComboBox2.SelectedItem?.ToString();
            string GhiChu = guna2TextBox4.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(MaPhong))
            {
                MessageBox.Show("Vui lòng nhập mã phòng.");
                return;
            }

            if (string.IsNullOrEmpty(MaLoaiPhong))
            {
                MessageBox.Show("Vui lòng chọn loại phòng.");
                return;
            }

            if (string.IsNullOrEmpty(TinhTrang))
            {
                MessageBox.Show("Vui lòng chọn tình trạng phòng.");
                return;
            }

            if (GhiChu.Length > 255)
            {
                MessageBox.Show("Ghi chú không được vượt quá 255 ký tự.");
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
                // Kiểm tra nếu MaLoaiPhong tồn tại trong bảng loaiphong
                string queryCheckLoaiPhong = "SELECT COUNT(*) FROM loaiphong WHERE MaLoaiPhong = @MaLoaiPhong";
                using (var command = new MySqlCommand(queryCheckLoaiPhong, connection))
                {
                    command.Parameters.AddWithValue("@MaLoaiPhong", MaLoaiPhong);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Loại phòng không tồn tại. Vui lòng kiểm tra lại.");
                        return;
                    }
                }

                // Kiểm tra nếu MaPhong đã tồn tại trong bảng phong
                string queryCheckMaPhong = "SELECT COUNT(*) FROM phong WHERE MaPhong = @MaPhong";
                using (var command = new MySqlCommand(queryCheckMaPhong, connection))
                {
                    command.Parameters.AddWithValue("@MaPhong", MaPhong);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Mã phòng đã tồn tại. Vui lòng nhập mã khác.");
                        return;
                    }
                }

                // Thêm dữ liệu vào bảng phong
                string queryInsertPhong = "INSERT INTO phong (MaPhong, MaLoaiPhong, TinhTrang, GhiChu) " +
                                           "VALUES (@MaPhong, @MaLoaiPhong, @TinhTrang, @GhiChu)";
                using (var command = new MySqlCommand(queryInsertPhong, connection))
                {
                    command.Parameters.AddWithValue("@MaPhong", MaPhong);
                    command.Parameters.AddWithValue("@MaLoaiPhong", MaLoaiPhong);
                    command.Parameters.AddWithValue("@TinhTrang", TinhTrang);
                    command.Parameters.AddWithValue("@GhiChu", GhiChu);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Nhập dữ liệu thành công!");
                    LoadDataIntoDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nhập dữ liệu: " + ex.Message);
            }
            finally
            {
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
            string query = "SELECT MaPhong, MaLoaiPhong, TinhTrang, GhiChu FROM phong";
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
        private void QuanLyPhongForm_Load(object sender, EventArgs e)
        {
            LoadMaLoaiPhongIntoComboBox();
            LoadDataIntoDataGridView();
            guna2DataGridView1.CellClick += guna2DataGridView1_CellContentClick;
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            importData();
        }



        private void editData()
        {
            // Lấy dữ liệu từ các điều khiển
            string MaPhong = guna2TextBox1.Text;          // Mã phòng
            string MaLoaiPhong = guna2ComboBox1.SelectedItem?.ToString(); // Mã loại phòng
            string TinhTrang = guna2ComboBox2.SelectedItem?.ToString();   // Tình trạng
            string GhiChu = guna2TextBox4.Text;           // Ghi chú

            // Kiểm tra nếu các trường dữ liệu quan trọng bị bỏ trống
            if (string.IsNullOrEmpty(MaPhong) || string.IsNullOrEmpty(MaLoaiPhong) || string.IsNullOrEmpty(TinhTrang))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin: Mã phòng, Mã loại phòng và Tình trạng.");
                return;
            }

            // Kết nối cơ sở dữ liệu
            MySqlConnection connection = ConnectDb.GetConnection();
            if (connection == null)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                return;
            }

            // Câu lệnh SQL để cập nhật thông tin phòng
            string query = "UPDATE phong SET MaLoaiPhong = @MaLoaiPhong, TinhTrang = @TinhTrang, GhiChu = @GhiChu " +
                           "WHERE MaPhong = @MaPhong";

            using (var command = new MySqlCommand(query, connection))
            {
                // Thêm tham số vào câu lệnh SQL để tránh SQL injection
                command.Parameters.AddWithValue("@MaPhong", MaPhong);
                command.Parameters.AddWithValue("@MaLoaiPhong", MaLoaiPhong);
                command.Parameters.AddWithValue("@TinhTrang", TinhTrang);
                command.Parameters.AddWithValue("@GhiChu", GhiChu);

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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            editData();
        }





        private void deleteData()
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Xác nhận người dùng muốn xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa các phòng đã chọn?", "Xác nhận", MessageBoxButtons.YesNo);
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
                            // Lấy mã phòng từ mỗi dòng được chọn
                            string MaPhong = row.Cells["MaPhong"].Value.ToString();

                            // Câu lệnh SQL để xóa
                            string query = "DELETE FROM phong WHERE MaPhong = @MaPhong";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@MaPhong", MaPhong);

                                // Thực hiện câu lệnh xóa cho mỗi dòng
                                command.ExecuteNonQuery();
                            }
                        }

                        // Hiển thị thông báo và tải lại dữ liệu
                        MessageBox.Show("Các phòng đã được xóa thành công.");
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
                MessageBox.Show("Vui lòng chọn một phòng để xóa.");
            }
        }
    
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            deleteData();
        }
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin từ dòng đã chọn
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                // Hiển thị thông tin vào các TextBox và ComboBox
                guna2TextBox1.Text = row.Cells["MaPhong"].Value.ToString();   // Mã phòng
                guna2ComboBox1.SelectedItem = row.Cells["MaLoaiPhong"].Value.ToString();  // Mã loại phòng
                guna2ComboBox2.SelectedItem = row.Cells["TinhTrang"].Value.ToString();    // Tình trạng
                guna2TextBox4.Text = row.Cells["GhiChu"].Value.ToString();                // Ghi chú
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            LoaiPhongForm loai = new LoaiPhongForm();
            loai.ShowDialog();
        }
    }
}
