using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quanlykhachsan.Model
{
    internal class LoaiPhongmodel
    {
        public string maloaiphong { get; set; }
        public string tenloai { get; set; }
        public int songuoi { get; set; }
        public double dongia { get; set; }

        public LoaiPhongmodel()
        {
        }

        public LoaiPhongmodel(string maloaiphong, string tenloai, int songuoi, double dongia)
        {
            this.maloaiphong = maloaiphong;
            this.tenloai = tenloai;
            this.songuoi = songuoi;
            this.dongia = dongia;
        }
    }
}
