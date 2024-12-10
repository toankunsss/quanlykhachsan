using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quanlykhachsan.Model
{
    class DatPhongmodel
    {
        public string MaPhieuThue { get; set; }
        public string TenKH {  get; set; }
        public string MaKh {  get; set; }
        public DateTime ngayden {  get; set; }
        public DateTime ngaydi {  get; set; }

        public DatPhongmodel(string maPhieuThue, string tenKH, string maKh, DateTime ngayden, DateTime ngaydi)
        {
            MaPhieuThue = maPhieuThue;
            TenKH = tenKH;
            MaKh = maKh;
            this.ngayden = ngayden;
            this.ngaydi = ngaydi;
        }
        public DatPhongmodel(DateTime ngayden, DateTime ngaydi)
        {
            this.ngayden = ngayden;
            this.ngaydi = ngaydi;
        }
        public DatPhongmodel()
        {
        }
    }
}
