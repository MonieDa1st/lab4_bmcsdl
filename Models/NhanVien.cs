using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLSVNhom.Models
{
    public class NhanVien
    {
    
        public string MaNV { get; set; } // Khóa chính
        public string HoTen { get; set; }
        public string Email { get; set; }
        public byte[] Luong { get; set; }
        public string TenDN { get; set; } // Unique
        public byte[] MatKhau { get; set; }
        public string PubKey { get; set; }

        public NhanVien()
        {
            MaNV = string.Empty;
            HoTen = string.Empty;
            Email = string.Empty;
            Luong = new byte[0];
            TenDN = string.Empty;
            MatKhau = new byte[0];
            PubKey = string.Empty;
        }
    }
}