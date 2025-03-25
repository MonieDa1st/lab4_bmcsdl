using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using QLSVNhom.Models;

namespace QLSVNhom.ViewModels
{
    public class BangDiemViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<BangDiem> _bangDiemList = new();
        public ObservableCollection<BangDiem> BangDiemList
        {
            get => _bangDiemList;
            set
            {
                _bangDiemList = value;
                OnPropertyChanged(nameof(BangDiemList));
            }
        }

        public string MASV { get; private set; }
        private string _manv;

        public BangDiemViewModel(string masv, string manv)
        {
            MASV = masv;
            _manv = manv;
            LoadBangDiem();
        }

        private void LoadBangDiem()
        {
            string query = "EXEC SP_SEL_PUBLIC_BANGDIEM @MASV, @MANV";
            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new SqlParameter("@MASV", MASV),
                new SqlParameter("@MANV", _manv));

            BangDiemList.Clear();
            foreach (DataRow row in dt.Rows)
            {
                BangDiemList.Add(new BangDiem
                {
                    MaHP = row["MAHP"].ToString(),
                    DiemThi  = Convert.ToDouble(row["DIEMTHI"])
                });
            }
            OnPropertyChanged(nameof(BangDiemList)); // Cập nhật UI khi dữ liệu thay đổi
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

