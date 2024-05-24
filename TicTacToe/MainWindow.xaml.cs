using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        int xWins = 0;
        int oWins = 0;
        bool isXTurn = true;
        public string[,] board = new string[3, 3];
        static int moves;
        int maxDepth = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;

            if (IsCanvasEmpty(canvas))
            {
                int row = Grid.GetRow(canvas);
                int column = Grid.GetColumn(canvas);

                if (isXTurn)
                {
                    DrawX(canvas);
                    board[row, column] = "x";
                    moves++;

                    if (CheckWinner(board))
                    {
                        MessageBox.Show("X kazandi!");
                        xWins++;
                        ResetGame();
                    }
                    else if (moves == 9)
                    {
                        MessageBox.Show("Berabere");
                        ResetGame();
                    }
                    else
                    {
                        AITurn();
                    }
                }
            }
        }

        public void DrawX(Canvas canvas)
        {
            double canvasWidth = canvas.ActualWidth;
            double canvasHeight = canvas.ActualHeight;

            double thickness = 4;

            Line line1 = new Line
            {
                X1 = 20,
                Y1 = 20,
                X2 = canvasWidth - 20,
                Y2 = canvasHeight - 20,
                Stroke = Brushes.Red,
                StrokeThickness = thickness
            };

            Line line2 = new Line
            {
                X1 = canvasWidth - 20,
                Y1 = 20,
                X2 = 20,
                Y2 = canvasHeight - 20,
                Stroke = Brushes.Red,
                StrokeThickness = thickness
            };

            canvas.Children.Add(line1);
            canvas.Children.Add(line2);

            isXTurn = false;
        }

        public bool IsCanvasEmpty(Canvas canvas)
        {
            return !canvas.Children.Cast<UIElement>().Any();
        }

        private bool CheckWinner(string[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] != null && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                {
                    return true;
                }
                if (board[0, i] != null && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                {
                    return true;
                }
            }

            if (board[0, 0] != null && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            {
                return true;
            }
            if (board[0, 2] != null && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            {
                return true;
            }
            return false;
        }

        private void ResetGame()
        {
            isXTurn = true;
            moves = 0;
            ClearAllCanvases(myGrid);
        }

        public void ClearAllCanvases(Grid grid)
        {
            foreach (UIElement element in grid.Children)
            {
                if (element is Canvas canvas)
                {
                    canvas.Children.Clear();
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = null;
                }
            }
        }

        private void AITurn()
        {
            Tuple<int, int> bestMove = GetBestMove(board, maxDepth);

            int row = bestMove.Item1;
            int column = bestMove.Item2;
            Canvas canvas = myGrid.Children.Cast<UIElement>()
                                          .OfType<Canvas>()
                                          .First(c => Grid.GetRow(c) == row && Grid.GetColumn(c) == column);

            DrawO(canvas);

            board[row, column] = "o";
            moves++;

            if (CheckWinner(board))
            {
                MessageBox.Show("O kazandi!");
                ResetGame();
            }
            else if (moves == 9)
            {
                MessageBox.Show("Berabere");
                ResetGame();
            }
            else
            {
                isXTurn = true;
            }
        }

        public void DrawO(Canvas canvas)
        {
            double ellipseSize = Math.Min(canvas.ActualWidth, canvas.ActualHeight) * 0.8;

            Ellipse ellipse = new Ellipse
            {
                Width = ellipseSize,
                Height = ellipseSize,
                Stroke = Brushes.Red,
                StrokeThickness = 4
            };

            Canvas.SetLeft(ellipse, (canvas.ActualWidth - ellipseSize) / 2);
            Canvas.SetTop(ellipse, (canvas.ActualHeight - ellipseSize) / 2);

            canvas.Children.Clear();
            canvas.Children.Add(ellipse);
        }

        private Tuple<int, int> GetBestMove(string[,] currentBoard, int depth)
        {
            int bestScore = int.MinValue;
            Tuple<int, int> bestMove = null;

            if (moves == 1 && (board[0, 0] != null || board[2, 0] != null || board[0, 2] != null || board[2, 2] != null))
            {
                bestMove = Tuple.Create(1, 1);
                return bestMove;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string[,] newBoard1 = (string[,])currentBoard.Clone();
                    if (board[i, j]== null)
                    {
                        newBoard1[i, j] = "o";
                        if (CheckWinner(newBoard1))
                        {
                            bestMove = Tuple.Create(i, j);
                            return bestMove;
                        }
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string[,] newBoard1 = (string[,])currentBoard.Clone();
                    if (board[i, j] == null)
                    {
                        newBoard1[i, j] = "x";
                        if (CheckWinner(newBoard1))
                        {
                            bestMove = Tuple.Create(i, j);
                            return bestMove;
                        }
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string[,] newBoard1 = (string[,])currentBoard.Clone();
                    if (board[i, j] == null)
                    {
                        newBoard1[i, j] = "x";
                        for (int a = 0; a < 3; a++)
                        {
                            for (int b = 0; b < 3; b++)
                            {
                                if (newBoard1[a, b] == null)
                                {
                                    newBoard1[a, b] = "x";
                                    if (CheckWinner(newBoard1))
                                    {
                                        bestMove = Tuple.Create(a, b);
                                        return bestMove;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 3;i++)
            {
                for(int j = 0; j < 3;j++)
                {
                    string[,] newBoard1 = (string[,])currentBoard.Clone();
                    if (board[i, j]== null)
                    {
                        newBoard1[i, j] = "o";
                        for (int a = 0; a < 3; a++)
                        {
                            for (int b = 0; b < 3; b++)
                            {
                                if (newBoard1[a, b] == null)
                                {
                                    newBoard1[a, b] = "o";
                                    if (CheckWinner(newBoard1))
                                    {
                                        bestMove = Tuple.Create(a, b);
                                        return bestMove;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (currentBoard[i, j] == null)
                    {
                        string[,] newBoard2 = (string[,])currentBoard.Clone();
                        newBoard2[i, j] = "o";
                        int score = Minimax(newBoard2, depth - 1, false);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = Tuple.Create(i, j);
                        }
                    }
                }
            }

            return bestMove;
        }


        public int Minimax(string[,] currentboard, int depth, bool isMaximizing)
        {
            if (CheckWinner(currentboard) && isMaximizing)
            {
                return 1;
            }
            else if (CheckWinner(currentboard))
            {
                return -1;
            }
            else if (isBoardFull(currentboard) || depth == 0)
            {
                return 0;
            }


            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        if (currentboard[r, c] == null)
                        {
                            string[,] newBoard = (string[,])currentboard.Clone();
                            newBoard[r, c] = "x";
                            bestScore = Math.Max(bestScore, Minimax(newBoard, depth - 1, false));
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (currentboard[i, j] == null)
                        {
                            string[,] newBoard = (string[,])currentboard.Clone();
                            newBoard[i, j] = "o";
                            bestScore = Math.Min(bestScore, Minimax(newBoard, depth - 1, true));
                        }
                    }
                }
                return bestScore;
            }
        }

        public bool isBoardFull(string[,] _board)
        {
            int moves = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] != null)
                    {
                        moves++;
                    }
                }
            }

            if (moves == 9)
            {
                return true;
            }

            return false;
        }
    }
}
