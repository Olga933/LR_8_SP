using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Thread main_thread;
        public Form1()
        {
            InitializeComponent();
            main_thread = Thread.CurrentThread;
            main_thread.Name = "Главный поток";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Имя домена: " + Thread.GetDomain().FriendlyName + "\n" +
                            "Идентификатор контекста: " + Thread.CurrentContext.ContextID + "\n" +
                            "Имя потока: " + main_thread.Name + "\n" +
                            "Запущен ли поток: " + main_thread.IsAlive + "\n" +
                            "Уровень приоритета: " + main_thread.Priority + "\n" +
                            "Состояние потока: " + main_thread.ThreadState);
        }

        DateTime start, end,start1,end1;
        Thread thread1;
        Thread thread2;
        Thread thread3;
        private void button1_Click_1(object sender, EventArgs e)
        {
            // создаём потоки
            thread1 = new Thread(new ThreadStart(DrawFirst));
            thread2 = new Thread(new ThreadStart(DrawSecond));
            thread3 = new Thread(new ThreadStart(GetInfo));
            //время начала
            //start = DateTime.Now;
            //start1 = DateTime.Now;
            //запускаем
            thread1.Start();
            thread2.Start();
            thread3.Start();



            //ожидаем окончания
            thread1.Join();
            thread2.Join();
            //end = DateTime.Now;
            textBox1.Text = (end - start).Seconds + " c. " + (end - start).Milliseconds + " мc.";
            
            //end1 = DateTime.Now;
            textBox2.Text = (end1 - start1).Seconds + " c. " + (end1 - start1).Milliseconds + " мc.";
            
            thread3.Join();
        }
           

           
        public static byte[,,] BmpToRgb(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[,,] res = new byte[3, height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color color = bmp.GetPixel(j, i);
                    res[0, i, j] = color.R;
                    res[1, i, j] = color.G;
                    res[2, i, j] = color.B;
                }
            }
            return res;
        }
        public static Bitmap RgbToBmp(byte[,,] mas)
        {
            int width = mas.GetLength(2),
            height = mas.GetLength(1);

            Bitmap res = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    res.SetPixel(j, i, Color.FromArgb(mas[0, i, j], mas[1, i, j], mas[2, i, j]));
                }
            }

            return res;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void DrawFirst()
        {
            start = DateTime.Now;
            // загрузили картинку
            Bitmap bmp = new Bitmap("D:\\учеба\\Системное программирование\\LR_8_SP\\LR8\\WindowsFormsApp1\\WindowsFormsApp1\\Cat.bmp");
            // создаем область рисования
            Graphics dc = pictureBox1.CreateGraphics();
            // получаем массив RGB для картинки
            byte[,,] mas = BmpToRgb(bmp);
            // высота и ширина
            int width = bmp.Width, height = bmp.Height;
            // освобождаем ресурсы
            bmp.Dispose();

            //разделим картинку на четыре равные части, создадим и заполним массивы каждой части
            byte[,,] mas1 = new byte[3, height / 2, width / 2];
            byte[,,] mas2 = new byte[3, height / 2, width / 2];
            byte[,,] mas3 = new byte[3, height / 2, width / 2];
            byte[,,] mas4 = new byte[3, height / 2, width / 2];

            for (int i = 0; i < height / 2; i++)
            {
                for (int j = 0; j < width / 2; j++)
                {
                    mas1[0, i, j] = mas[0, i, j];
                    mas1[1, i, j] = mas[1, i, j];
                    mas1[2, i, j] = mas[2, i, j];
                }
            }
            for (int i = 0; i < height / 2; i++)
            {
                for (int j = width / 2; j < width; j++)
                {
                    mas2[0, i, j - width / 2] = mas[0, i, j];
                    mas2[1, i, j - width / 2] = mas[1, i, j];
                    mas2[2, i, j - width / 2] = mas[2, i, j];
                }
            }
            for (int i = height / 2; i < height; i++)
            {
                for (int j = 0; j < width / 2; j++)
                {
                    mas3[0, i - height / 2, j] = mas[0, i, j];
                    mas3[1, i - height / 2, j] = mas[1, i, j];
                    mas3[2, i - height / 2, j] = mas[2, i, j];
                }
            }
            for (int i = height / 2; i < height; i++)
            {
                for (int j = width / 2; j < width; j++)
                {
                    mas4[0, i - height / 2, j - width / 2] = mas[0, i, j];
                    mas4[1, i - height / 2, j - width / 2] = mas[1, i, j];
                    mas4[2, i - height / 2, j - width / 2] = mas[2, i, j];
                }
            }

            // Далее сделаем обратное действие:
            // из массивов RGB получим картинки
            Bitmap img1 = RgbToBmp(mas1);
            Bitmap img2 = RgbToBmp(mas2);
            Bitmap img3 = RgbToBmp(mas3);
            Bitmap img4 = RgbToBmp(mas4);
            //составляем массив созданных массивов
            Bitmap[] imas = new Bitmap[] { img1, img2, img3, img4 };
            //правильная последовательность картинок
            int[] true_ = new int[] { 0, 1, 2, 3 };
            //текущий результат
            int[] res = new int[] {0, 1, 2, 3};
            //флаг окончания
            bool fl = true;
            while (fl)
            {
                //будем случайно перемешивать массив результата
                Random rand = new Random();

                for (int i = res.Length - 1; i >= 1; i--)
                {
                    int j = rand.Next(i + 1);

                    int tmp = res[j];
                    res[j] = res[i];
                    res[i] = tmp;
                }
                //проверяем, совпадает ли текцщий результат с настоящим
                if (res.SequenceEqual(true_))
                {
                    fl = false;
                }
                //рисуем текущую картинку
                dc.DrawImage(imas[res[0]], 0, 0, width / 2, height / 2);
                dc.DrawImage(imas[res[1]], width / 2, 0, width / 2, height / 2);
                dc.DrawImage(imas[res[2]], 0, height / 2, width / 2, height / 2);
                dc.DrawImage(imas[res[3]], width / 2, height / 2, width / 2, height / 2);
                // 1 секунду смотрим на картинку
                Thread.Sleep(300);
            }
            end = DateTime.Now;
        }
        private void DrawSecond()
        {
            start1 = DateTime.Now;
            // загрузили картинку
            Bitmap bmp = new Bitmap("D:\\учеба\\Системное программирование\\LR_8_SP\\LR8\\WindowsFormsApp1\\WindowsFormsApp1\\Dog.bmp");
            // создаем область рисования
            Graphics dc = pictureBox1.CreateGraphics();
            // получаем массив RGB для картинки
            byte[,,] mas = BmpToRgb(bmp);
            // высота и ширина
            int width = bmp.Width, height = bmp.Height;
            // освобождаем ресурсы
            bmp.Dispose();

            //разделим картинку на четыре равные части, создадим и заполним массивы каждой части
            byte[,,] mas1 = new byte[3, height / 2, width / 2];
            byte[,,] mas2 = new byte[3, height / 2, width / 2];
            byte[,,] mas3 = new byte[3, height / 2, width / 2];
            byte[,,] mas4 = new byte[3, height / 2, width / 2];

            for (int i = 0; i < height / 2; i++)
            {
                for (int j = 0; j < width / 2; j++)
                {
                    mas1[0, i, j] = mas[0, i, j];
                    mas1[1, i, j] = mas[1, i, j];
                    mas1[2, i, j] = mas[2, i, j];
                }
            }
            for (int i = 0; i < height / 2; i++)
            {
                for (int j = width / 2; j < width; j++)
                {
                    mas2[0, i, j - width / 2] = mas[0, i, j];
                    mas2[1, i, j - width / 2] = mas[1, i, j];
                    mas2[2, i, j - width / 2] = mas[2, i, j];
                }
            }
            for (int i = height / 2; i < height; i++)
            {
                for (int j = 0; j < width / 2; j++)
                {
                    mas3[0, i - height / 2, j] = mas[0, i, j];
                    mas3[1, i - height / 2, j] = mas[1, i, j];
                    mas3[2, i - height / 2, j] = mas[2, i, j];
                }
            }
            for (int i = height / 2; i < height; i++)
            {
                for (int j = width / 2; j < width; j++)
                {
                    mas4[0, i - height / 2, j - width / 2] = mas[0, i, j];
                    mas4[1, i - height / 2, j - width / 2] = mas[1, i, j];
                    mas4[2, i - height / 2, j - width / 2] = mas[2, i, j];
                }
            }

            // Далее сделаем обратное действие:
            // из массивов RGB получим картинки
            Bitmap img1 = RgbToBmp(mas1);
            Bitmap img2 = RgbToBmp(mas2);
            Bitmap img3 = RgbToBmp(mas3);
            Bitmap img4 = RgbToBmp(mas4);
            //составляем массив созданных масси
            Bitmap[] imas = new Bitmap[] { img1, img2, img3, img4 };
            //правильная последовательность картинок
            int[] true_ = new int[] { 0, 1, 2, 3 };
            //текущий результат
            int[] res = new int[] { 0, 1, 2, 3 };
            //флаг окончания
            bool fl = true;
            while (fl)
            {
                //будем случайно перемешивать массив результата
                Random rand = new Random();

                for (int i = res.Length - 1; i >= 1; i--)
                {
                    int j = rand.Next(i + 1);

                    int tmp = res[j];
                    res[j] = res[i];
                    res[i] = tmp;
                }
                //проверяем, совпадает ли текцщий результат с настоящим
                if (res.SequenceEqual(true_))
                {
                    fl = false;
                }
                //рисуем текущую картинку
                dc.DrawImage(imas[res[0]], 500, 0, width / 2, height / 2);
                dc.DrawImage(imas[res[1]], 500+width / 2, 0, width / 2, height / 2);
                dc.DrawImage(imas[res[2]], 500, height / 2, width / 2, height / 2);
                dc.DrawImage(imas[res[3]], 500+width / 2, height / 2, width / 2, height / 2);
                // 1 секунду смотрим на картинку
                Thread.Sleep(300);
            }
            end1 = DateTime.Now;
        }
        private void GetInfo()
        {
            string t = "Версия операционной системы: " + Environment.OSVersion + "\n" +
                          "Разрядность процессора: " + Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") + "\n" +
                          "Модель процессора: " + Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") + "\n" +
                          "Путь к системному каталогу:  " + Environment.SystemDirectory + "\n" +
                          "Число процессоров:  " + Environment.ProcessorCount + "\n" +
                          "Имя пользователя: " + Environment.UserName + "\n" +
                          "Имя машины: " + Environment.MachineName + "\n" +
                          "Код выхода из процесса: " + Environment.ExitCode + "\n" +
                          "Количество миллисекунд работы с последнего запуска: " + Environment.TickCount + "\n" +
                          "Процесс выполняется в режиме взаимодействия с пользователем: " + Environment.UserInteractive;
            Graphics dc = pictureBox2.CreateGraphics();
            thread1.Join();
            thread2.Join();
            dc.DrawString(t, new Font("Arial", 10), Brushes.Black, 0, 0);
        }
    }
  }
