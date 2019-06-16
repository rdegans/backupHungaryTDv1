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
    public class Tower
    {
        public Point Location;
        int towerType;
        Rectangle towerRect;
        Canvas cBackground;
        Canvas cObstacles;
        int[] positions;
        Point[] track;
        List<int> targets = new List<int>();
        int range;
        int cost;
        int bSpeed = 10;
        int bPower = 10;
        public Bullet bullet;
        public Tower(int tT, Canvas cBack, Canvas cObs, int[] p, Point[] t, Point l)
        {
            towerType = tT;
            towerRect = new Rectangle();
            cBackground = cBack;
            cObstacles = cObs;
            positions = p;
            track = t;
            Location = l;
            if (towerType == 0)//norm
            {
                range = 150;

                BitmapImage bi = new BitmapImage(new Uri("normal.png", UriKind.Relative));
                towerRect.Fill = new ImageBrush(bi);
                towerRect.Height = 35;
                towerRect.Width = 35;

            }
            else if (towerType == 1)//popo
            {
                range = 300;

                BitmapImage bi = new BitmapImage(new Uri("police.png", UriKind.Relative));
                towerRect.Fill = new ImageBrush(bi);
                towerRect.Height = 35;
                towerRect.Width = 35;

            }
            else if (towerType == 2)//fam
            {
                range = 50;

                BitmapImage bi = new BitmapImage(new Uri("family.png", UriKind.Relative));
                towerRect.Fill = new ImageBrush(bi);
                towerRect.Height = 35;
                towerRect.Width = 35;
            }
            else//thicc
            {
                range = 50;

                BitmapImage bi = new BitmapImage(new Uri("tank.png", UriKind.Relative));
                towerRect.Fill = new ImageBrush(bi);
                towerRect.Height = 35;
                towerRect.Width = 35;
            }

            Canvas.SetTop(towerRect, Location.Y - towerRect.Height / 2);
            Canvas.SetLeft(towerRect, Location.X - towerRect.Width / 2);
            cObstacles.Children.Add(towerRect);
            cBackground.Children.Remove(cObstacles);
            cBackground.Children.Add(cObstacles);

            double shortestDistance = 0;
            double startPosition = 0;
            for (int i = 0; i < positions.Length; i++)
            {
                double xDistance = 0;
                double yDistance = 0;

                xDistance = track[i].X - Location.X;
                yDistance = Location.Y - track[i].Y;

                double TotalDistance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));

                if (shortestDistance > TotalDistance || shortestDistance == 0)
                {
                    shortestDistance = TotalDistance;
                    startPosition = i;
                }
            }

            for (int i = (int)startPosition + range; i >= startPosition - range; i--)
            {
                targets.Add(i);
            }

            bullet = new Bullet(bSpeed, bPower, Location, cBackground);
        }
        public void Shoot()
        {
            Point frontEnemy = new Point(0, 0);
            for (int i = 0; i < targets.Count; i++)
            {
                if (positions[targets[i]] != -1 && bullet.bulletDrawn == false)
                {
                    bullet.DrawBullet(track[targets[i]]);
                    frontEnemy = track[targets[i]];
                    i = targets.Count;
                }
            }
            if (bullet.bulletDrawn)
            {
                bullet.DrawBullet(frontEnemy);
            }
        }
    }
}