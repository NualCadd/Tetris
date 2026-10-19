using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameLogic;
using Tetris.GameSetting;
namespace Tetris.Models
{
    public class Board
    {
        public int [,] Grid { get; private set; }

        public Board()
        {
            Grid = new int[Setting.row, Setting.col];
        }
        private bool IsInside(int x,int y)
        {
            return x >= 0 && x < Setting.row && y >= 0 && y < Setting.col;
        }
        private bool IsCollide(int x,int y)
        {
            return Grid[x, y] != 0;
        }
        public bool CheckCollision(Block block)
        {
            int size = block.shape.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (block.shape[i, j] != 0)
                    {
                        if(!IsInside(block.y + i, block.x + j) || IsCollide(block.y + i, block.x + j))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public void LockBlock(Block block)
        {
            int size = block.shape.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (block.shape[i, j] != 0)
                    {
                        Grid[block.y + i, block.x + j] = block.ID;
                    }
                }
            }
        }
        public int ClearLine()
        {
            int ClearedLine = 0;
            for (int i = Setting.row - 1; i >= 0; i--)
            {
                bool isFull = true;
                for (int j = 0; j < Setting.col; j++)
                {
                    if (Grid[i, j] == 0)
                    {
                        isFull = false;
                        break;
                    }
                }
                if (isFull)
                {
                    ClearedLine++;
                    for (int k = i; k > 0; k--)
                    {
                        for (int j = 0; j < Setting.col; j++)
                        {
                            Grid[k, j] = Grid[k - 1, j];
                        }
                    }
                    for (int j = 0; j < Setting.col; j++)
                    {
                        Grid[0, j] = 0;
                    }
                    i++;
                }
            }
            return ClearedLine;
        }
    }
}
