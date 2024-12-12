using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace quanlykhachsan.Model
{
    class KhachHangModel
    {
        public String khCCCd {  get; set; }
        public String tenKH { get; set; }
        public String diaChi { get; set; }
        public String gioiTinh { get; set;  }
        public string soDt { get;set; }
        public DateTime ngaySinh { get; set; }

        public KhachHangModel(string khCCCd, string tenKH, string diaChi, string gioiTinh, string soDt, DateTime ngaySinh)
        {
            this.khCCCd = khCCCd;
            this.tenKH = tenKH;
            this.diaChi = diaChi;
            this.gioiTinh = gioiTinh;
            this.soDt = soDt;
            this.ngaySinh = ngaySinh;
        }

        public KhachHangModel()
        {
        }
    }
}
