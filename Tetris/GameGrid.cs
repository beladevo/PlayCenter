using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Colums { get; }

        public int this[int r, int c]
        {
            get => grid[r, c];  
            set => grid[r, c] = value;
        }

        public GameGrid (int rows , int colums)
        {
            Rows = rows;
            Colums = colums;
            grid = new int[rows, colums];
        }

        public bool IsInside(int r , int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Colums;
        }

        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) &&  grid[r, c] == 0;
        }

        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Colums; c++)
            {
                if (grid[r,c]==0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsRowEmpty( int r)
        {
            for (int c = 0; c < Colums; c++)
            {
                if (grid[r,c]!=0)
                {
                    return false;
                }
            }
            return true;
        }

        public void ClearRow( int r)
        {
            for (int c = 0; c < Colums; c++)
            {
                grid[r, c] = 0;
            }
        }

        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Colums; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r,c]= 0;
            }
        }

       public int ClearFullRows()
        {
            int cleared = 0;
            for (int r =Rows-1 ; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }

            }
            return cleared;
        }
    }
}
