



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;



namespace Graphicapplication1
{

 

    class core
    {
        public Point p1;
        public Point p2;
        public core(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            
        }

    }
    class pointcode
    {
        public int x, y, z;
        public char[] code = new char[3];
    }

    class points
    {
        public pointcode p1;
        public pointcode p2;
        public points()
        {
            p1 = new pointcode();
            p2 = new pointcode();
        }
    }
}