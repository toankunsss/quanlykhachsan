using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quanlykhachsan.Model
{
    class ChiTietPhieuThueModel
    {
        public String MaPhieuThue {  get; set; }
        public String MaPhong { get; set; }
        public String MaPhieuDichVu { get; set; }

        public ChiTietPhieuThueModel(string maPhieuThue, string maPhong, string maPhieuDichVu)
        {
            MaPhieuThue = maPhieuThue;
            MaPhong = maPhong;
            MaPhieuDichVu = maPhieuDichVu;
        }
        public ChiTietPhieuThueModel(string maPhong)
        {
            MaPhong = maPhong;
        }
        public ChiTietPhieuThueModel()
        {
        }
    }
}
