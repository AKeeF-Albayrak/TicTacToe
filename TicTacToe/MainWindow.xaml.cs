using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isXTurn = true;
        public string[,] board = new string[3, 3];
        int moves;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;


            if(IsCanvasEmpty(canvas))
            {
                int row = Grid.GetRow(canvas);
                int column = Grid.GetColumn(canvas);

                if (isXTurn == false )
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
                    isXTurn = true;

                    board[row, column] = "o";
                    moves++;
                }
                else
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
                    board[row, column] = "x";
                    moves++;
                }


                if(CheckWinner())
                {
                    string winner = isXTurn ? "O" : "X";
                    MessageBox.Show($"{winner} kazandi!");
                    ResetGame();
                }
                else if(moves==9)
                {
                    MessageBox.Show("Berabere");
                    ResetGame();
                }
                else
                {
                }
            }
        }

        public bool IsCanvasEmpty(Canvas canvas)
        {
            return !canvas.Children.Cast<UIElement>().Any();
        }

        private bool CheckWinner()
        {
            for(int i =0; i<3; i++) 
            {
                if(board[i, 0] != null && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                {
                    return true;
                }
                if(board[0, i] != null && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                {
                    return true;
                }
            }

            if (board[0,0] != null && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
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
            for(int i =0;i<3;i++)
            {
                for (int j = 0; j < 3;j++)
                {
                    board[i, j] = null;
                }
            }
        }

    }
}
