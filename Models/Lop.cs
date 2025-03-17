using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLSVNhom.Models
{
    public class Lop
    {
        
        public string MaLop { get; set; } // Khóa chính
        public string TenLop { get; set; }
        public string MaNV { get; set; } // Giáo viên chủ nhiệm

        public Lop()
        {
            MaLop = string.Empty;
            TenLop = string.Empty;
            MaNV = string.Empty;
        }
    }
}