using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media; //lembrar de apagar quando tiver certeza que n vai precisar

namespace Tetris
{
  public class GameGrid
  {
    public int Rows { get; }
    public int Cols { get; }

    public GridValue[,] Grid {get;}
    private GridValue Color;
    private int HalfCols { get; }

    private readonly Position[][] Blocks = new Position[7][]; 

    private readonly Dictionary<int, GridValue> IntToColor = new()
    {
      {0, GridValue.Cyan},
      {1, GridValue.Orange},
      {2, GridValue.Blue},
      {3, GridValue.Yellow},
      {4, GridValue.Green},
      {5, GridValue.Purple},
      {6, GridValue.Red}
    };

    public Position[] ActualBlock;
    public int ActualBlockType;

    //private readonly Random random = new();

    public GameGrid(int rows, int cols)
    {
      Rows = rows;
      Cols = cols;
      HalfCols = Cols/2;
      Grid = new GridValue[rows, cols];
      Setup();
    }

    private static readonly int[] IB_I = {1,1,1,1};
    private static readonly int[] IB_L = {1,1,2};
    private static readonly int[] IB_O = {2,2};
    private static readonly int[] IB_T = {1,3};

    public int rotate = 0; // contador de voltas

    private readonly Dictionary<int, int[]> IntToBlockConfig = new()
    {
      {0, IB_I},
      {1, IB_L},
      {2, IB_L},
      {3, IB_O},
      {4, IB_O},
      {5, IB_T},
      {6, IB_O}
    };

    private void Setup()
    {
      rotate = 0; //segurança em primeiro lugar
      Blocks[0] = GBlock(IntToBlockConfig[0]);
      Blocks[1] = GBlock(IntToBlockConfig[1]); 
      Blocks[2] = GBlock(IntToBlockConfig[2], 1); 
      Blocks[3] = GBlock(IntToBlockConfig[3]); 
      Blocks[4] = GBlock(IntToBlockConfig[4], 1); 
      Blocks[5] = GBlock(IntToBlockConfig[5], 1); 
      Blocks[6] = GBlock(IntToBlockConfig[6], 2); 
    }

    public void GenerateBlock(int TB)
    {
      Color = IntToColor[TB];
      ActualBlockType = TB;
      ActualBlock = Blocks[TB];

      DrawBlock();
    }

    public void DeletObject()
    {
      for(int i = 0; i < ActualBlock.Length; i++)
      {
        Grid[ActualBlock[i].Row, ActualBlock[i].Col] = GridValue.Empty;
      }
    }

    public void DrawBlock()
    {
      for(int i = 0; i < ActualBlock.Length; i++)
      {
        Grid[ActualBlock[i].Row, ActualBlock[i].Col] = Color;
      }
    }

    public void DrawActualBlock(int row = 0, int col = 0)
    {
      DeletObject();

      for (int i = 0; i < ActualBlock.Length; i++)  ActualBlock[i] = new Position(ActualBlock[i].Row+row, ActualBlock[i].Col+col); //muda a posição do bloco por inteiro

      DrawBlock();
    }

    private Position[] GBlock(int[] R, int esq = 0)
    {
      Position[] Block = new Position[4];
      int arrayCont = 0;

      for (int i = 0; i < R.Length; i++) 
      {
        for(int r = 0; r < R[i]; r++)
        {
          if(i == (R.Length-esq)) 
          {
            Block[arrayCont] = new Position(i, HalfCols + ( r-1 ));
          }
          else Block[arrayCont] = new Position(i,HalfCols+r);
          arrayCont++;
        }
      }
      return Block;
    }

    // ==> Levar para o Rotation.cs <==
    public Position[] RotateActualBlock(int[] R)
    {
      DeletObject();
      Position[] Block = new Position[4];
      int arrayCont = 0;

      //ele servia para rotacionar o bloco no rotate 2 e 3

      int esq = 0; //faz com que o bloco gire apartir da camada 'array'-esq
      int constSubtraction = -1;

      //configuração para cada bloco, má pratica necessaria para teste (n tinha deixado isso claro)
      if(ActualBlockType == 5) esq = 1;
      if(ActualBlockType == 4) esq = 1;
      if(ActualBlockType == 1) esq = (rotate == 1 || rotate == 3) ? 1 : 0;
      if(ActualBlockType == 2) esq = (rotate == 0 || rotate == 2) ? 1 : 0; //já da para ver a regra da onde o esq original se aplica

      if (ActualBlockType == 6) {
        esq = 2;
      }

      if(rotate == 0 || rotate == 2)
      {
        if(ActualBlockType == 1) constSubtraction = 1;
        for (int i = 0; i < R.Length; i++) 
        {
          for(int r = 0; r < R[i]; r++)
          {
            if(i == (R.Length-esq)) {
              if(rotate == 0) Block[arrayCont] = new Position(ActualBlock[0].Row+i, ActualBlock[0].Col+( r+constSubtraction ));
              else            Block[arrayCont] = new Position(ActualBlock[0].Row-i, ActualBlock[0].Col-( r+constSubtraction ));
            }
            else 
            {
              if(rotate == 0) Block[arrayCont] = new Position(ActualBlock[0].Row+i, ActualBlock[0].Col+r);
              else            Block[arrayCont] = new Position(ActualBlock[0].Row-i, ActualBlock[0].Col-r);
            }
            arrayCont++;
          }
        }
      }
      
      if(rotate == 1 || rotate == 3)
      {
        if(ActualBlockType == 4 || ActualBlockType == 6) constSubtraction = 1;
        for (int i = 0; i < R.Length; i++) 
        {
          for(int r = 0; r < R[i]; r++)
          {
            if(i == (R.Length-esq)) {
              if(rotate == 1) Block[arrayCont] = new Position(ActualBlock[0].Row+(r+constSubtraction), ActualBlock[0].Col+i);
              else            Block[arrayCont] = new Position(ActualBlock[0].Row-(r+constSubtraction), ActualBlock[0].Col-i);
            }
            else 
            {
              if(rotate == 1) Block[arrayCont] = new Position(ActualBlock[0].Row+r, ActualBlock[0].Col+i);
              else            Block[arrayCont] = new Position(ActualBlock[0].Row-r, ActualBlock[0].Col-i);
            }
            arrayCont++;
          }
        }
      }

      return Block;
    }

    public void Rotate(bool Dir)
    {
      if(rotate != 3 && Dir) rotate +=  1;
      else if(rotate != 0 && !Dir) rotate -= 1;
      else if (rotate == 3) rotate = 0;
      else rotate = 3;

      ActualBlock = RotateActualBlock(IntToBlockConfig[ActualBlockType]); //testar, ultima mudança foi mudar o tipo e dar retorno em vez de mudança no codigo
      DrawBlock(); //ultima linha do RotateActualBlock
    }
  }
}
