   M S B A M L   `   `   `  ����  QLSVNhom     QLSVNhom.Views.InforNVView   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Thông Tin Nhân Viên��5   	   $	��250��6%   $	��400��62    � WindowStartupLocation$ CenterScreen=�6>   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $��50 �6    � ColumnDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6!   $��Mã NV:��61   $��Center=�6?   $	��0,5q�6Z   �� 5   
   $��0��6   $��1��6    �� 
IsReadOnly$
 True��6E   $��5q�6W   $��5q�6c   ��6/   �� *MaNV+6c   �� 5   
   $��1��6   $��0��6!   $��
Họ Tên:��61   $��Center=�6@   $	��0,5q�6[   �� 5   
   $��1��6   $��1��6   $��5q�6/   $��5q�6:   ��5      �� *HoTen+ �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   $��2��6   $��0��6!   $��Email:��61   $��Center=�6?   $	��0,5q�6Z   �� 5   
   $��2��6   $��1��6   $��5q�6/   $��5q�6:   ��5       �� *Email+$ PropertyChanged=��� 5#   
   $��3��6   $��0��6!   $��Lương (Encrypted):��61   $��Center=�6K   $	��0,5q�6f   �� 5$   
   $��3��6   $��1��6   $��5q�6/   $��5q�6:   ��5%      �� *Luong+$ PropertyChanged=��� 5(   
   $��4��6   $��2��6"    �� Orientation$ 
Horizontal=�66   $��Center=�6O   $
��0,10q�6l   .+�5)      �� $��	Xem lớp��6   $	��100��6N   $��5q�6Z   ��6'   �� *OpenLopViewCommand+6Z   �� 5*      $��Cập nhật��6   $	��100��6R   $��5q�6^   ��6(   �� *UpdateNhanVienCommand+6^   5+      5,      5-      