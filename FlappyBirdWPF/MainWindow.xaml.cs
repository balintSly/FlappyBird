using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

namespace FlappyBirdWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += MainEventTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

        private void MainEventTimer(object? sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappybird), Canvas.GetTop(flappybird), flappybird.Width, flappybird.Height);
            Canvas.SetTop(flappybird, Canvas.GetTop(flappybird) + gravity);

            if (Canvas.GetTop(flappybird)<-10 || Canvas.GetTop(flappybird)>458)
            {
                Endgame();
            }
            
            
            foreach (var item in MainCanvas.Children.OfType<Image>())
            {
                if ((string)item.Tag=="obs1" || (string)item.Tag == "obs2" || (string)item.Tag == "obs3")
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - 5);
                    if (Canvas.GetLeft(item)<-100)
                    {
                        Canvas.SetLeft(item, 800);
                        score += 0.5;
                    }
                    Rect pipeHitbox = new Rect(Canvas.GetLeft(item), Canvas.GetTop(item), item.Width, item.Height);
                    if (flappyBirdHitBox.IntersectsWith(pipeHitbox))
                    {
                        Endgame();
                    }

                }
                if ((string)item.Tag=="cloud")
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item)-1);
                    if (Canvas.GetLeft(item) < -250)
                    {
                        Canvas.SetLeft(item, 550);
                    
                    }
                }
            }

        }

        DispatcherTimer gameTimer = new DispatcherTimer();
        double score;
        int gravity = 8;
        bool gameOver;
        Rect flappyBirdHitBox;
        
        private void MainCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Space)
            {
                flappybird.RenderTransform = new RotateTransform(-20, flappybird.Width / 2, flappybird.Height / 2);
                gravity = -8;
            }
            if (e.Key==Key.R && gameOver==true)
            {
                StartGame();
            }
        }

        private void MainCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                flappybird.RenderTransform = new RotateTransform(5, flappybird.Width / 2, flappybird.Height / 2);
                gravity = 8;
            }
        }
        private void StartGame()
        {
            MainCanvas.Focus();
            int temp = 300;
            score = 0;
            gameOver = false;
            Canvas.SetTop(flappybird, 190);
            foreach (var item in MainCanvas.Children.OfType<Image>())
            {
                if ((string)item.Tag=="obs1")
                {
                    Canvas.SetLeft(item, 500);
                }
                if ((string)item.Tag == "obs2")
                {
                    Canvas.SetLeft(item, 800);
                }
                if ((string)item.Tag == "obs3")
                {
                    Canvas.SetLeft(item, 1100);
                }
                if ((string)item.Tag == "cloud")
                {
                    Canvas.SetLeft(item, 300+temp);
                    temp = 800;
                }
            }
            gameTimer.Start();
        }
        private void Endgame()
        {
            gameTimer.Stop();
            gameOver = true;
            txtScore.Content += " Game Over! Press R to try again";
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
