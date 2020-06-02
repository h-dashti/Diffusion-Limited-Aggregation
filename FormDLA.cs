using System;
using System.Drawing;
using System.Windows.Forms;

namespace DLA
{
    public partial class FormDLA : Form
    {
        //
        #region Fields
        //int sizeBitMap;
        Bitmap bitMap;
        int xC_Bitmap, yC_BitMap;
        int sizeWalker = 1;
        DLA dla;
        int snapshots = 0;
        Brush brushWalker = Brushes.Red;
        Color[] colorArr = { Color.SkyBlue, Color.Violet, Color.Brown, Color.Red, Color.Orange,
         Color.Green, Color.Blue, Color.DarkBlue, Color.DarkKhaki, Color.DarkMagenta, Color.DarkOrange, Color.DarkRed,Color.Tomato, Color.Black};
   

        #endregion
        //

        //
        #region Private Methods
        void Reset()
        {
            int sizeBitMapMax = Math.Max(pictureBox1.Width, pictureBox1.Height);
            bitMap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            xC_Bitmap = bitMap.Width/2;
            yC_BitMap = bitMap.Height/2;
            int l = sizeBitMapMax / sizeWalker;
            dla = new DLA(4*l);
            label_nWalkers.Text = dla.nWalkers.ToString();
            BitMapSetPixel();
            pictureBox1.Invalidate();
            GC.Collect();
            
        }
        void DynamicSimulation()
        {
            int iteration = (int)numericUpDown1.Value;
            for (int i = 0; i < iteration; i++)
            {
                dla.MoveNextWalker();
                BitMapSetPixel();
            }
            label_nWalkers.Text = dla.nWalkers.ToString();
            pictureBox1.Invalidate();
        }

        void BitMapSetPixel()
        {
            if (!dla.canPlot)
                return;
            int di = dla.iCurrnet - dla.i0;
            int dj = dla.jCurrent - dla.i0;
            int x0 = xC_Bitmap + di * sizeWalker - sizeWalker / 2;
            int y0 = yC_BitMap + dj * sizeWalker - sizeWalker / 2;
            if (x0 >= bitMap.Width || x0 < 0 || y0 >= bitMap.Height || y0 < 0)
                return;

            int dr = (int)Math.Sqrt(di * di + dj * dj);
            Color colorWalker = colorArr[(dr / 20) % colorArr.Length];
            if (sizeWalker > 1)
            {
                Graphics g = Graphics.FromImage(bitMap);
                g.FillRectangle(brushWalker, new Rectangle(x0, y0, sizeWalker, sizeWalker));
                g.Dispose();
            }
            else // sizeWalker = 1;
            {
                bitMap.SetPixel(x0, y0, colorWalker);
            }


            
        }
        #endregion 
        //

        public FormDLA()
        {
            InitializeComponent();
            pictureBox1.Size = new Size(1200, 640);
            Reset();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            DynamicSimulation();
            
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void buttonDynamic_Click(object sender, EventArgs e)
        {
            if (buttonDynamic.Text == "Dynamic")
            {
                buttonDynamic.Text = "Stop";
                timer1.Start();
            }
            else
            {
                buttonDynamic.Text = "Dynamic";
                timer1.Stop();
            }

            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(bitMap, 0, 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DynamicSimulation();      
        }

        private void FormDLA_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer1 != null && timer1.Enabled)
                timer1.Stop();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "Designed by Hor Dashti (h.dashti2@gmail.com)";
            MessageBox.Show(text, "About");
        }

        
        private void buttonSnapshot_Click(object sender, EventArgs e)
        {
            string namefile = snapshots.ToString() + ".png";
            bitMap.Save(namefile);
            snapshots++;
        }

    }
}