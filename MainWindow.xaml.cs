using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
    {
      { GridValue.Empty, Images.Empty },
      { GridValue.Blue, Images.Blue },
      { GridValue.Cyan, Images.Cyan },
      { GridValue.Green, Images.Green },
      { GridValue.Orange, Images.Orange },
      { GridValue.Purple, Images.Purple },
      { GridValue.Red, Images.Red },
      { GridValue.Yellow, Images.Yellow }
    };

    private readonly int rows = 22, cols = 11;
    private bool gameRuning;

    private readonly System.Windows.Controls.Image[,] gridImages;
    private readonly System.Windows.Controls.Image[,] AfterBlocks;

    private readonly GameGrid gameGrid;

    private BlocksGenerator blocksGenerator = new();

    int points = 0;

    public MainWindow()
    {
      InitializeComponent();
      gridImages = SetupGrid();
      blocksGenerator.RandomizeImageBlock();
      AfterBlocks = GenerateBlocks();
      gameGrid = new GameGrid(rows, cols);
    }

    private System.Windows.Controls.Image[,] SetupGrid()
    {
      System.Windows.Controls.Image[,] images = new System.Windows.Controls.Image[rows,cols];
      TetrisGrid.Rows = rows;
      TetrisGrid.Columns = cols;
      TetrisGrid.Width = TetrisGrid.Height * (cols / (double) rows);

      for(int r = 0; r < rows; r++)
      {
        for(int c = 0; c < cols; c++){
          System.Windows.Controls.Image image = new System.Windows.Controls.Image
          {
            Source = Images.Empty,
            RenderTransformOrigin = new System.Windows.Point(0.5, 0.5)
          };
          images[r,c] = image;
          TetrisGrid.Children.Add(image);
        }
      }
      return images;
    }

    private System.Windows.Controls.Image[,] GenerateBlocks() //cria a caixa ao lado com os proximos blocos
    {
      System.Windows.Controls.Image[,] images = new System.Windows.Controls.Image[3,1];
      NewBlocksGrid.Rows = 3;
      NewBlocksGrid.Columns = 1;
      NewBlocksGrid.Width = NewBlocksGrid.Height * (1 / (double) 3);

      for(int c = 0; c < 1; c++) //formalidades, caso tenha que ser trocado é mais facil
      {
        for(int r = 0; r < 3; r++){
          System.Windows.Controls.Image image = new System.Windows.Controls.Image
          {
            Source = Images.Empty,
            RenderTransformOrigin = new System.Windows.Point(0.5, 0.5)
          };
          images[r,c] = image;
          NewBlocksGrid.Children.Add(image);
        }
      }
      return images;
    }

    private bool AutoColide(int row, int col, int block) //concertar gerar bloco e colisão com blocos col
    {
      for(int i = 0; i < gameGrid.ActualBlock.Length; i++)
      {
        if(gameGrid.ActualBlock[block].Row+row == gameGrid.ActualBlock[i].Row && gameGrid.ActualBlock[block].Col+col == gameGrid.ActualBlock[i].Col) return true;
      }
      return false;
    }

    private bool BlockInteractor(int row = 1, int col = 0)
    {
      for(int i = 0; i < gameGrid.ActualBlock.Length; i++)
      {
        if(gameGrid.ActualBlock[i].Row+row == rows || gameGrid.ActualBlock[i].Col+col == cols || (gameGrid.ActualBlock[i].Col+col == 0+col && col < 0) 
        || (gameGrid.Grid[gameGrid.ActualBlock[i].Row+row, gameGrid.ActualBlock[i].Col+col] != GridValue.Empty && !AutoColide(row, col, i))) 
        {
          return true;
        }
      }
      return false;
    } 

    private void CleanLines()
    {
      bool deleteLine = true;
      int ExtraPoints = 1000;
      for(int r = 0; r < rows; r++)
      {
        deleteLine = true;
        for(int c = 0; c < cols; c++){
          if(gameGrid.Grid[r,c] == GridValue.Empty) deleteLine = false;
        }
        if(deleteLine)
        {
          points+=ExtraPoints;
          ExtraPoints*=2;
          for(int c = 0; c < cols; c++){
            gameGrid.Grid[r,c] = GridValue.Empty;
          }
          bool Up = false;
          for(int R = r; R > 1; R--)
          {
            Up = false;
            for(int c = 0; c < cols; c++){
              gameGrid.Grid[R,c] = gameGrid.Grid[R-1,c]; 
              if(gameGrid.Grid[R,c] != GridValue.Empty) Up = true;
            }
            if(Up == false) break;
          }
          r--;
        }
      }
    }

    private async Task RunGame()
    {
      DrawAfterBlocks();
      gameGrid.GenerateBlock(blocksGenerator.preIntBlock); //blocksGenerator.preIntBlock (para voltar caso eu mude)
      blocksGenerator.ImageBlock(true);
      while(gameRuning)
      {
        Pontos.Text = points.ToString();
        gameGrid.DrawActualBlock();
        DrawGrid();
        if(BlockInteractor())
        {
          points+=10;
          DrawAfterBlocks(); 
          gameRuning = gameGrid.GenerateBlock(blocksGenerator.preIntBlock);
          blocksGenerator.ImageBlock(true);
          CleanLines();
        }
        else 
        {
          gameGrid.DrawActualBlock(1);
          await Task.Delay(280);
        }
      }
    }

    private void ResetGame()
    {
      Pontos.Text = points.ToString();
      for(int r = 0; r < rows; r++)
      {
        for(int c = 0; c < cols; c++){
          gameGrid.Grid[r,c] = GridValue.Empty;
        }
      }
    }

    private async void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if(Overlay.Visibility == Visibility.Visible)
      {
        e.Handled = true;
      }
      if(!gameRuning)
      {
        points = 0;
        gameRuning = true;
        for(int i = 0; i<3; i++)
        {
          OverlayText.Text = (i+1).ToString();
          await Task.Delay(1000);
        }

        OverlayText.Text = "GO!!";
        await Task.Delay(300);
        Overlay.Visibility = Visibility.Hidden;

        DrawGrid(); //criar uma função Draw que tb estaca informaçoes sobre a pontuação do player

        await RunGame();

        await Task.Delay(300);
        Overlay.Visibility = Visibility.Visible;

        OverlayText.Text = "   GAMEOVER!\nPRESS ANY KEY TO RESTART";
        ResetGame();

        gameRuning = false;
      }
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if(!gameRuning) return;
      switch(e.Key)
      {
        case Key.Left:
          if(!BlockInteractor(0, -1)) gameGrid.DrawActualBlock(0, -1);
          break;
        case Key.Right:
          if(!BlockInteractor(0, 1)) gameGrid.DrawActualBlock(0, 1);
          break;
        case Key.Up:
          gameGrid.Rotate();
          break;
        case Key.Down:
          gameGrid.DrawActualBlock(1);
          points+=10;
          break;
        case Key.Space:
          while(true)
          {
            if(!BlockInteractor(1))
            {
              gameGrid.DrawActualBlock(1);
              points+=10;
            }
            else
            {
              break;
            }
          } 
          break;
      }
      DrawGrid();
    }

    private void DrawGrid()
    {
      for(int r = 0; r < rows; r++)
      {
        for(int c = 0; c < cols; c++){
          GridValue gridVal = gameGrid.Grid[r,c];
          Position gridPos = new Position(r,c); 

          System.Windows.Controls.Image image = gridImages[r,c];

          image.Source = gridValToImage[gridVal];
          image.RenderTransform = Transform.Identity;
        }
      }
    }

    private void DrawAfterBlocks()
    {
      for(int r = 0; r < 1; r++) //formalidade caso eu decida mudar depois
      {
        NewBlocksGrid.Children.Clear();
        for(int c = 0; c < 3; c++){
          System.Windows.Controls.Image image = new System.Windows.Controls.Image
          {
            Source = blocksGenerator.ImageBlock()[c],
            RenderTransformOrigin = new System.Windows.Point(0.5, 0.5)
          };

          NewBlocksGrid.Children.Add(image);
        }
      }
    }
  }
}