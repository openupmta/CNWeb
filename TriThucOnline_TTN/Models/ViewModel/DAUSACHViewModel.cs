using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriThucOnline_TTN.Models.ViewModel
{
    public class DAUSACHViewModel
    {
        public int MaSach { get; set; }

        public string TenSach { get; set; }

        public int? MaNXB { get; set; }
        public string TenNXB { get; set; }

        public int? MaKhoaHoc { get; set; }
        public string TenKhoaHoc { get; set; }

        public int? MaTG { get; set; }
        public string TenTG { get; set; }

        public int? NamXuatBan { get; set; }

        public double? GiaTien { get; set; }

        public string PicBook { get; set; }

        public int? SoTrang { get; set; }

        public string BoCucSach { get; set; }

        public string TrichDan { get; set; }

        public int? SoLuongTon { get; set; }

        public int? Moi { get; set; }

    }
}