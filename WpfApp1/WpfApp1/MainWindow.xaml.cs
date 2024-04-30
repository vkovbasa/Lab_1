using System;
using System.Threading;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // Ініціалізація масиву значень
        static double[] x = { 1, 2, 3, 4, 5 };
        static double[] y = { 7.1, 27.8, 62.1, 110, 161 };
        static int n = 0;
        // Підсумкові коефіцієнти рівняння
        static double a1, b1, a2, b2;
        // Похибка розрахунків
        static double d1, d2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (x.Length == y.Length)
            {
                n = x.Length;
            };

            Thread thread1 = new Thread(ThreadFunction1);
            Thread thread2 = new Thread(ThreadFunction2);

            thread1.Start();
            thread2.Start();
        }

        static void ThreadFunction1()
        {
            // Компоненти для вирішення системи
            double Xi = 0;
            double Xi2 = 0;
            double XiYi = 0;
            double Yi = 0;
            // Знайдемо необхідні компоненти для вирішення системи
            for (int i = 0; i < n; i++)
            {
                Xi += x[i];
                Xi2 += x[i] * x[i];
                XiYi += x[i] * y[i];
                Yi += y[i];
            }
            // Знайдемо підсумкові коефіцієнти і похибку
            a1 = (Yi * Xi2 * n - XiYi * n * Xi) / (Xi2 * n * n - n * Xi * Xi);
            b1 = (XiYi * n - Yi * Xi) / (Xi2 * n - Xi * Xi);
            d1 = Math.Sqrt(((Yi - a1 * Xi - b1) * (Yi - a1 * Xi - b1)) / (Yi * Yi));

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Result from Thread 1: a1 = {a1}, b1 = {b1}, d1 = {d1}");
            });
        }

        static void ThreadFunction2()
        {
            double Xi = 0;
            double Xi2 = 0;
            double XiYi = 0;
            double Yi = 0;
            // Нормалізація даних для y = a*x^b
            for (int i = 0; i < n; i++)
            {
                y[i] = Math.Log(y[i]);
            }
            // Знайдемо необхідні компоненти для вирішення системи
            for (int i = 0; i < n; i++)
            {
                Xi += x[i];
                Xi2 += x[i] * x[i];
                XiYi += x[i] * y[i];
                Yi += y[i];
            }
            // Знайдемо підсумкові коефіцієнти і похибку
            a2 = (Yi * Xi2 * n - XiYi * n * Xi) / (Xi2 * n * n - n * Xi * Xi);
            b2 = (XiYi * n - Yi * Xi) / (Xi2 * n - Xi * Xi);
            d2 = Math.Sqrt(((Yi - a2 * Xi - b2) * (Yi - a2 * Xi - b2)) / (Yi * Yi));

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Result from Thread 2: a2 = {a2}, b2 = {b2}, d2 = {d2}");
            });
        }
    }
}
