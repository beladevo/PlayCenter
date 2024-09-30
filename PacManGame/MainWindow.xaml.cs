
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PacManGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();

        bool goLeft, goRight, goDown, goUp; 
        bool noLeft, noRight, noDown, noUp; 

        int speed = 8; 

        Rect pacmanHitBox; 

        int ghostSpeed = 10; 
        int ghostMoveStep = 160; 
        int currentGhostStep; 
        int score = 0;


        public MainWindow()
        {
            InitializeComponent();
            GameSetUp(); 
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
          

            if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goUp = goDown = false; 
                noRight = noUp = noDown = false; 
                goLeft = true;
                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2); 
            }

            if (e.Key == Key.Right && noRight == false)
            {
                noLeft = noUp = noDown = false; 
                goLeft = goUp = goDown = false;

                goRight = true; 

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2); 

            }

            if (e.Key == Key.Up && noUp == false)
            {
                noRight = noDown = noLeft = false;
                goRight = goDown = goLeft = false; 
                goUp = true;
                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2); 
            }

            if (e.Key == Key.Down && noDown == false)
            {
                noUp = noLeft = noRight = false; 
                goUp = goLeft = goRight = false; 
                goDown = true; 
                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2); 
            }


        }

        private void GameSetUp()
        {
            
            MyCanvas.Focus();
            gameTimer.Tick += GameLoop; 
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); 
            gameTimer.Start();
            currentGhostStep = ghostMoveStep; 

           
            ImageBrush pacmanImage = new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg"));
            pacman.Fill = pacmanImage;

            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.jpg"));
            redGuy.Fill = redGhost;

            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.jpg"));
            orangeGuy.Fill = orangeGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pink.jpg"));
            pinkGuy.Fill = pinkGhost;
        }

        private void GameLoop(object sender, EventArgs e)
        {

            txtScore.Text = "Score: " + score; 

            double canvasWidth = MyCanvas.ActualWidth;
            double canvasHeight = MyCanvas.ActualHeight;


            if (goRight)
            {
                double newPos = Canvas.GetLeft(pacman) + speed;
                if (newPos + pacman.Width <= canvasWidth)
                {
                    Canvas.SetLeft(pacman, newPos);
                }
            }
            if (goLeft)
            {
                double newPos = Canvas.GetLeft(pacman) - speed;
                if (newPos >= 0) 
                {
                    Canvas.SetLeft(pacman, newPos);
                }
            }
            if (goUp)
            {
                double newPos = Canvas.GetTop(pacman) - speed;
                if (newPos >= 0)
                {
                    Canvas.SetTop(pacman, newPos);
                }
            }
            if (goDown)
            {
                double newPos = Canvas.GetTop(pacman) + speed;
                if (newPos + pacman.Height <= canvasHeight) 
                {
                    Canvas.SetTop(pacman, newPos);
                }
            }
            

        
            if (goDown && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height)
            {
                noDown = true;
                goDown = false;
            }
            if (goUp && Canvas.GetTop(pacman) < 1)
            {
               
                noUp = true;
                goUp = false;
            }
            if (goLeft && Canvas.GetLeft(pacman) - 10 < 1)
            {
                noLeft = true;
                goLeft = false;
            }
            if (goRight && Canvas.GetLeft(pacman) + 70 > Application.Current.MainWindow.Width)
            {
                noRight = true;
                goRight = false;
            }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height); 


            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); 
              
                if ((string)x.Tag == "wall")
                {
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }
                   
                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }
                  
                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }
                   
                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }

                }

                if ((string)x.Tag == "coin")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        score++;
                    }

                }

                if ((string)x.Tag == "ghost")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        GameOver("Ghosts got you, click ok to play again");
                        return;
                    }

                    if (x.Name.ToString() == "orangeGuy")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);

                    }
                    else
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }

                    currentGhostStep--;

                    if (currentGhostStep < 1)
                    {
                        currentGhostStep = ghostMoveStep;
                        ghostSpeed = -ghostSpeed;

                    }
                }
            }


            if (score == 85)
            {
                GameOver("You Win, you collected all of the coins");
            }


        }

        private void GameOver(string message)
        {
            gameTimer.Stop();
            MessageBox.Show(message, "The Pac Man Game WPF MOO ICT"); 
            ResetGame();
        }

        private void ResetGame()
        {
            score = 0;
            txtScore.Text = "Score: " + score;
            Canvas.SetLeft(pacman, 50); 
            Canvas.SetTop(pacman, 104); 

            Canvas.SetLeft(redGuy, 173);
            Canvas.SetTop(redGuy, 29);
            Canvas.SetLeft(orangeGuy, 651);
            Canvas.SetTop(orangeGuy, 104);
            Canvas.SetLeft(pinkGuy, 173);
            Canvas.SetTop(pinkGuy, 404);

            foreach (var coin in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)coin.Tag == "coin")
                {
                    coin.Visibility = Visibility.Visible;
                }
            }


            goLeft = goRight = goUp = goDown = false;
            noLeft = noRight = noUp = noDown = false;

            currentGhostStep = ghostMoveStep;
            ghostSpeed = 10; 

            gameTimer.Start();
        }
    }
}