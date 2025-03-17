using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLSVNhom.Models
{
    public class HocPhan
    {
        public string MaHP { get; set; } // Khóa chính
        public string TenHP { get; set; }
        public int SoTC { get; set; }

        public HocPhan() 
        {
            MaHP = string.Empty;
            TenHP = string.Empty;
            SoTC = 0;
        }

    }
}