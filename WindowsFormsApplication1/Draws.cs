using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Perceptron {
    class Draws {

        public void ReadDraws(int Ni, int Nk, ref int[,] x) {

            StreamWriter f = new StreamWriter("res.txt");
            int k;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();

            for (int l = 0; l < Nk; l++) {
                String s = dialog.SelectedPath + "\\" + (l + 1).ToString() + ".bmp";
                Bitmap Bmp = new Bitmap(s);
                Color color;
                k = 0;
                for (int j = 0; j < Bmp.Height; j++) {
                    for (int i = 0; i < Bmp.Width; i++) {
                        color = Bmp.GetPixel(i, j);
                        x[l, k] = ((color.R + color.B + color.G) < 50) ? 1 : 0;
                        f.Write(x[l, k] + " ");
                        k++;
                    }
                    f.WriteLine();
                }
                f.WriteLine();
                f.WriteLine();
            }
            f.Close();
        }

        public void ReadDraw(OpenFileDialog ofd, PictureBox pb, int Ni, ref int [] x) {
            ofd.ShowDialog();
            String s = ofd.FileName;
            pb.Load(s);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    
            int k = 0;       
            Bitmap Bmp = new Bitmap(s);
            Color color;

            for (int j = 0; j < Bmp.Height; j++) {
                for (int i = 0; i < Bmp.Width; i++) {
                    color = Bmp.GetPixel(i, j);
                    x[k] = ((color.R + color.B + color.G) < 50) ? 1 : 0;
                    k++;
                }
            }

        }


    }
}
