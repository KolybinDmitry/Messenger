   M S B A M L   `   `   `  ����D  @Messenger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Messenger	Messenger  5   	       Messenger.Start   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Messenger     5   	     � Title$  Start��5   	   $	��240��6   $	��320��6$   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��Введите имя��6   $��0��6'   $
��Left=�64   $��5q�6O   �� 5   
   -   5         ��Name$ textBoxName��5      $��1��6'   $��2��64   $��5q�6I   �� 5   
   $��Введите ip��6   $��2��6&   $
��Left=�63   $��5q�6N   �� 5   
   -   5        $ 	textBoxIp��5      $��	127.0.0.1��6%   $��3��66   $��2��6C   $��5q�6X   �� 5   
   -   5         ��Name$ buttonNewChat��5      $��Новый чат��6(   $��5��6<   $��0��6I   $��5q�6Y   6d   �� 5   
   -   5        $ buttonConnect��5      $*��$Подключиться к чату��6(   $��5��6F   $��1��6S   $��5q�6c   6n   5      5      