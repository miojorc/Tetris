using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tetris
{
  public class BlocksGenerator
  {
    public readonly Dictionary<int, ImageSource> IntToImage = new()
    {
      {0, Images.B_I},
      {1, Images.B_L},
      {2, Images.B_J},
      {3, Images.B_O},
      {4, Images.B_S},
      {5, Images.B_T},
      {6, Images.B_Z},
      {7, Images.B_Z} //caso ocarra algo que n deveria ocorrer
    };

    public readonly Dictionary<ImageSource, int> ImageToInt = new()
    {
      {Images.B_I, 0},
      {Images.B_L, 1},
      {Images.B_J, 2},
      {Images.B_O, 3},
      {Images.B_S, 4},
      {Images.B_T, 5},
      {Images.B_Z, 6}
    };

    private readonly Random random = new();

    public int[] intBlocks = {2,4,3};

    public void RandomizeImageBlock()
    {
      for(int i = 0; i < intBlocks.Length; i++)
      {
        intBlocks[i] = random.Next(0,6);
      }
    }

    public ImageSource[] ImageBlock(bool change = false)
    {
      if(change)
      {
        for(int i = 0; i < (intBlocks.Length-1); i++) intBlocks[i] = intBlocks[i+1];
        intBlocks[^1] = random.Next(0,6);
      }

      ImageSource[] arrays =  new ImageSource[intBlocks.Length];

      for(int i = 0; i < arrays.Length; i++) arrays[i] = IntToImage[intBlocks[i]];

      return arrays;
    }
  }
}
