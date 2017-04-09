using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;

namespace Perceptron {
    class Calculation {
        
        double[,] wij;
        double[,] wjk;

        public void Training(int Nk, int Ni, int Nj, int[,] x, RichTextBox rtb1) {
            // выходной сигнал
            double[] Y = new double[Nk];

            // синаптические веса
            wij = new double[Ni, Nj];
            wjk = new double[Nj, Nk];

            // входной слой
            int[] Oi = new int[Ni];
            // скрытый слой
            double[] Oj = new double[Nj];
            //выходной слой
            double[] Ok = new double[Nk];

            // составляющие ошибки
            double[] betak = new double[Nk];
            double[] betaj = new double[Nj];

            // пороги функций
            const double Hj = 14.5;
            const double Hk = 14.5;

            // параметры
            double mu1 = 0.3;
            double mu2 = 0.3;
            
            // число итераций
            int st = 0;
            // число успешных итераций
            int nok = 0;
            // допустимая погрешность
            double eps = 0.05; 
            // процент успешных итераций
            double procent; 

            double sum;
            int v, p, pr;

            // инициализация весов
            Random ran = new Random();
            for (int i = 0; i < Ni; i++)
                for (int j = 0; j < Nj; j++)
                    wij[i, j] = 0.001*ran.Next(1000);

            for (int j = 0; j < Nj; j++)
                for (int k = 0; k < Nk; k++)
                    wjk[j, k] = 0.001*ran.Next(1000);

            do {
                // выбираем случайный образ
                p = ran.Next(15);

                // входной слой
                for (int i = 0; i < Ni; i++)
                    Oi[i] = x[p, i];

                // определяем какой из нейронов на выходе должен сработать для данного образа
                if (p < 5)
                    v = 0;
                else if (p > 4 && p < 10)
                    v = 1;
                else
                    v = 2;

                for (int k = 0; k < Nk; k++)
                    Y[k] = (k == v) ? 1 : 0;

                // скрытый слой
                for (int j = 0; j < Nj; j++) {
                    sum = 0;
                    for (int i = 0; i < Ni; i++)
                        sum += wij[i, j]*Oi[i];
                    Oj[j] = (float) (1/(1 + Math.Exp(-sum/Hj)));
                }

                // выходной слой
                for (int k = 0; k < Nk; k++) {
                    sum = 0;
                    for (int j = 0; j < Nj; j++)
                        sum += wjk[j, k]*Oj[j];
                    Ok[k] = (float) (1/(1 + Math.Exp(-sum/Hk)));
                }

                // вычисление ошибки
                for (int k = 0; k < Nk; k++)
                    betak[k] = Y[k] - Ok[k];

                // корректировка весов
                for (int j = 0; j < Nj; j++)
                    for (int k = 0; k < Nk; k++)
                        wjk[j, k] += mu1*Oj[j]*betak[k];

                for (int j = 0; j < Nj; j++) {
                    sum = 0;
                    for (int k = 0; k < Nk; k++)
                        sum += wjk[j, k]*betak[k];
                    betaj[j] = Oj[j]*(1 - Oj[j])*sum;
                }

                for (int i = 0; i < Ni; i++)
                    for (int j = 0; j < Nj; j++)
                        wij[i, j] += mu2*Oi[i]*betaj[j];


                // вычисление погрешности
                pr = 1;
                for (int k = 0; k < Nk; k++)
                    if (Math.Abs(Y[k] - Ok[k]) > eps)
                        pr = 0;

                if (pr != 0)
                    nok++;

                st++;
                procent = (float) nok/st*100;

            } while (st < 10000);

            rtb1.AppendText("Сеть обучена на " + procent.ToString() + "%");
        }

        public void Testing(int Nk, int Ni, int Nj, int[] x, RichTextBox rtb1) {
            int[] Oi = new int[Ni];
            double[] Oj = new double[Nj];
            double[] Ok = new double[Nk];
            double sum;
            const double Hj = 14.5;
            const double Hk = 14.5;

            for (int i = 0; i < Ni; i++)
                Oi[i] = x[i];

            for (int j = 0; j < Nj; j++) {
                sum = 0;
                for (int i = 0; i < Ni; i++)
                    sum += wij[i, j]*Oi[i];
                Oj[j] = (float) (1/(1 + Math.Exp(-sum/Hj)));
            }

            for (int k = 0; k < Nk; k++) {
                sum = 0;
                for (int j = 0; j < Nj; j++)
                    sum += wjk[j, k]*Oj[j];
                Ok[k] = (float) (1/(1 + Math.Exp(-sum/Hk)));
            }

            int v = 5;

            for (int k = 0; k < Nk; k++)
                if (Ok[k] > 0.95)
                    v = k;

            switch (v) {
                case 0: rtb1.Text = "Это буква Б";
                    break;
                case 1: rtb1.Text = "Это буква С";
                    break;
                case 2: rtb1.Text = "Это цифра 4";
                    break;
                default:
                    rtb1.Text = "Символ не распознан";
                    break;
            }     
        }
    }
}
