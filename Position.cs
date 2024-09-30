using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
  public class Position
  {
    public int Row { get; }
    public int Col { get; }

    public Position(int row, int col)
    {
      Row = row;
      Col = col;
    }

    //isso era de um outro projeto meu do SnakeGame, feito com ajuda de um video do OttoBotoCode
    //e eu esqueci de tirar depois que vi que n era util 

    //Hash code
    public override bool Equals(object obj)
    {
      return obj is Position position &&
              Row == position.Row &&
              Col == position.Col;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Row, Col);
    }

    public static bool operator ==(Position left, Position right)
    {
      return EqualityComparer<Position>.Default.Equals(left, right);
    }

    public static bool operator !=(Position left, Position right)
    {
      return !(left == right);
    }
  }
}
