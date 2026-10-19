using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris.GameLogic;
using Tetris.GameSetting;
using Tetris.Models;

namespace Tetris
{
    public partial class Form1 : Form

    {
        private Board board = new Board();
        private Block currentBlock;
        private Timer gameTimer;
        private Random random = new Random();
        
        public Form1()
        {
            InitializeComponent();
            // Ensure form receives key events
            this.KeyPreview = true;

            // Start the game when the form is created
            InitGame();
        }
        public void InitGame()
        {
            board = new Board(); // tạo bảng
            SpawnNewBlock();

            gameTimer = new Timer();
            gameTimer.Interval = 500; // thời gian rơi của khối
            gameTimer.Tick += GameTick;
            gameTimer.Start();
            //Khóa bảng
            picBoard.Invalidate();
        }

        private void SpawnNewBlock()
        {
            int randomID = random.Next(1, 8);
            currentBlock = new Block(5, 0, randomID); 

            if (board.CheckCollision(currentBlock))
            {
                gameTimer.Stop();
                MessageBox.Show("GAME OVER! Bạn đã xếp chạm nóc.", "Thua rồi");
            }
        }

        private void GameTick(object sender, EventArgs e)
        {
            currentBlock.y++;
            if (board.CheckCollision(currentBlock))
            {
                currentBlock.y--;
                board.LockBlock(currentBlock);
                board.ClearLine();
                SpawnNewBlock();
            }
            picBoard.Invalidate();
        }
        private void picBoard_Paint(object sender, PaintEventArgs e)
        {
            //Nếu chưa load xong game thì không vẽ gì cả
            if (board == null || currentBlock == null)
                return;

            Graphics g = e.Graphics;

            // BƯỚC 1: Vẽ các khối gạch chết (đã đóng băng) trên Bàn cờ
            for (int r = 0; r < Setting.row; r++)
            {
                for (int c = 0; c < Setting.col; c++)
                {
                    if (board.Grid[r, c] != 0)
                    {
                        DrawCell(g, c, r, board.Grid[r, c]);
                    }
                }
            }

            // BƯỚC 2: Vẽ khối gạch đang rơi đè lên trên
            int size = currentBlock.shape.GetLength(0);
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    // Chỉ vẽ những ô có gạch, bỏ qua ô số 0
                    if (currentBlock.shape[r, c] != 0)
                    {
                        DrawCell(g, currentBlock.x + c, currentBlock.y + r, currentBlock.ID);
                    }
                }
            }

            // BƯỚC 3: Vẽ lưới mờ (Sử dụng kích thước của picBoard thay vì Form)
            Pen gridPen = new Pen(Color.FromArgb(40, 40, 40)); // Màu xám mờ
            for (int r = 0; r <= Setting.row; r++)
                g.DrawLine(gridPen, 0, r * Setting.BlockSize, picBoard.Width, r * Setting.BlockSize);

            for (int c = 0; c <= Setting.col; c++)
                g.DrawLine(gridPen, c * Setting.BlockSize, 0, c * Setting.BlockSize, picBoard.Height);
        }
        private void DrawCell(Graphics g, int x, int y, int id)
        {
            Color color = GetColorByID(id);
            int drawX = x * Setting.BlockSize;
            int drawY = y * Setting.BlockSize;

            // Tô màu ruột
            g.FillRectangle(new SolidBrush(color), drawX, drawY, Setting.BlockSize, Setting.BlockSize);
            // Vẽ viền sáng hơn 1 chút tạo độ nổi 3D nhẹ
            g.DrawRectangle(new Pen(ControlPaint.Light(color)), drawX, drawY, Setting.BlockSize, Setting.BlockSize);
        }
        private Color GetColorByID(int id)
        {
            switch (id)
            {
                case 1: return Color.Cyan;    
                case 2: return Color.Blue;    
                case 3: return Color.Orange;  
                case 4: return Color.Yellow;  
                case 5: return Color.Lime;    
                case 6: return Color.Purple;  
                case 7: return Color.Red;     
                default: return Color.White;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (currentBlock == null) return base.ProcessCmdKey(ref msg, keyData);

            int oldX = currentBlock.x;
            int oldY = currentBlock.y;

            switch (keyData)
            {
                case Keys.Left:
                    currentBlock.x--; // Thử đi sang trái
                    if (board.CheckCollision(currentBlock)) currentBlock.x = oldX; // Nếu đụng, lùi lại
                    break;

                case Keys.Right:
                    currentBlock.x++; // Thử đi sang phải
                    if (board.CheckCollision(currentBlock)) currentBlock.x = oldX;
                    break;

                case Keys.Down:
                    currentBlock.y++; // Thử rơi nhanh xuống
                    if (board.CheckCollision(currentBlock)) currentBlock.y = oldY;
                    break;

                case Keys.Up:
                    // CÓ LƯU Ý TOÁN HỌC: Xoay khối
                    currentBlock.RotateCW();
                    // Nếu xoay xong mà hình dạng mới bị lọt ra ngoài lề hoặc đâm vào gạch cũ, 
                    // ta phải ép nó xoay ngược lại 3 lần (xoay 270 độ) để trở về y như cũ.
                    if (board.CheckCollision(currentBlock))
                    {
                        currentBlock.RotateCW();
                        currentBlock.RotateCW();
                        currentBlock.RotateCW();
                    }
                    break;
            }

            this.Invalidate(); // Sau khi người dùng bấm phím, vẽ lại màn hình ngay
            return true; // Báo cho WinForms biết: "Tôi đã bắt phím này rồi, đừng tít tít báo lỗi"
        }
    }
}
