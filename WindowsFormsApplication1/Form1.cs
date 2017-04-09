using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace Perceptron
{
    public partial class Form1 : Form
    {
        public Form1()
        {  
            calc = new Calculation();
            InitializeComponent();
        }

        private Calculation calc;
        // размер сетки
        const int Ni = 15*15;
        // число нейронов
        const int Nj = 250;
        // число символов
        const int Nk = 15;
        // число нейрнов на выходе
        const int N = 3;

        private void button2_Click(object sender, EventArgs e){        
            // входной сигнал (массив с двоичным представлением символов)
            int[,] x = new int[Nk, Ni];

            // считываение символов из файлов
            Draws dr = new Draws();
            dr.ReadDraws(Ni, Nk, ref x);

            // обучение сети
            calc.Training(N, Ni, Nj, x, rtb1);
           
         }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            int[] x = new int[Ni];

            // считываение символа из файлов
            Draws dr = new Draws();
            dr.ReadDraw(openFileDialog1, pictureBox1, Ni, ref x);

            calc.Testing(N, Ni, Nj, x, rtb1);
        }

     }
}
