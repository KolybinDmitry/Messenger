   M S B A M L   `   `   `  ����D  @Messenger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Messenger	Messenger  5   	       Messenger.AdminWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Messenger     5   	     � Title$  Server��5   	   $	��320��6   $	��480��6%   .��5
      � 
������� � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ 	textBlock��5      $��Пользователи:��6'   $��5,5,5,5q�6<   �� 5   
   -   5         ��Name$ listBoxUsers��5      $��1��6(   $��2��65   $��5q�6F   �� 5   
   -   5        $ listBoxLogs��5      $��3��6'   $��5q�68   $y�Hidden=�6C   �� 5   
   -   5         ��Name$ buttonLookLog��5      $,��&Посмотреть логи чата��6(   $��1��6G   $��3��6W   $��5q�6k   6v   �� 5   
   -   5        $ 
buttonExit��5      $��
Выход��6%   $��1��65   $��2��6E   $��5q�6R   6]   �� 5   
   -   5         ��Name$ textBoxMessage��5      $��2��6*   $��2��67   $��5,5,5,5q�6G   �� 5   
   -   5        $ 
buttonSend��5      $��Отправить��6%   $��3��69   $��2��6I   $��5q�6V   6a   �� 5   
   -   5        $ listBoxMessages��5      $��1��6+   $��1��68   $��3��6H   $��5q�6\   5      5      