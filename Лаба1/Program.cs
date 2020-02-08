using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаба1
{
    public class Node
    {
       public int i, j;
        public float X, Y;
        public int CountR =0;
        public int CountTime = 0;
        public int RecoveryTime = -1;
        public int IntectionRate;
        public List<Node> Near = new List<Node>();

        public Node(int i, int j, float X, float Y)
        {
            this.i = i;
            this.j = j;
            this.X = X;
            this.Y = Y;
        }
        public Node(List<Node> Node)
        {
            this.Near = Node;
        }
        public bool S = true;
        public bool I;
        public bool R;
        public bool Rem = false;
        public int Sflag = 0, Iflag = 0, Rflag = 0;
        public bool RebPai;
        public bool RebRem;
   
    }
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

}
