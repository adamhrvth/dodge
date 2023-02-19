using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dodge
{
    class Asteroid
    {

        Rectangle recAsteroid;

        int iVelx;
        int iVelY;
        int iDiameter;

        SolidBrush asteroidBrush = new SolidBrush(Color.OrangeRed);

        static Random rn = new Random();

        public Asteroid(int iWidth, int iHeight, Rectangle recPlayer)
        {
            iDiameter = rn.Next(10, 31);

            recAsteroid = new Rectangle(rn.Next(0 ,iWidth - iDiameter),rn.Next(0, iHeight - iDiameter), iDiameter, iDiameter);

            while (recAsteroid.IntersectsWith(recPlayer))
            {
                recAsteroid = new Rectangle(rn.Next(0, iWidth - iDiameter), rn.Next(0, iHeight - iDiameter), iDiameter, iDiameter);
            }
           
            while(iVelx==0 && iVelY == 0)
            {
                iVelx = rn.Next(-10, 10);
                iVelY = rn.Next(-10, 10);
            }

        }


        public void Draw(Graphics sheet)
        {
            sheet.FillEllipse(asteroidBrush, recAsteroid);
        }

        public void Move()
        {
            recAsteroid.X += iVelx;
            recAsteroid.Y += iVelY;
        }

        public bool Intersects(Rectangle recPlayer)
        {
            bool bIntersectst = false;

            if (recPlayer.IntersectsWith(recAsteroid))
            {
                bIntersectst = true;
            }

            return bIntersectst;
        }

        public bool ReachedWall(int iWidth, int iHeight, int progressBarHeight)
        {
            bool bReachedWall = false;

            if (recAsteroid.Left <= 0 || recAsteroid.Right >= iWidth 
                 || recAsteroid.Top <= progressBarHeight || recAsteroid.Bottom >= iHeight)
            {
                bReachedWall = true;
            }
            return bReachedWall;
        }

    }
}
