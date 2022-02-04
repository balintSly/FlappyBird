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
        #region fields
        DispatcherTimer gameTimer = new DispatcherTimer();
        double score;
        Rect flappyBirdHitBox;
        bool gameOver;
        //size dependent base height=490, base width=525
        //height depedent
        int gravity = 8;
        int flightMax = 458;
        int flightMin = 1;
        int flappyspawn = 190;
        double[] heightRatios = new double[4];

        //width dependent
        int obsmove = 5;
        int piperesetpoint = -100;
        int pipespawnpoint = 800;
        int cloudmove = 1;
        int cloudresetpoint=-250;
        int cloudspawnpoint = 550;
        int temp = 300;
        int obs1spawn = 500;
        int obs2spawn = 800;
        int obs3spawn = 1100;
        int cloud1spawn = 300;
        double[] widthRatios=new double[11];
        

        #endregion
        #region events
        private void MainEventTimer(object? sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappybird), Canvas.GetTop(flappybird), flappybird.Width, flappybird.Height);
            Canvas.SetTop(flappybird, Canvas.GetTop(flappybird) + gravity);

            if (Canvas.GetTop(flappybird) < -flightMin || Canvas.GetTop(flappybird) > flightMax)
            {
                Endgame();
            }


            foreach (var item in MainCanvas.Children.OfType<Image>())
            {
                if ((string)item.Tag == "obs1" || (string)item.Tag == "obs2" || (string)item.Tag == "obs3")
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - obsmove);
                    if (Canvas.GetLeft(item) < piperesetpoint)
                    {
                        Canvas.SetLeft(item, pipespawnpoint);
                        score += 0.5;
                    }
                    Rect pipeHitbox = new Rect(Canvas.GetLeft(item), Canvas.GetTop(item), item.Width, item.Height);
                    if (flappyBirdHitBox.IntersectsWith(pipeHitbox))
                    {
                        Endgame();
                    }

                }
                if ((string)item.Tag == "cloud")
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - cloudmove);
                    if (Canvas.GetLeft(item) < cloudresetpoint)
                    {
                        Canvas.SetLeft(item, cloudspawnpoint);

                    }
                }

            }

        }
        private void MainCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                flappybird.RenderTransform = new RotateTransform(-20, flappybird.Width / 2, flappybird.Height / 2);
                gravity = -gravity;
            }
            if (e.Key == Key.R && gameOver == true)
            {
                StartGame();
            }
        }
        private void MainCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                flappybird.RenderTransform = new RotateTransform(5, flappybird.Width / 2, flappybird.Height / 2);
                gravity = Math.Abs(gravity);
            }
        }
        #endregion
        #region methods
        private void StartGame()
        {
            MainCanvas.Focus();
            score = 0;
            gameOver = false;
            Canvas.SetTop(flappybird, flappyspawn);
            foreach (var item in MainCanvas.Children.OfType<Image>())
            {
                if ((string)item.Tag == "obs1")
                {
                    Canvas.SetLeft(item, obs1spawn);
                }
                if ((string)item.Tag == "obs2")
                {
                    Canvas.SetLeft(item, obs2spawn);
                }
                if ((string)item.Tag == "obs3")
                {
                    Canvas.SetLeft(item, obs3spawn);
                }
                if ((string)item.Tag == "cloud")
                {
                    Canvas.SetLeft(item, cloud1spawn + temp);
                    temp = obs2spawn;
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
        #endregion
        #region RESPONSIVE STUFF-----------------------------------------------------------------------------------------------
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (loaded)
            {
                int i = 0;
                foreach (var item in MainCanvas.Children.OfType<Image>())
                {
                    item.Height = Math.Round(ratios[i]* ((Window)sender).ActualHeight);
                    i++;
                    item.Width = Math.Round(ratios[i] * ((Window)sender).ActualWidth);
                    i++;
                    Canvas.SetTop(item, Math.Round(ratios[i] * ((Window)sender).ActualHeight));
                    i++;
                    Canvas.SetLeft(item, Math.Round(ratios[i] * ((Window)sender).ActualHeight));
                    i++;
                }
                gravity = (int)Math.Round(((Window)sender).ActualHeight * heightRatios[0]);
                  flightMax = (int)Math.Round(((Window)sender).ActualHeight * heightRatios[1]);
                flightMin = (int)Math.Round(((Window)sender).ActualHeight * heightRatios[2]);
                flappyspawn = (int)Math.Round(((Window)sender).ActualHeight * heightRatios[3]);

                obsmove = (int)Math.Round(((Window)sender).ActualWidth * widthRatios[0]);
                piperesetpoint = (int)Math.Round(((Window)sender).ActualWidth * widthRatios[1]);
                pipespawnpoint = (int)Math.Round(((Window)sender).ActualWidth * widthRatios[2]);
                cloudmove=(int)Math.Round(((Window)sender).ActualWidth * widthRatios[3]);
                cloudresetpoint = (int)Math.Round(((Window)sender).ActualWidth * widthRatios[4]);
                cloudspawnpoint = (int)Math.Round(((Window)sender).ActualWidth * widthRatios[5]);
                temp = (int)Math.Round(((Window)sender).ActualWidth * widthRatios[6]);
                obs1spawn = (int)Math.Round(((Window)sender).ActualWidth * widthRatios[7]);
                obs2spawn = (int)Math.Round(((Window)sender).ActualWidth * widthRatios[8]);
                obs3spawn = (int)Math.Round(((Window)sender).ActualWidth * widthRatios[9]);
                cloud1spawn = (int)Math.Round(((Window)sender).ActualWidth * widthRatios[10]);
            }                                                   
          
        }
        double[] ratios=new double[0];
        bool loaded = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            ratios=new double[MainCanvas.Children.Count*4];
            int i = 0;
            foreach (var item in MainCanvas.Children.OfType<Image>())
            {
                ratios[i]=(item.ActualHeight / ((Window)sender).ActualHeight);
                i++;
                ratios[i]=(item.ActualWidth / ((Window)sender).ActualWidth);
                i++;
                ratios[i] = (Canvas.GetTop(item) / ((Window)sender).ActualHeight);
                i++;
                ratios[i] = (Canvas.GetLeft(item) / ((Window)sender).ActualWidth);
                i++;
            }
            heightRatios[0]=gravity/ ((Window)sender).ActualHeight;
            heightRatios[1] = flightMax / ((Window)sender).ActualHeight;
            heightRatios[2]=flightMin/ ((Window)sender).ActualHeight;
            heightRatios[3] = flappyspawn / ((Window)sender).ActualHeight;

            widthRatios[0]= obsmove/ ((Window)sender).ActualWidth;
            widthRatios[1]= piperesetpoint / ((Window)sender).ActualWidth;
            widthRatios[2]= pipespawnpoint / ((Window)sender).ActualWidth;
            widthRatios[3]= cloudmove / ((Window)sender).ActualWidth;
            widthRatios[4]= cloudresetpoint / ((Window)sender).ActualWidth;
            widthRatios[5]= cloudspawnpoint / ((Window)sender).ActualWidth;
            widthRatios[6]= temp / ((Window)sender).ActualWidth;
            widthRatios[7]= obs1spawn / ((Window)sender).ActualWidth;
            widthRatios[8]= obs2spawn / ((Window)sender).ActualWidth;
            widthRatios[9]= obs3spawn / ((Window)sender).ActualWidth;
            widthRatios[10]= cloud1spawn / ((Window)sender).ActualWidth;

            loaded = true;
            ;
        }
        #endregion
    }
}
