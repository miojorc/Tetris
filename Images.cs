using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris
{
  public static class Images
  {
    public readonly static ImageSource B_I = LoadImage("Block-I.png");
    public readonly static ImageSource B_L = LoadImage("Block-L.png");
    public readonly static ImageSource B_J = LoadImage("Block-J.png");
    public readonly static ImageSource B_O = LoadImage("Block-O.png");
    public readonly static ImageSource B_S = LoadImage("Block-S.png");
    public readonly static ImageSource B_T = LoadImage("Block-T.png");
    public readonly static ImageSource B_Z = LoadImage("Block-Z.png");

    public readonly static ImageSource Blue = LoadImage("TileBlue.png");
    public readonly static ImageSource Cyan = LoadImage("TileCyan.png");
    public readonly static ImageSource Green = LoadImage("TileGreen.png");
    public readonly static ImageSource Orange = LoadImage("TileOrange.png");
    public readonly static ImageSource Purple = LoadImage("TilePurple.png");
    public readonly static ImageSource Red = LoadImage("TileRed.png");
    public readonly static ImageSource Yellow = LoadImage("TileYellow.png");

    public readonly static ImageSource Empty = LoadImage("TileEmpty.png");
    private static ImageSource LoadImage(string fileName)
    {
      return new BitmapImage(new Uri($"TetrisAssets/{fileName}", UriKind.Relative));
    } 
  }
}
