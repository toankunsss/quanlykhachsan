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
    public partial class DatPhongFrom : Form
    {
        public DatPhongFrom()
        {
            InitializeComponent();
            Display();
            FillLoaiPhongComboBox();
        }
        private void Display()
        {
            DatPhongData.DisplayAndFill("select phieuthue.MaPhieuThue, khachhang.TenKH, phieuthue.MaKH, chitietphieuthue.MaPhong, phieuthue.ngayden, phieuthue.ngaydi from phieuthue, chitietphieuthue, khachhang where khachhang.KhCCCD = phieuthue.MaKH and  chitietphieuthue.MaPhieuThue = phieuthue.MaPhieuThue", guna2DataGridView1);
        }
        public void Clear()
        {
            cbxGender.Items.Clear();
            cbxMPhong.Items.Clear();
            txtAdress.Text = txtCCCD.Text = txtGia.Text= txtName.Text = txtSearch.Text = txtSoLuong.Text = txtLPhong.Text = txtDt.Text =string.Empty;


        }
        private void FillLoaiPhongComboBox()
        {
            List<string> loaiPhongList = DatPhongData.GetLoaiPhong();
            if (loaiPhongList != null && loaiPhongList.Count > 0)
            {
                cbxLPhong.Items.Clear(); // Xóa dữ liệu cũ trong ComboBox
                foreach (string loaiPhong in loaiPhongList)
                {
                    cbxLPhong.Items.Add(loaiPhong);
                }
            }
            else
            {
                MessageBox.Show("Không có loại phòng nào trong cơ sở dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void FillMaPhongCbx()
        {
            string lPhongSelect = cbxLPhong.GetItemText(cbxLPhong.SelectedItem);
            DateTime ngayden = dateTimeFr.Value.Date;
            DateTime ngaydi = dateTimeTo.Value.Date;
            List<Phongmodel> listPhong = DatPhongData.GetValuePhong(lPhongSelect,ngayden,ngaydi);
            if (listPhong != null && listPhong.Count > 0)
            {
                cbxMPhong.Items.Clear();
                foreach (Phongmodel phong in listPhong)
                {
                    Console.WriteLine("Phòng trả về: " + phong.maphong); // Debug thông tin
                    cbxMPhong.Items.Add(phong.maphong);
                }
            }
            else
            {
                MessageBox.Show("Không có loại phòng nào trong cơ sở dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void ShowInFRoom()
        {
            string phongSelect = cbxMPhong.GetItemText(cbxMPhong.SelectedItem);

            LoaiPhongmodel loai = DatPhongData.FillMaphong(phongSelect);
            txtSoLuong.Text = loai.songuoi.ToString();
            txtLPhong.Text = phongSelect;
            txtGia.Text = loai.dongia.ToString();
            

        }
        public string CreatId()
        {
            // Assuming PhieuThueData.SelectAll() returns a list of DatPhongmodel
            List<DatPhongmodel> phieu = PhieuThueData.SelectAll();
            string check = "";
            int id = phieu.Count + 1; // Start from the count of existing records + 1

            if (phieu != null && phieu.Count > 0)
            {
                // Loop through existing records to check if the ID exists
                foreach (DatPhongmodel dat in phieu)
                {
                    if (dat.MaPhieuThue == "PN" + id)
                    {
                        check = dat.MaPhieuThue;
                        break;
                    }
                }

                // While the ID already exists, keep incrementing the ID
                while (!string.IsNullOrEmpty(check))
                {
                    id++; // Increment the ID by 1
                    foreach (DatPhongmodel dat in phieu)
                    {
                        if (dat.MaPhieuThue == "PN" + id)
                        {
                            check = dat.MaPhieuThue;
                            break;
                        }
                        else
                        {
                            check = ""; // If ID does not exist, break the loop
                        }
                    }
                }
                return "PN" + id; // Return the new unique ID
            }
            else
            {
                // If no records exist, start from "PH1"
                return "PN1";
            }
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

  


        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            // Kiểm tra số CCCD
            if (string.IsNullOrWhiteSpace(txtCCCD.Text))
            {
                MessageBox.Show("Vui lòng nhập số CCCD.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCCCD.Focus();
                return;
            }
            else if (!txtCCCD.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số CCCD chỉ được chứa chữ số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCCCD.Focus();
                return;
            }

            // Kiểm tra số điện thoại
            if (string.IsNullOrWhiteSpace(txtDt.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDt.Focus();
                return;
            }
            else if (!txtDt.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại chỉ được chứa chữ số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDt.Focus();
                return;
            }

            // Kiểm tra ngày sinh
            if (dateTimeBorn.Value == DateTimePicker.MinimumDateTime)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimeBorn.Focus();
                return;
            }

            // Kiểm tra ngày đến
            if (dateTimeFr.Value == DateTimePicker.MinimumDateTime)
            {
                MessageBox.Show("Vui lòng chọn ngày đến.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimeFr.Focus();
                return;
            }

            // Kiểm tra ngày đi
            if (dateTimeTo.Value == DateTimePicker.MinimumDateTime)
            {
                MessageBox.Show("Vui lòng chọn ngày đi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimeTo.Focus();
                return;
            }
            else if (dateTimeTo.Value <= dateTimeFr.Value)
            {
                MessageBox.Show("Ngày đi phải lớn hơn ngày đến.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimeTo.Focus();
                return;
            }

            // Kiểm tra loại phòng
            if (string.IsNullOrWhiteSpace(txtLPhong.Text))
            {
                MessageBox.Show("Vui lòng nhập loại phòng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLPhong.Focus();
                return;
            }

            // Kiểm tra mã phòng
            if (cbxMPhong.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn mã phòng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxMPhong.Focus();
                return;
            }

            // Kiểm tra số lượng khách
            if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng khách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }
            else if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng khách phải là số nguyên dương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            // Kiểm tra giá tiền
            if (string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập giá tiền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGia.Focus();
                return;
            }
            else if (!decimal.TryParse(txtGia.Text, out decimal giaTien) || giaTien <= 0)
            {
                MessageBox.Show("Giá tiền phải là số hợp lệ lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGia.Focus();
                return;
            }
            else
            {
                DatPhongmodel datPhongmodel = new DatPhongmodel();
                ChiTietPhieuThueModel chiTietPhieuThueModel = new ChiTietPhieuThueModel();
                // them khach hang
                string id = txtCCCD.Text;
                if (KhachHangdb.SearchIdKhachHang(id) != null)
                {
                    MessageBox.Show("khách hàng đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                }
                else
                {
                    KhachHangModel khachHangModel = new KhachHangModel
                    {
                        khCCCd = txtCCCD.Text,
                        tenKH = txtName.Text,
                        diaChi = txtAdress.Text,
                        soDt = Convert.ToInt64(txtDt.Text),
                        ngaySinh = dateTimeBorn.Value,
                        gioiTinh = cbxGender.GetItemText(cbxGender.SelectedItem)
                    };
                    KhachHangdb.AddKhachHang(khachHangModel);
                    MessageBox.Show("Thêm mới khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // them phieu nhap
                datPhongmodel.MaPhieuThue=  CreatId();
                String MaKh = cbxMPhong.GetItemText(cbxMPhong.SelectedItem);
                datPhongmodel.MaKh = txtCCCD.Text;
                datPhongmodel.ngayden = dateTimeFr.Value;
                datPhongmodel.ngaydi = dateTimeTo.Value;
                PhieuThueData.AddDatPhong(datPhongmodel);
                MessageBox.Show("Thông tin đã được lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // them chi tiet phieu
                String maphong = cbxMPhong.GetItemText(cbxMPhong.SelectedItem);
                chiTietPhieuThueModel.MaPhong = maphong;
                chiTietPhieuThueModel.MaPhieuDichVu = "Không có";
                chiTietPhieuThueModel.MaPhieuThue = datPhongmodel.MaPhieuThue;
                chitietdata.AddChiTiet(chiTietPhieuThueModel);
                
                Clear();
                Display();

            }



        }

        private void DatPhongFrom_Load(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void guna2Button3_Click(object sender, EventArgs e)
        {
            FillMaPhongCbx();



        }

        private void cbxMPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowInFRoom();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string id = txtSearch.Text;
            KhachHangModel khachHangModel = KhachHangdb.SearchIdKhachHang(id);

            // Kiểm tra nếu khachHangModel không phải là null
            if (khachHangModel != null)
            {
                txtName.Text = khachHangModel.tenKH;
                txtAdress.Text = khachHangModel.diaChi;
                txtCCCD.Text = khachHangModel.khCCCd;
                txtDt.Text = khachHangModel.soDt.ToString();
                dateTimeBorn.Value = khachHangModel.ngaySinh;

                if (khachHangModel.gioiTinh == "Nữ")
                {
                    cbxGender.SelectedItem = "Nữ";
                }
                else if (khachHangModel.gioiTinh == "Nam")
                {
                    cbxGender.SelectedItem = "Nam";
                }
            }
            else
            {
                MessageBox.Show("không tìm thấy khách hàng! ","Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
