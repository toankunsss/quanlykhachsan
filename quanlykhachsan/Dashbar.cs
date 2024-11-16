using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlykhachsan
{
    public partial class Dashbar : Form
    {
        bool siderbarExpand;
        private Guna2Button curentButton;
        private Form activeForm;
        public Dashbar()
        {
            InitializeComponent();
            OpenChildForm(new Forms.HomeForm());

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void Siderbar_TimeTick(object sender, EventArgs e)
        {
            if (siderbarExpand)
            {
                // Mở rộng
                sliderContainer.Width += 10;
                if (sliderContainer.Width >= sliderContainer.MaximumSize.Width)
                {
                    SidebarTime.Stop();
                    // Hiện chữ trên các button khi mở rộng
                    SetButtonTextVisible(true);
                }
            }
            else
            {
                // Thu nhỏ
                sliderContainer.Width -= 10;
                if (sliderContainer.Width <= sliderContainer.MinimumSize.Width)
                {
                    SidebarTime.Stop();
                }
            }
        }

        // Hàm để thiết lập chữ trên các button
        private void SetButtonTextVisible(bool isVisible)
        {
            exitBtn.Text = isVisible ? "     Đăng xuất" : "";
            serviceBtn.Text = isVisible ? "     Quản lý dịch vụ" : "";
            userBtn.Text = isVisible ? "     Quản lý nhân viên" : "";
            clientBtn.Text = isVisible ? "     Quản lý khách" : "";
            roomBtn.Text = isVisible ? "     Quản lý phòng" : "";
            voteBtn.Text = isVisible ? "     Lập phiếu dịch vụ" : "";
            hoadonBtn.Text = isVisible ? "     Lập hóa đơn" : "";
            votesBKBtn.Text = isVisible ? "     Phiếu đặt phòng" : "";
            bookingButton.Text = isVisible ? "     Đặt phòng" : "";
            homeButon.Text = isVisible ? "     Home" : "";
        }
        // hàm xử lý hành động click chuột vào menu
        private void menuButton_Click(object sender, EventArgs e)
        {
            if (siderbarExpand)
            {
                // Ẩn chữ trên các button khi thu nhỏ
                SetButtonTextVisible(false);
                // Nếu đang mở rộng, thu nhỏ
                SidebarTime.Start();
                siderbarExpand = false;
            }
            else
            {
                // Nếu đang thu nhỏ, mở rộng
                SidebarTime.Start();
                siderbarExpand = true;
            }
        }
        private void OpenChildForm(Form childFrom)
        {
            try
            {
                // Kiểm tra nếu đã có Form con đang mở
                if (activeForm != null)
                {
                    activeForm.Close(); // Đóng Form hiện tại
                    activeForm.Dispose(); // Giải phóng tài nguyên
                }

                // Gán Form mới vào biến activeForm
                activeForm = childFrom;
                childFrom.TopLevel = false; // Form con không phải là Form độc lập
                childFrom.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền
                childFrom.Dock = DockStyle.Fill; // Form con chiếm toàn bộ panelDesktopRFoot

                // Xóa toàn bộ Control cũ trong panelDesktopRFoot để tránh trùng lặp
                panelDesktopRFoot.Controls.Clear();

                // Thêm Form con vào panelDesktopRFoot
                panelDesktopRFoot.Controls.Add(childFrom);
                panelDesktopRFoot.Tag = childFrom;
                childFrom.BringToFront(); // Đưa Form con lên phía trước
                childFrom.Show(); // Hiển thị Form con
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Đã xảy ra lỗi khi mở Form: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
 

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void bookingButton_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.DatPhongFrom());
        }

        private void votesBKBtn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.PhieuDatPhongForm());

        }

        private void hoadonBtn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.LapHoaDon());

        }

        private void voteBtn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.LapPhieuDVForm());

        }

        private void roomBtn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.QuanLyPhongForm());

        }

        private void clientBtn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.QuanLyKhach());

        }

        private void userBtn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.QuanLyNhanVienForm());

        }

        private void serviceBtn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.QuanLyDV());

        }

        private void exitBtn_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void homeButon_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.HomeForm());
        }
    }
}
