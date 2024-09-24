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
    //private readonly System.Windows.Controls.Image[,] AfterBlocks;

    private readonly Random random = new();
    private GameGrid gameGrid;

    private BlocksGenerator blocksGenerator = new();

    public MainWindow()
    {
      InitializeComponent();
      gridImages = SetupGrid();
      gameGrid = new GameGrid(rows, cols);
    }

    private async Task Teste()
    {
      if(blocksGenerator.intBlocks[0] == 7) 
      {
        for(int i = 0; i < blocksGenerator.intBlocks.Length; i++) 
        {
          blocksGenerator.intBlocks[i] = random.Next(0,6);
          await Task.Delay(1);
        }
      }
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
            Source = blocksGenerator.ImageBlock()[r],
            RenderTransformOrigin = new System.Windows.Point(0.5, 0.5)
          };
          images[r,c] = image;
          NewBlocksGrid.Children.Add(image);
        }
      }
      return images;
    }

    private async Task RunGame()
    {
      GenerateBlocks();
      gameGrid.GenerateBlock(2); //blocksGenerator.intBlocks[0]
      //blocksGenerator.ImageBlock(true);
      while(true)
      {
        gameGrid.DrawActualBlock();
        DrawGrid();
        if(gameGrid.ActualBlock[3].Row == rows-1) 
        {
          gameGrid.GenerateBlock(blocksGenerator.intBlocks[0]);
          blocksGenerator.ImageBlock(true);
        }
        await Task.Delay(500);
        //gameGrid.DrawActualBlock(1);
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
        await Teste();

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

        OverlayText.Text = "PRESS ANY KEY TO RESTART";

        gameRuning = false;
      }
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if(!gameRuning) return;
      switch(e.Key)
      {
        case Key.Left:
          gameGrid.DrawActualBlock(0, -1);
          break;
        case Key.Right:
          gameGrid.DrawActualBlock(0, 1);
          break;
        case Key.Up:
          gameGrid.Rotate(true);
          break;
        case Key.Down:
          gameGrid.Rotate(false);
          break;
        case Key.Space:
          gameGrid.DrawActualBlock(1);
          break;
      }
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
  }
}