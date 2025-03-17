using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLSVNhom.Models
{
    public class BangDiem
    {
        public string MaSV { get; set; } // Khóa chính
        public string MaHP { get; set; } // Khóa chính
        public byte[] DiemThi { get; set; } // Mã hóa điểm

        public BangDiem() 
        { 
            MaSV = string.Empty;
            MaHP = string.Empty;
            DiemThi = new byte[0];
        }
    }
}