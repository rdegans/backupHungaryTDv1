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
using System.IO;

namespace hungaryTDv1
{
    public class Bullet
    {
        //Tower parentTower;
        public int speed;
        int power;
        Point origin;
        public Point enemy;
        Canvas canvas;
        double xDistance;
        double yDistance;
        double xMove;
        double yMove;
        Point CurrentLocation;
        Rectangle sprite;
        public double NumbOfTranforms = 0;
        public bool bulletDrawn;
        public int counter = 1;
        public Polygon hitBox = new Polygon();

        public Bullet(int s, int p, Point o, Canvas c)
        {
            speed = s;
            power = p;
            origin = o;
            canvas = c;
            StreamReader sr = new StreamReader("forkBox.txt");
            PointCollection myPointCollection = new PointCollection();
            while (!sr.EndOfStream)
            {
                string currentLine = sr.ReadLine();
                double xPosition, yPosition;
                double.TryParse(currentLine.Split(',')[0], out xPosition);
                double.TryParse(currentLine.Split(',')[1], out yPosition);
                Point point = new Point(xPosition, yPosition);
                myPointCollection.Add(point);
            }
            sr.Close();
            hitBox.Points = myPointCollection;
            hitBox.Fill = Brushes.Red;
        }

        public Point DrawBullet(Point e)
        {
            if (bulletDrawn == false)
            {
                enemy = e;
                xDistance = 0;
                yDistance = 0;

                xDistance = enemy.X - origin.X;
                yDistance = origin.Y - enemy.Y;

                double TotalDistance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                NumbOfTranforms = Math.Ceiling(TotalDistance / speed);
                xMove = xDistance / NumbOfTranforms;
                yMove = yDistance / NumbOfTranforms;

                double temp = Math.Atan(xDistance / yDistance);
                double angle = temp * 180 / Math.PI;

                sprite = new Rectangle();
                sprite.Height = 20;
                sprite.Width = 10;
                BitmapImage bi = new BitmapImage(new Uri("fork.png", UriKind.Relative));
                sprite.Fill = new ImageBrush(bi);
                canvas.Children.Add(sprite);
                canvas.Children.Add(hitBox);

                if (enemy.Y > origin.Y)
                {
                    angle += 180;
                    RotateTransform rotate = new RotateTransform(angle);
                    sprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    sprite.RenderTransform = rotate;
                    hitBox.RenderTransformOrigin = new Point(0.5, 0.5);
                    hitBox.RenderTransform = rotate;
                }
                else
                {
                    RotateTransform rotate = new RotateTransform(angle);
                    sprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    sprite.RenderTransform = rotate;
                    hitBox.RenderTransformOrigin = new Point(0.5, 0.5);
                    hitBox.RenderTransform = rotate;
                }
                bulletDrawn = true;
                CurrentLocation = origin;
                return CurrentLocation;
            }
            else
            {
                CurrentLocation.X = origin.X + (xMove * counter);
                CurrentLocation.Y = origin.Y - (yMove * counter);
                Canvas.SetLeft(sprite, CurrentLocation.X);
                Canvas.SetTop(sprite, CurrentLocation.Y);
                Canvas.SetLeft(hitBox, CurrentLocation.X);
                Canvas.SetTop(hitBox, CurrentLocation.Y);
                counter++;
                if (CurrentLocation == enemy)
                {
                    canvas.Children.Remove(sprite);
                    canvas.Children.Remove(hitBox);
                    bulletDrawn = false;
                    counter = 1;
                }
                return CurrentLocation;
            }
        }
    }
}