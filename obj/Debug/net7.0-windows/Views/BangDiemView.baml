   M S B A M L   `   `   `  ����  QLSVNhom!    QLSVNhom.Views.BangDiemView   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Bảng Điểm��5   	   $	��350��6   $	��450��6(   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�6.   $��0��6@   .+�5      �� $��Mã Sinh Viên: ��6   $
��Bold"�6.   $��Center=�6@   �� 5      $��14(�6.   
����  �$��Center=�6R   ��6   �� *MASV+6R   5      �� 5   
   $��1��6   $��Bảng Điểm��6!   $��16(�62   $
��Bold"�6@   
���  ��$��0,5,0,10q�6g   �� 5   
   $��3��6   
�������$��1q�6X   ��6    �� *BangDiemList+ �� View 5      �� 5      .P�5      �� $��Mã HP��6%    �� Width$	 150��6Z    �� DisplayMemberBinding 64   �� *MaHP+6Z   �� 5      $��Điểm��6%   $	 100��6\    63   �� *	DiemThi+6\   5      5      �� 5    
   -   5         ��Name$ 
ScorePanel��5    ;   $ 
Horizontal=�6   $��2��6.   $y�	Collapsed=�6O   .+�5"      �� $��Mã học phần:��6   �� 5#      $	��100��6   ��6"   �� *NewBangDiem.MaHP+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5%      $��Điểm thi:��6   �� 5&      $	��100��6   ��6#   �� *NewBangDiem.DiemThi+$ TwoWay=�$ PropertyChanged=�	 �� StringFormat$	 {0:N2}���� 5(      -   5        $��Lưu điểm��5(      $	��100��6(   $��10q�64   6@   5)      �� 5+   
   $ 
Horizontal=�6   $��1��6.   $��Center=�6;   $��10q�6X   .+�5,      �� -   5        $��Nhập Điểm��5,      $	��108��6)   ��6Q   �� *ShowEnterScorePanelCommand+5-      5.      5/      