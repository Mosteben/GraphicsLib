using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphicapplication1
{
    public partial class Form3 : Form
    {
        double preang = 0;
        int flag = 0;
        ArrayList row;

        Point corx1, corx2, cory1, cory2, newcen;
        bool click1 = true, click2 = false, dir = false, rot = true, abxy = false;
        List<core> cores = new List<core>();
        List<points> pos = new List<points>();

        int d = 0;
        int tx, ty;
        float sx, sy;
        double sin, cos, ang;
        char s;
        Bitmap p = null;
        Point locxy;
        Point locx1y1;


        void cors()
        {

            corx1.X = 0;
            corx1.Y = 675 / 2;
            corx2.X = 771;
            corx2.Y = 675 / 2;
            cory1.X = 771 / 2;
            cory1.Y = 0;
            cory2.X = 771 / 2;
            cory2.Y = 675;
           DDA(corx1, corx2);
           DDA(cory1, cory2);
          

        }
        private void Swap(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }
        private void Swapi(ref Point x, ref Point y)
        {
            Point temp = x;
            x = y;
            y = temp;
        }
        public void draw_axess(Point origin)
        {
            Point p1 = new Point(origin.X - 460, origin.Y);
            Point p2 = new Point(origin.X + 380, origin.Y);
            Point p3 = new Point(origin.X, origin.Y - 500);
            Point p4 = new Point(origin.X, origin.Y + 500);
            Pen p = new Pen(Color.Black, 7);
            Graphics g = pictureBox2.CreateGraphics();
            g.DrawLine(p, p1, p2);
            g.DrawLine(p, p3, p4);





        }
        private void revber(Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            Swapi(ref p1, ref p2);
            double derr = Math.Abs(dy / dx);
            double err = derr - .5;
            int y = p1.Y;
            for (int x = p1.X; x <= p2.X; x++)
            {
                p.SetPixel(x, y, Color.Green);
                err += derr;
                if (err >= 0)
                {
                    y += 1;
                    err -= 1.0;
                }

            }

        }
        private int calradius(Point CEN, Point p2)
        {
            int x;
            double dx = p2.X - CEN.X;
            double dy = p2.Y - CEN.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
                x = (int)Math.Abs(dx);
            else
                x = (int)Math.Abs(dy);
            return x;
        }
        private Point getmidpoint(Point CEN, Point p2)
        {
            double dx = p2.X + CEN.X;
            double dy = p2.Y + CEN.Y;

            CEN.X = (int)Math.Round(dx / 2);
            CEN.Y = (int)Math.Round(dy / 2);

            return CEN;
        }
        private void plot(int xcen, int ycen, int x, int y)
        {
            try
            {
                p.SetPixel(xcen + x, ycen + y, Color.Black);
                p.SetPixel(Math.Abs(xcen - x), ycen + y, Color.Black);
                p.SetPixel(xcen + x, Math.Abs(ycen - y), Color.Black);
                p.SetPixel(Math.Abs(xcen - x), Math.Abs(ycen - y), Color.Black);
            }
            catch { }
        }
        private void plotrot(int xcen, int ycen, int x, int y)
        {
            int xr, yr;
            try
            {

                xr = xcen + x;
                yr = ycen + y;
                rotate(ref xr, ref yr);
                p.SetPixel(xr, yr, Color.Black);



                xr = Math.Abs(xcen - x);
                yr = ycen + y;
                rotate(ref xr, ref yr);
                p.SetPixel(xr, yr, Color.Black);


                xr = xcen + x;
                yr = Math.Abs(ycen - y);
                rotate(ref xr, ref yr);
                p.SetPixel(xr, yr, Color.Black);


                xr = Math.Abs(xcen - x);
                yr = Math.Abs(ycen - y);
                rotate(ref xr, ref yr);
                p.SetPixel(xr, yr, Color.Black);
            }
            catch { }
        }
        void rotate(ref int x, ref int y)
        {
            int xo, yo, xn, yn;
            xo = x - newcen.X;
            yo = y - newcen.Y;
            xn = (int)Math.Round((xo * cos) + (yo * sin));
            yn = (int)Math.Round((yo * cos) - (xo * sin));
            x = xn + newcen.X;
            y = yn + newcen.Y;
        }
        private void p1_MouseClick(object sender, MouseEventArgs e)
        {


            if (click1)
            {
                locxy = e.Location;
                click2 = true;
                click1 = false;
                if (s == '5' && !abxy)
                {
                    click1 = true;
                    click2 = false;
                    ellipse();

                }


            }
            else if (click2)
            {
                locx1y1 = e.Location;
                click1 = true;
                click2 = false;


                switch (s)
                {
                    case '1':
                        cores.Add(new core(locx1y1, locxy));
                        DDA(locxy, locx1y1);
                        break;
                    case '2':
                        Bresenham(locxy, locx1y1);
                        break;
                    case '3':
                        MidCircle(getmidpoint(locxy, locx1y1), calradius(getmidpoint(locxy, locx1y1), locx1y1));
                        cores.Add(new core(locxy, locx1y1));
                        break;

                }

            }
        }

        


        public void DDA(Point p1, Point p2)
        {

            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "X";
            dataGridView1.Columns[1].Name = "Y";
            dataGridView1.Columns[2].Name = "polt";
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            double step, xinc, yinc, x = p1.X, y = p1.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                step = Math.Abs(dx);
            }
            else
            {
                step = Math.Abs(dy);
            }

            xinc = dx / step;
            yinc = dy / step;
            for (int i = 1; i <= step; i++)
            {
                row = new ArrayList();
                row.Add(x.ToString());
                row.Add(y.ToString());
                row.Add("(" + (Math.Round(x).ToString()) + "," + (Math.Round(y).ToString() + ")"));
                dataGridView1.Rows.Add(row.ToArray());
                try
                {
                    p.SetPixel((int)Math.Round(x), (int)Math.Round(y), Color.Red);
                    x = x + xinc;
                    y = y + yinc;
                }
                catch { }
            }
            row = new ArrayList();
            row.Add((cores.Count() + 1).ToString());
            row.Add("___________________");
            row.Add("___________________");
            row.Add("___________________");
            dataGridView1.Rows.Add(row.ToArray());
            pictureBox2.Image = p;


        }
        private void Bresenham(Point p1, Point p2)
        {
            double od;
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "K";
            dataGridView1.Columns[1].Name = "Pk";
            dataGridView1.Columns[2].Name = "Pk+1";
            dataGridView1.Columns[3].Name = "Xk+1";
            dataGridView1.Columns[4].Name = "Yk+1";
            if (p == null)
                p = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;

            int swaps = 0;
            if (dy > dx)
            {
                Swap(ref dx, ref dy);
                swaps = 1;
            }
            int a = Math.Abs(dy);
            int b = -Math.Abs(dx);

            double d = 2 * a + b;
            int x = p1.X;
            int y = p1.Y;
            p.SetPixel(Math.Abs(x), Math.Abs(y), Color.Black);

            int s = 1;
            int q = 1;
            if (p1.X > p2.X) q = -1;
            if (p1.Y > p2.Y) s = -1;
            od = d;
            for (int k = 0; k < dx; k++)
            {
                if (d >= 0)
                {
                    d = 2 * (a + b) + d;
                    y = y + s;
                    x = x + q;
                }
                else
                {
                    if (swaps == 1) y = y + s;
                    else x = x + q;
                    d = 2 * a + d;
                }
                try
                {
                    p.SetPixel(x, y, Color.Black);
                    row = new ArrayList();
                    row.Add(k.ToString());
                    row.Add(od.ToString());
                    row.Add(d.ToString());
                    row.Add(x.ToString());
                    row.Add(y.ToString());
                    dataGridView1.Rows.Add(row.ToArray());
                    od = d;
                }
                catch
                {

                }
            }



            if (dy < 0 && dx < 0)
                revber(p1, p2);
            cores.Add(new core(p1, p2));
            row = new ArrayList();
            row.Add((cores.Count() + 1).ToString());
            row.Add("___________________");
            row.Add("___________________");
            row.Add("___________________");
            dataGridView1.Rows.Add(row.ToArray());
            pictureBox2.Image = p;
        }
        private void MidCircle(Point CEN, int radius)
        {
            int k = 0, oe;
            // ArrayList row;
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "K";
            dataGridView1.Columns[2].Name = "Pk";
            dataGridView1.Columns[2].Name = "Pk+1";
            dataGridView1.Columns[3].Name = "Xk+1";
            dataGridView1.Columns[4].Name = "Yk+1";
            int x = radius;
            int y = 0;
            int err = 0;
            oe = err;
            while (x >= y)
            {
                try
                {
                    p.SetPixel(CEN.X + x, CEN.Y + y, Color.Black);
                    p.SetPixel(CEN.X + y, CEN.Y + x, Color.Black);
                    p.SetPixel(CEN.X - y, CEN.Y + x, Color.Black);
                    p.SetPixel(CEN.X - x, CEN.Y + y, Color.Black);
                    p.SetPixel(CEN.X - x, CEN.Y - y, Color.Black);
                    p.SetPixel(CEN.X - y, CEN.Y - x, Color.Black);
                    p.SetPixel(CEN.X + y, CEN.Y - x, Color.Black);
                    p.SetPixel(CEN.X + x, CEN.Y - y, Color.Black);
                }
                catch { }
                if (err <= 0)
                {
                    y += 1;
                    err += 2 * y + 1;
                }

                if (err > 0)
                {
                    x -= 1;
                    err -= 2 * x + 1;
                }
                row = new ArrayList();
                row.Add((k++).ToString());
                row.Add(oe.ToString());
                row.Add(err.ToString());
                row.Add(x.ToString());
                row.Add(y.ToString());
                dataGridView1.Rows.Add(row.ToArray());
                oe = err;
            }
            pictureBox2.Image = p;
            row = new ArrayList();
            row.Add((cores.Count() + 1).ToString());
            row.Add("____________________");
            row.Add("____________________");
            row.Add("____________________");
            row.Add("____________________");
            dataGridView1.Rows.Add(row.ToArray());
        }
        public void ellipse(int Xc = 0, int Yc = 0, int Rx = 0, int Ry = 0)
        {
            int k = 0;

            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "K";
            dataGridView1.Columns[1].Name = "Pk";
            dataGridView1.Columns[2].Name = "(Xk+1,Yk+1)";
            dataGridView1.Columns[3].Name = "2r^2Yk+1";
            dataGridView1.Columns[4].Name = "2r^2Xk+1";
            Point p1;
            int rx;
            int ry;
            int xc;
            int yc;
            if ((textBox3.Text == "" && textBox4.Text == "") && rot)
            {
                rx = int.Parse(textBox1.Text);
                ry = int.Parse(textBox2.Text);
                xc = locxy.X; ;
                yc = locxy.Y;
                p1 = locxy;
                p1.X = rx;
                p1.Y = ry;
                cores.Add(new core(locxy, p1));
            }
            else
            {
                rx = Rx;
                ry = Ry;
                xc = Xc;
                yc = Yc;
            }








            int rx2 = rx * rx;
            int ry2 = ry * ry;
            int tworx2 = 2 * rx2;
            int twory2 = 2 * ry2;
            int pe;
            int x = 0, y = ry, px = 0;
            int py = tworx2 * y;
            plot(xc, yc, x, y);

            row = new ArrayList();
            row.Add("region 1");
            row.Add("region 1");
            row.Add("region 1");
            row.Add("region 1");
            row.Add("region 1");
            dataGridView1.Rows.Add(row.ToArray());
            pe = Convert.ToInt32(Math.Round(ry2 - (rx2 * ry) + (.25 * rx2)));
            while (px < py)
            {
                x++;
                px += twory2;
                if (pe < 0)
                    pe += ry2 + px;
                else
                {
                    y--;
                    py -= tworx2;
                    pe += ry2 + px - py;
                }
                plot(xc, yc, x, y);
                row = new ArrayList();
                row.Add((k++).ToString());
                row.Add(pe.ToString());
                row.Add("( " + x + " " + y + " )");
                row.Add((twory2 * x).ToString());
                row.Add((tworx2 * y).ToString());
                dataGridView1.Rows.Add(row.ToArray());
            }

            pe = Convert.ToInt32(Math.Round(ry2 * (x + .5) * (x + .5) + rx2 * (y - 1) * (y - 1) - rx2 * ry2));
            row = new ArrayList();
            row.Add("region 2");
            row.Add("region 2");
            row.Add("region 2");
            row.Add("region 2");
            row.Add("region 2");
            dataGridView1.Rows.Add(row.ToArray());
            while (y > 0)
            {
                y--;
                py -= tworx2;
                if (pe > 0)
                    pe += rx2 - py;
                else
                {
                    x++;
                    px += twory2;
                    pe += rx2 - py + px;
                }
                plot(xc, yc, x, y);
                row = new ArrayList();
                row.Add((k++).ToString());
                row.Add(pe.ToString());
                row.Add("( " + x + " " + y + " )");
                row.Add((twory2 * x).ToString());
                row.Add((tworx2 * y).ToString());
                dataGridView1.Rows.Add(row.ToArray());
            }
            pictureBox2.Image = p;
            row = new ArrayList();
            row.Add((cores.Count() + 1).ToString());
            row.Add("___________________");
            row.Add("___________________");
            row.Add("___________________");
            row.Add("");
            dataGridView1.Rows.Add(row.ToArray());
        }
        public void ellipserot(int Xc = 0, int Yc = 0, int Rx = 0, int Ry = 0)
        {
            int k = 0;
            //ArrayList row;
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "K";
            dataGridView1.Columns[1].Name = "Pk";
            dataGridView1.Columns[2].Name = "(Xk+1,Yk+1)";
            dataGridView1.Columns[3].Name = "2r^2Yk+1";
            dataGridView1.Columns[4].Name = "2r^2Xk+1";
            Point p1;
            int rx;
            int ry;
            int xc;
            int yc;

            if ((textBox3.Text == "" && textBox4.Text == "") && rot)
            {
                rx = int.Parse(textBox1.Text);
                ry = int.Parse(textBox2.Text);
                xc = locxy.X;
                yc = locxy.Y;
                p1 = locxy;
                p1.X = rx;
                p1.Y = ry;
                cores.Add(new core(locxy, p1));
            }
            else
            {
                rx = Rx;
                ry = Ry;
                xc = Xc;
                yc = Yc;
            }



            int rx2 = rx * rx;
            int ry2 = ry * ry;
            int tworx2 = 2 * rx2;
            int twory2 = 2 * ry2;
            int pe;
            int x = 0, y = ry, px = 0;
            int py = tworx2 * y;
            plotrot(xc, yc, x, y);
            /*region 1*/
            row = new ArrayList();
            row.Add("region 1");
            row.Add("region 1");
            row.Add("region 1");
            row.Add("region 1");
            row.Add("region 1");
            dataGridView1.Rows.Add(row.ToArray());
            pe = Convert.ToInt32(Math.Round(ry2 - (rx2 * ry) + (.25 * rx2)));
            while (px < py)
            {
                x++;
                px += twory2;
                if (pe < 0)
                    pe += ry2 + px;
                else
                {
                    y--;
                    py -= tworx2;
                    pe += ry2 + px - py;
                }
                plotrot(xc, yc, x, y);
                row = new ArrayList();
                row.Add((k++).ToString());
                row.Add(pe.ToString());
                row.Add("( " + x + " " + y + " )");
                row.Add((twory2 * x).ToString());
                row.Add((tworx2 * y).ToString());
                dataGridView1.Rows.Add(row.ToArray());
            }
            /*region 2*/
            pe = Convert.ToInt32(Math.Round(ry2 * (x + .5) * (x + .5) + rx2 * (y - 1) * (y - 1) - rx2 * ry2));
            row = new ArrayList();
            row.Add("region 2");
            row.Add("region 2");
            row.Add("region 2");
            row.Add("region 2");
            row.Add("region 2");
            dataGridView1.Rows.Add(row.ToArray());
            while (y > 0)
            {
                y--;
                py -= tworx2;
                if (pe > 0)
                    pe += rx2 - py;
                else
                {
                    x++;
                    px += twory2;
                    pe += rx2 - py + px;
                }
                plotrot(xc, yc, x, y);
                row = new ArrayList();
                row.Add((k++).ToString());
                row.Add(pe.ToString());
                row.Add("( " + x + " " + y + " )");
                row.Add((twory2 * x).ToString());
                row.Add((tworx2 * y).ToString());
                dataGridView1.Rows.Add(row.ToArray());
            }
            pictureBox2.Image = p;
            row = new ArrayList();
            row.Add((cores.Count() + 1).ToString());
            row.Add("-------------------");
            row.Add("-------------------");
            row.Add("-------------------");
            row.Add("-------------------");
            dataGridView1.Rows.Add(row.ToArray());
        }







        private void DDA_clk(object sender, EventArgs e)
        {
            s = '1';
        }
        private void Drawax_clk(object sender, EventArgs e)
        {
            Point origin = new Point(382, 340);
            draw_axess(origin);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            s = '2';
        }
        private void button5_Click(object sender, EventArgs e)
        {
            p = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            cores.Clear();
            cors();
            dataGridView1.ColumnCount = 0;
            row = null;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            s = '3';
        }
        private void button7_Click(object sender, EventArgs e)
        {
            s = '5';
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (flag == 0)
            {

                if (s == '5')
                {
                    ang = 180.0;
                    ang += preang;
                    preang = ang;
                    sin = Math.Sin((ang * Math.PI) / 180);
                    cos = Math.Cos((ang * Math.PI) / 180);
                    p = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                }
                else
                {
                    ang = 180;
                    sin = Math.Sin((ang * Math.PI) / 180);
                    cos = Math.Cos((ang * Math.PI) / 180);
                    p = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                }


                int x1, x2, y1, y2;
                d = cores.Count();
                for (int i = 0; i < d; i++)
                {

                    if (s != '5')
                    {
                        x1 = cores[i].p1.X - newcen.X;
                        y1 = cores[i].p1.Y - newcen.Y;

                        cores[i].p1.X = (int)Math.Round((x1 * cos) + (y1 * sin));
                        cores[i].p1.Y = (int)Math.Round((y1 * cos) - (x1 * sin));
                        x2 = cores[i].p2.X - newcen.X;
                        y2 = cores[i].p2.Y - newcen.Y;
                        cores[i].p2.X = (int)Math.Round((x2 * cos) + (y2 * sin));
                        cores[i].p2.Y = (int)Math.Round((y2 * cos) - (x2 * sin));
                        cores[i].p2.X += newcen.X;
                        cores[i].p2.Y += newcen.Y;
                        cores[i].p1.X += newcen.X;
                        cores[i].p1.Y += newcen.Y;
                    }

                    switch (s)
                    {
                        case '1':
                        case '8':
                            DDA(cores[i].p1, cores[i].p2);
                            break;
                        case '2':
                            Bresenham(cores[i].p1, cores[i].p2);
                            break;
                        case '3':
                            MidCircle(cores[i].p1, calradius(cores[i].p1, cores[i].p2));
                            break;
                        case '4':
                            MidCircle(getmidpoint(cores[i].p1, cores[i].p2), calradius(getmidpoint(cores[i].p1, cores[i].p2), cores[i].p2));
                            break;
                        case '5':
                            rot = false;
                            ellipserot(cores[i].p1.X, cores[i].p1.Y, cores[i].p2.X, cores[i].p2.Y);
                            break;



                    }
                    if (s == '0')
                    {
                        DDA(cores[i].p1, cores[i].p2);
                    }
                }
                flag += 1;
            }
            else
            {
                if (s == '5')
                {
                    ang = -180.0;
                    ang += preang;
                    preang = ang;
                    sin = Math.Sin((ang * Math.PI) / 180);
                    cos = Math.Cos((ang * Math.PI) / 180);
                    p = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                }
                else
                {
                    ang = 180;
                    sin = Math.Sin((ang * Math.PI) / 180);
                    cos = Math.Cos((ang * Math.PI) / 180);
                    p = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                }


                int x1, x2, y1, y2;
                d = cores.Count();
                for (int i = 0; i < d; i++)
                {

                    if (s != '5')
                    {
                        x1 = cores[i].p1.X - newcen.X;
                        y1 = cores[i].p1.Y - newcen.Y;

                        cores[i].p1.X = (int)Math.Round((x1 * cos) + (y1 * sin));
                        cores[i].p1.Y = (int)Math.Round((y1 * cos) - (x1 * sin));
                        x2 = cores[i].p2.X - newcen.X;
                        y2 = cores[i].p2.Y - newcen.Y;
                        cores[i].p2.X = (int)Math.Round((x2 * cos) + (y2 * sin));
                        cores[i].p2.Y = (int)Math.Round((y2 * cos) - (x2 * sin));
                        cores[i].p2.X += newcen.X;
                        cores[i].p2.Y += newcen.Y;
                        cores[i].p1.X += newcen.X;
                        cores[i].p1.Y += newcen.Y;
                    }

                    switch (s)
                    {
                        case '1':
                        case '8':
                            DDA(cores[i].p1, cores[i].p2);
                            break;
                        case '2':
                            Bresenham(cores[i].p1, cores[i].p2);
                            break;
                        case '3':
                            MidCircle(cores[i].p1, calradius(cores[i].p1, cores[i].p2));
                            break;
                        case '4':
                            MidCircle(getmidpoint(cores[i].p1, cores[i].p2), calradius(getmidpoint(cores[i].p1, cores[i].p2), cores[i].p2));
                            break;
                        case '5':
                            rot = false;
                            ellipserot(cores[i].p1.X, cores[i].p1.Y, cores[i].p2.X, cores[i].p2.Y);
                            break;



                    }
                    if (s == '0')
                    {
                        DDA(cores[i].p1, cores[i].p2);
                    }
                }


            }
            textBox5.Text = " ";
            dir = false;
            rot = true;
            cors();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }








        private void button12_Click(object sender, EventArgs e)
        {
            rot = false;

            float xS = float.Parse(textBox6.Text);

            p = null;
            p = new Bitmap(pictureBox2.Width, pictureBox2.Height);


            d = cores.Count();
            for (int i = 0; i < d; i++)
            {
                if (s != '5')
                {
                    cores[i].p1.X = (int)Math.Round(cores[i].p1.Y + cores[i].p1.X * xS);
                    cores[i].p1.Y = (int)Math.Round(cores[i].p1.Y * 1.00);
                }


                cores[i].p2.X = (int)Math.Round(cores[i].p2.Y + cores[i].p2.X * xS);
                cores[i].p2.Y = (int)Math.Round(cores[i].p2.Y * 1.00);

                switch (s)
                {
                    case '1':
                    case '0':
                    case '8':
                        DDA(cores[i].p1, cores[i].p2);
                        break;
                    case '2':
                        Bresenham(cores[i].p1, cores[i].p2);
                        break;
                    case '3':
                        MidCircle(cores[i].p1, calradius(cores[i].p1, cores[i].p2));
                        break;
                    case '4':
                        MidCircle(getmidpoint(cores[i].p1, cores[i].p2), calradius(getmidpoint(cores[i].p1, cores[i].p2), cores[i].p2));
                        break;
                    case '5':
                        ellipse(cores[i].p1.X, cores[i].p1.Y, cores[i].p2.X, cores[i].p2.Y);
                        break;

                }
                textBox6.Text = " ";

                rot = true;
                cors();
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            rot = false;

            float yS = float.Parse(textBox7.Text);

            p = null;
            p = new Bitmap(pictureBox2.Width, pictureBox2.Height);


            d = cores.Count();
            for (int i = 0; i < d; i++)
            {
                if (s != '5')
                {
                    cores[i].p1.Y = (int)Math.Round(cores[i].p1.X + cores[i].p1.Y * yS);
                    cores[i].p1.X = (int)Math.Round(cores[i].p1.X * 1.00);
                }


                cores[i].p2.Y = (int)Math.Round(cores[i].p2.X + cores[i].p2.Y * yS);
                cores[i].p2.X = (int)Math.Round(cores[i].p2.X * 1.00);

                switch (s)
                {
                    case '1':
                    case '0':
                    case '8':
                        DDA(cores[i].p1, cores[i].p2);
                        break;
                    case '2':
                        Bresenham(cores[i].p1, cores[i].p2);
                        break;
                    case '3':
                        MidCircle(cores[i].p1, calradius(cores[i].p1, cores[i].p2));
                        break;
                    case '4':
                        MidCircle(getmidpoint(cores[i].p1, cores[i].p2), calradius(getmidpoint(cores[i].p1, cores[i].p2), cores[i].p2));
                        break;
                    case '5':
                        ellipse(cores[i].p1.X, cores[i].p1.Y, cores[i].p2.X, cores[i].p2.Y);
                        break;

                }
                textBox7.Text = " ";

                rot = true;
                cors();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 0;
            rot = false;

            tx = int.Parse(textBox3.Text);
            ty = int.Parse(textBox4.Text) * -1;

            p = null;
            p = new Bitmap(pictureBox2.Width, pictureBox2.Height);


            d = cores.Count();
            for (int i = 0; i < d; i++)
            {
                cores[i].p1.X = Math.Abs(cores[i].p1.X + tx);
                cores[i].p1.Y = Math.Abs(cores[i].p1.Y + ty);

                if (s != '5')
                {
                    cores[i].p2.X = Math.Abs(cores[i].p2.X + tx);
                    cores[i].p2.Y = Math.Abs(cores[i].p2.Y + ty);

                }
                switch (s)
                {
                    case '1':
                    case '0':
                    case '8':

                        DDA(cores[i].p1, cores[i].p2);
                        break;
                    case '2':
                        Bresenham(cores[i].p1, cores[i].p2);
                        break;
                    case '3':
                        MidCircle(cores[i].p1, calradius(cores[i].p1, cores[i].p2));
                        break;
                    case '4':
                        MidCircle(getmidpoint(cores[i].p1, cores[i].p2), calradius(getmidpoint(cores[i].p1, cores[i].p2), cores[i].p2));
                        break;
                    case '5':
                        ellipse(cores[i].p1.X, cores[i].p1.Y, cores[i].p2.X, cores[i].p2.Y);
                        break;

                }
                rot = true;
            }
            textBox3.Text = " "; textBox4.Text = " ";
            cors();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            rot = false;

            sx = float.Parse(textBox3.Text);
            sy = float.Parse(textBox4.Text);
            p = null;
            p = new Bitmap(pictureBox2.Width, pictureBox2.Height);


            d = cores.Count();
            for (int i = 0; i < d; i++)
            {
                if (s != '5')
                {
                    cores[i].p1.X = (int)Math.Round(cores[i].p1.X * sx);
                    cores[i].p1.Y = (int)Math.Round(cores[i].p1.Y * sy);
                }

                cores[i].p2.X = (int)Math.Round(cores[i].p2.X * sx);
                cores[i].p2.Y = (int)Math.Round(cores[i].p2.Y * sy);
                switch (s)
                {
                    case '1':
                    case '0':
                    case '8':
                        DDA(cores[i].p1, cores[i].p2);
                        break;
                    case '2':
                        Bresenham(cores[i].p1, cores[i].p2);
                        break;
                    case '3':
                        MidCircle(cores[i].p1, calradius(cores[i].p1, cores[i].p2));
                        break;
                    case '4':
                        MidCircle(getmidpoint(cores[i].p1, cores[i].p2), calradius(getmidpoint(cores[i].p1, cores[i].p2), cores[i].p2));
                        break;
                    case '5':
                        ellipse(cores[i].p1.X, cores[i].p1.Y, cores[i].p2.X, cores[i].p2.Y);
                        break;

                }
                textBox3.Text = " ";
                textBox4.Text = " ";
                rot = true;
                cors();
            }
        }

        private void button10_Click_1(object sender, EventArgs e)
        {


            if (s == '5')
            {
                ang = double.Parse(textBox5.Text);
                ang += preang;
                preang = ang;
                sin = Math.Sin((ang * Math.PI) / 180);
                cos = Math.Cos((ang * Math.PI) / 180);
                p = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            }
            else
            {
                ang = double.Parse(textBox5.Text);
                sin = Math.Sin((ang * Math.PI) / 180);
                cos = Math.Cos((ang * Math.PI) / 180);
                p = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            }


            int x1, x2, y1, y2;
            d = cores.Count();
            for (int i = 0; i < d; i++)
            {

                if (s != '5')
                {
                    x1 = cores[i].p1.X - newcen.X;
                    y1 = cores[i].p1.Y - newcen.Y;

                    cores[i].p1.X = (int)Math.Round((x1 * cos) + (y1 * sin));
                    cores[i].p1.Y = (int)Math.Round((y1 * cos) - (x1 * sin));
                    x2 = cores[i].p2.X - newcen.X;
                    y2 = cores[i].p2.Y - newcen.Y;
                    cores[i].p2.X = (int)Math.Round((x2 * cos) + (y2 * sin));
                    cores[i].p2.Y = (int)Math.Round((y2 * cos) - (x2 * sin));
                    cores[i].p2.X += newcen.X;
                    cores[i].p2.Y += newcen.Y;
                    cores[i].p1.X += newcen.X;
                    cores[i].p1.Y += newcen.Y;
                }

                switch (s)
                {
                    case '1':
                    case '8':
                        DDA(cores[i].p1, cores[i].p2);
                        break;
                    case '2':
                        Bresenham(cores[i].p1, cores[i].p2);
                        break;
                    case '3':
                        MidCircle(cores[i].p1, calradius(cores[i].p1, cores[i].p2));
                        break;
                    case '4':
                        MidCircle(getmidpoint(cores[i].p1, cores[i].p2), calradius(getmidpoint(cores[i].p1, cores[i].p2), cores[i].p2));
                        break;
                    case '5':
                        rot = false;
                        ellipserot(cores[i].p1.X, cores[i].p1.Y, cores[i].p2.X, cores[i].p2.Y);
                        break;



                }
                if (s == '0')
                {
                    DDA(cores[i].p1, cores[i].p2);
                }
            }
            textBox5.Text = " ";
            dir = false;
            rot = true;
            cors();
        }



        



       
      
       

     

       
     
   

      

        
        
        
     

      
      
        
     
      

        public Form3()
        {
            InitializeComponent();
            p = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            cors();
            dataGridView1.ColumnCount = 0;
            row = null;
            newcen.X = 771 / 2;
            newcen.Y = 675 / 2;

        }


        private void pictureBox2_Click(object sender, MouseEventArgs e)
        {

        }







        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
