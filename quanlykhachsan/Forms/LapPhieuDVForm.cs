using MySql.Data.MySqlClient;
using quanlykhachsan.DatabaseConect;
using quanlykhachsan.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlykhachsan.Forms
{
    public partial class LapPhieuDVForm : Form
    {
        public LapPhieuDVForm()
        {
            InitializeComponent();
            display();
            FillMaPhieuComboBox();
            FillMaDVComboBox();
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }
        private void display()
        {
            string sql = "select phieudichvu.MaPDV, chitietphieuthue.MaPhong, phieudichvu.MaPhieuThue, phieudichvu.MaDV, phieudichvu.SoLuong,phieudichvu.thanhtien from chitietphieuthue, phieudichvu where chitietphieuthue.MaPhieuThue=phieudichvu.MaPhieuThue";
            DatPhongData.DisplayAndFill(sql, guna2DataGridView1);
        }
        private void LoadText()
        {
            cbxMaDp.Items.Clear();
            cbxMaDV.Items.Clear();
            txtTenDV.Text = string.Empty;
            txtMaP.Text = string.Empty;
            txtSL.Text = string.Empty;
            txtThanhTien.Text = string.Empty;
        }
        private void FillMaPhieuComboBox()
        {
            List<DatPhongmodel> loaiPhongList = PhieuThueData.SelectAll();
            if (loaiPhongList != null && loaiPhongList.Count > 0)
            {
                cbxMaDp.Items.Clear(); // Xóa dữ liệu cũ trong ComboBox
                foreach (DatPhongmodel dp in loaiPhongList)
                {
                    cbxMaDp.Items.Add(dp.MaPhieuThue);
                }
            }
            else
            {
                MessageBox.Show("Không có mã phiếu thuê nào trong cơ sở dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void FillMaDVComboBox()
        {
            List<string> loaiPhongList = selectMaDV();
            if (loaiPhongList != null && loaiPhongList.Count > 0)
            {
                cbxMaDV.Items.Clear(); // Xóa dữ liệu cũ trong ComboBox
                foreach (string dp in loaiPhongList)
                {
                    cbxMaDV.Items.Add(dp);
                }
            }
            else
            {
                MessageBox.Show("Không có mã dịch vụ nào trong cơ sở dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        public static List<string> selectMaDV()
        {
            List<string> MaDVs = new List<string>();

            string sql = "SELECT MaDV FROM dichvu";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Retrieving all necessary fields from the database
                    string maDV = reader["MaDV"].ToString();
                    MaDVs.Add(maDV);
                    // Creating the DatPhongmodel instance with all the required parameters
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error executing query: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            return MaDVs;
        }
        private void LapPhieuDVForm_Load(object sender, EventArgs e)
        {

        }
        private Tuple<string, double> DVName(string name)
        {
            string tenDV = null;
            double donGia = 0; // Sử dụng double thay vì decimal
            string sql = "SELECT TenDV, DonGia FROM dichvu WHERE MaDV = @MaDV";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            // Thêm tham số vào câu lệnh SQL
            cmd.Parameters.AddWithValue("@MaDV", name);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();

                // Kiểm tra nếu có dữ liệu trả về
                if (reader.Read())
                {
                    tenDV = reader["TenDV"].ToString();
                    donGia = Convert.ToDouble(reader["DonGia"]); // Convert sang double
                }

                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error executing query: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
            return Tuple.Create(tenDV, donGia);
        }

        private string PhongName(string name)
        {
            string tenDV = null;
            string sql = "SELECT MaPhong FROM chitietphieuthue WHERE MaPhieuThue = @MaPhieuThue";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            // Thêm tham số vào câu lệnh SQL
            cmd.Parameters.AddWithValue("@MaPhieuThue", name);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();

                // Kiểm tra nếu có dữ liệu trả về
                if (reader.Read())
                {
                    tenDV = reader["MaPhong"].ToString();
                }

                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error executing query: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            return tenDV;
        }
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy hàng hiện tại
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                // Gán dữ liệu từ các ô vào các TextBox
                cbxMaDp.Text = row.Cells["MaPhieuThue"].Value?.ToString();
                cbxMaDV.Text = row.Cells["MaDV"].Value?.ToString();
                txtMaP.Text = row.Cells["MaPhong"].Value?.ToString();
                txtSL.Text = row.Cells["SoLuong"].Value?.ToString();
                txtThanhTien.Text = row.Cells["ThanhTien"].Value?.ToString();



            }
        }

        private void txtTenDV_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbxMaDV_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = cbxMaDV.SelectedItem as string;
            var ketqua = DVName(name);
            txtTenDV.Text = ketqua.Item1;

        }

        private void cbxMaDp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = cbxMaDp.SelectedItem as string;
            txtMaP.Text = PhongName(name);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            LoadText();
        }

        private void txtSL_TextChanged(object sender, EventArgs e)
        {
            string name = cbxMaDV.SelectedItem as string;
            var kq = DVName(name);
            double gia = kq.Item2;
            int soluong = int.Parse(txtSL.Text);
            double thanhtien = soluong * gia;
            txtThanhTien.Text = thanhtien.ToString();
        }
        public void InsertDV()
        {
            // Tạo ID tự động như PDV1, PDV2, ...
            string maPDV = GeneratePDVId();

            // Nếu không thể tạo ID, thoát khỏi phương thức
            if (maPDV == null)
            {
                MessageBox.Show("Không thể tạo ID phiếu dịch vụ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // SQL Insert với ID tự động
            string sql = "INSERT INTO phieudichvu (MaPDV, MaPhieuThue, MaDV, SoLuong, thanhtien) VALUES (@MaPDV, @MaPhieuThue, @MaDV, @SoLuong, @thanhtien)";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            // Thêm tham số vào câu lệnh SQL
            cmd.Parameters.Add("@MaPDV", MySqlDbType.VarChar).Value = maPDV;
            cmd.Parameters.Add("@MaPhieuThue", MySqlDbType.VarChar).Value = cbxMaDp.Text;
            cmd.Parameters.Add("@MaDV", MySqlDbType.VarChar).Value = cbxMaDV.Text;
            cmd.Parameters.Add("@SoLuong", MySqlDbType.Int32).Value = txtSL.Text;
            cmd.Parameters.Add("@thanhtien", MySqlDbType.Double).Value = txtThanhTien.Text;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi truy vấn: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
        public void UpdateDV()
        {
            // Kiểm tra các trường dữ liệu trước khi thực hiện cập nhật
            if (string.IsNullOrEmpty(cbxMaDp.Text) || string.IsNullOrEmpty(cbxMaDV.Text) || string.IsNullOrEmpty(txtSL.Text) || string.IsNullOrEmpty(txtThanhTien.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate dữ liệu số lượng
            if (!int.TryParse(txtSL.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên dương.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // SQL Update
            string sql = "UPDATE phieudichvu SET MaPhieuThue = @MaPhieuThue, MaDV = @MaDV, SoLuong = @SoLuong, thanhtien = @thanhtien WHERE MaPDV = @MaPDV";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            // Thêm tham số vào câu lệnh SQL
            cmd.Parameters.Add("@MaPDV", MySqlDbType.VarChar).Value = cbxMaDV.Text;  // Mã Phiếu Dịch Vụ cần sửa
            cmd.Parameters.Add("@MaPhieuThue", MySqlDbType.VarChar).Value = cbxMaDp.Text;  // Mã Phiếu Thuê
            cmd.Parameters.Add("@MaDV", MySqlDbType.VarChar).Value = cbxMaDV.Text;  // Mã Dịch Vụ
            cmd.Parameters.Add("@SoLuong", MySqlDbType.Int32).Value = txtSL.Text;  // Số lượng
            cmd.Parameters.Add("@thanhtien", MySqlDbType.Double).Value = txtThanhTien.Text;  // Thành tiền

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                display();  // Cập nhật lại giao diện bảng sau khi sửa
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi truy vấn: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
        public void DeleteDV(List<string> maPDVs)
        {
            // Kiểm tra nếu danh sách các MaPDV không rỗng
            if (maPDVs == null || maPDVs.Count == 0)
            {
                MessageBox.Show("Không có dịch vụ nào được chọn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo câu lệnh SQL DELETE với một mảng các MaPDV
            string sql = "DELETE FROM phieudichvu WHERE MaPDV IN (" + string.Join(",", maPDVs.Select(m => $"'{m}'")) + ")";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                display();  // Cập nhật lại bảng hiển thị sau khi xóa
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi truy vấn: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        public string GeneratePDVId()
        {
            string sql = "SELECT MAX(CAST(SUBSTRING(MaPDV, 4) AS UNSIGNED)) FROM phieudichvu";
            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            try
            {
                object result = cmd.ExecuteScalar();
                int newIdNumber = 1;

                // Kiểm tra kết quả có trả về giá trị hay không
                if (result != DBNull.Value)
                {
                    newIdNumber = Convert.ToInt32(result) + 1; // Lấy giá trị lớn nhất và cộng thêm 1
                }

                return "PDV" + newIdNumber.ToString(); // Tạo ID theo định dạng PDV1, PDV2, ...
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error executing query: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu các trường bắt buộc chưa được nhập
            if (string.IsNullOrEmpty(cbxMaDp.Text) || string.IsNullOrEmpty(cbxMaDV.Text) || string.IsNullOrEmpty(txtSL.Text) || string.IsNullOrEmpty(txtThanhTien.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate dữ liệu số lượng
            if (!int.TryParse(txtSL.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên dương.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Gọi hàm InsertDV để thêm dịch vụ vào cơ sở dữ liệu
            InsertDV();

            // Hiển thị thông báo thêm dịch vụ thành công
            MessageBox.Show("Dịch vụ đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Cập nhật lại dữ liệu hiển thị nếu cần thiết
            display();  // Ví dụ: gọi lại phương thức hiển thị để làm mới bảng
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            UpdateDV();
            display();
        }

        private void txtSL_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu giá trị nhập vào là hợp lệ
                if (int.TryParse(txtSL.Value.ToString(), out int soluong))
                {
                    string name = cbxMaDV.SelectedItem as string;
                    var kq = DVName(name);
                    double gia = kq.Item2;
                    double thanhtien = soluong * gia;
                    txtThanhTien.Text = thanhtien.ToString();
                }
                else
                {
                    MessageBox.Show("Số lượng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            List<string> maPDVs = new List<string>();

            foreach (DataGridViewRow row in guna2DataGridView1.SelectedRows)
            {
                string maPDV = row.Cells["MaPDV"].Value?.ToString();
                if (!string.IsNullOrEmpty(maPDV))
                {
                    maPDVs.Add(maPDV);
                }
            }

            // Gọi hàm xóa nếu có ít nhất một mã dịch vụ được chọn
            if (maPDVs.Count > 0)
            {
                DeleteDV(maPDVs);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dịch vụ để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }
        private void SearchDV()
        {
            string timkiem = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(timkiem))
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Câu lệnh SQL tìm kiếm theo MaPhieuThue hoặc MaDV
            string sql = "SELECT phieudichvu.MaPDV, chitietphieuthue.MaPhong, phieudichvu.MaPhieuThue, phieudichvu.MaDV, phieudichvu.SoLuong, phieudichvu.thanhtien " +
                         "FROM chitietphieuthue, phieudichvu " +
                         "WHERE chitietphieuthue.MaPhieuThue = phieudichvu.MaPhieuThue " +
                         "AND (phieudichvu.MaPhieuThue LIKE @search OR phieudichvu.MaDV LIKE @search)";

            MySqlConnection con = ConnectDb.GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            // Thêm tham số tìm kiếm vào câu lệnh SQL
            cmd.Parameters.AddWithValue("@search", "%" + timkiem + "%");

            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                guna2DataGridView1.DataSource = dt;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi truy vấn: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            SearchDV();

        }
    }
}
