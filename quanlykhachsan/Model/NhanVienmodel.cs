using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quanlykhachsan.Model
{
    internal class NhanVienmodel
    {
        public NhanVienmodel() { }
        public String MaNV {  get; set; }
        public String TenNV { get; set; }
        public string CCCD { get; set; }
        public string GioiTinh{ get; set;   }
        public string DiaChi { get; set; }
        public string MatKhau {  get; set; }
        public DateTime ngaysinh { get; set; }
        public long SoDt { get; set; }
        public string chucvu {  get; set; }

        public NhanVienmodel(string maNV, string tenNV, string cCCD, string gioiTinh, string diaChi, string matKhau, DateTime ngaysinh, long soDt, string chucvu)
        {
            MaNV = maNV;
            TenNV = tenNV;
            CCCD = cCCD;
            GioiTinh = gioiTinh;
            DiaChi = diaChi;
            MatKhau = matKhau;
            this.ngaysinh = ngaysinh;
            SoDt = soDt;
            this.chucvu = chucvu;
        }
    }
}
