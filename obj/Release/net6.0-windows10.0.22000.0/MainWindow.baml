   M S B A M L   `   `   `  ���� A  =Tetris, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:TetrisTetris      Tetris.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     &localclr-namespace:Tetris     -     � Title$  Tetris��$	��500��$	��800��$	��400��$	��200�� � Icon$ TetrisAssets/Icon.ico��   	TextColor#��[    BackgroundColor#��[ .��(� .���  � RowDefinitions �� $
��Auto ��� $��* �.+��� -    ��Name$ 
TetrisText��$��	0,20,0,20q�$��15(�$��	 Tetris!!��$��Center=�$��Center=��� -    ��Name$ TatrisGridBorder��$��1��  GridLineColor#��[ $	��1.5q�$��Center=�$��Center=� �� EdgeMode$ Aliased=�$��20,40,20,20q�._�A� -    A�Name$ 
TetrisGrid��  GridBackgroundColor#��[ $	��200��$	��400�� J� SnapsToDevicePixels$
 True�� �� BitmapScalingMode$ HighQuality=�A� -   $ NewBlocksGrid��#��[ $	��100��$	��400��$
��Left=�$	��Top=�$
 True��$ HighQuality=�$��400,40,40,0q�$��1��A� -   $ T��#��[ $	��100��$	��400��$
��Left=�$	��Top=�$
 True��$ HighQuality=�$��
40,40,40,0q�$��1��.+��� -   $ Testador��$��	0,20,0,20q�$��15(�$��Endpoints!!��$��Center=�$��Center=��� -   $ Overlay��$��1��  OverlayColor#��[ $ Aliased=����� 	 �� Path$	 ActualWidth�
 �� ElementName$
 
GridBorder������ $	 ActualHeight�$
 
GridBorder������ $	 Margin�$
 
GridBorder��._��� -	   $ OverlayText��$��PRESS ANY KEY TO START��$��20(�$��Center=�$��Center=�$
��Wrap=�