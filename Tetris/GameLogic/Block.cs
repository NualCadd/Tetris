using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.GameLogic
{
    public class Block
    {
        public int[,] shape { get; private set; }
        public int x { get; set; }
        public int y { get; set; }
        public int ID { get; private set; }

        public Block(int x, int y, int id)
        {
            this.x = x;
            this.y = y;
            this.ID = id;
            SetShape(id);
        }
        private void SetShape(int id)
        {
            switch (id)
            {
                case 1:
                    shape = new int[,]
                    {
                        {1,1 },
                        {1,1 },
                    };
                    break;
                case 2:
                    shape = new int[,]
                    {
                        {0,2,0 },
                        {2,2,2 },
                        {0,0,0 }
                    };
                    break;
                case 3:
                    shape = new int[,]
                    {
                        {0,3,3 },
                        {3,3,0 },
                        {0,0,0 }
                    };
                    break;
                case 4:
                    shape = new int[,]
                    {
                        {4,0,0 },
                        {4,0,0 },
                        {4,4,0 }
                    };
                    break;
                case 5:
                    shape = new int[,]
                    {
                        {0,5,0,0 },
                        {0,5,0,0 },
                        {0,5,0,0 },
                        {0,5,0,0 }
                    };
                    break;
                case 6:
                    shape = new int[,]
                    {
                        {0,0,6 },
                        {0,0,6 },
                        {0,6,6 }
                    };
                    break;
                case 7:
                    shape = new int[,]
                    {
                        {7,7,0 },
                        {0,7,7 },
                        {0,0,0 }
                    };
                    break;

            }
        }

        public void RotateCW()
        {
            int length = shape.GetLength(0);
            int[,] rotated = new int[length, length];
            for(int r = 0; r < length; r++)
            {
                for(int c = 0; c < length; c++)
                {
                    rotated[c, length - 1 - r] = shape[r, c];
                }
            }
            shape = rotated;
        }
    }
}
