using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLSVNhom.Models
{
    public class SinhVien
    {

        public string MaSV { get; set; } // Khóa chính
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string MaLop { get; set; }
        public string TenDN { get; set; } // Unique
        public string MatKhau { get; set; }

        public SinhVien()
        { 
            MaSV = string.Empty;
            HoTen = string.Empty;
            DiaChi = string.Empty;
            MaLop = string.Empty;
            TenDN = string.Empty;
            MatKhau = string.Empty;
        }
    }
}

