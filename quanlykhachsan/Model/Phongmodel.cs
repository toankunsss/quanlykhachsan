using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quanlykhachsan.Model
{
    internal class Phongmodel
    {
        public string maphong { get; set; }
        public LoaiPhongmodel Lphongmodel { get; set; }
        public string tinhtrang { get; set; }
        public string ghichu { get; set; }
        public Phongmodel()
        {
        }

        public Phongmodel(string maphong, LoaiPhongmodel lphongmodel, string tinhtrang, string ghichu)
        {
            this.maphong = maphong;
            Lphongmodel = lphongmodel;
            this.tinhtrang = tinhtrang;
            this.ghichu = ghichu;
        }
    }
}
