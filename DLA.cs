using System;

namespace DLA
{
    /// <summary>
    /// Difusion Limited Aggregation
    /// </summary>
    class DLA
    {
        
        public byte[,] site;
        
        //
        public bool canPlot; // use this var for set bitamp
        public int iCurrnet, jCurrent;
        public int i0; // the index of array that we seed
        
        public int nWalkers;

        
        double R0 = 3;
        int l;
        bool isOnPerimeter;
        double r;
        
        Random rand = new Random();

        

        public DLA(int l, double R0)
        {
            this.l = l;
            this.R0 = R0;
            Init();
        }
        public DLA(int l)
        {
            this.l = l;
            Init();
        }

        void Init()
        {
            canPlot = true;
            site = new byte[l, l];
            i0 = l / 2;
            site[i0, i0] = 1; // occuiped seed site
            nWalkers = 1;
            iCurrnet = jCurrent = i0;
       
        }

        public void MoveNextWalker()
        {  
            double theta = 2 * Math.PI * rand.NextDouble();
            int x = i0 + (int)(R0 * Math.Cos(theta));
            int y = i0 + (int)(R0 * Math.Sin(theta));
            Walk(x, y);
            nWalkers++;      
        }

        void Walk(int x, int y)
        {
            double temp;
            int step, dx, dy;
            isOnPerimeter = false;
            
            do
            {
                dx = x - i0;
                dy = y - i0;
                r = Math.Sqrt(dx * dx + dy * dy);
                if (r <= R0 )
                    Test(x, y);  // test is walker on peimeter

                if (!isOnPerimeter)
                {
                    step = (int)(r - R0) - 1; // big step
                    if (step < 1) step = 1;

                    temp = rand.NextDouble();
                    if (temp < 0.25) x += step;
                    else if (temp < 0.5) x -= step;
                    else if (temp < 0.75) y += step;
                    else y -= step;
                }

            } while (!isOnPerimeter && r < 2 * R0);

            //
            canPlot = true; // just for plot in bitmap
            if (r >= 2 * R0) 
                canPlot = false;
            iCurrnet = x;
            jCurrent = y;
            //

        }

        void Test(int x, int y)
        {
            if (site[x + 1, y] + site[x - 1, y] + site[x, y + 1] + site[x, y - 1] > 0)
            {
                site[x, y] = 1;
                isOnPerimeter = true;
                if (r >= R0 - 1) R0 = (int)(r + 2);
            }

        }
    }
}
