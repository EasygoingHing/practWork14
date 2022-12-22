using Libmas;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }        

        int[,] myArray;

        //Событи кнопки "Выполнить"
        private void BtnDoATask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClassArray.FindToColums(myArray, tbResults);
            }
            catch (Exception)
            {
                MessageBox.Show("Таблица не создана");
            }
        }
        //Событие кнопки "Создать"
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            tbResults.Clear();

            try
            {
                int columnCount, rowCount;

                StreamReader streamReader = new StreamReader("config.ini");
                using (streamReader)
                {
                    rowCount = Convert.ToInt32(streamReader.ReadLine());
                    columnCount = Convert.ToInt32(streamReader.ReadLine());
                }

                if (rowCount <= 0 && columnCount <= 0)
                {
                    MessageBox.Show("Таблица не может быть размером 0х0!");
                }
                else
                {
                    myArray = new int[rowCount, columnCount];

                    dataGrid.ItemsSource = VisualArray.ToDataTable(myArray).DefaultView;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутсвует файл конфигурации, создайте его в настройках");
            }
        }

        //Событие кнопки "Сохранить"
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            save.Filter = "Все файлы (*.*) | *.* |Текстовые файлы | *.txt";
            save.FilterIndex = 2;
            save.Title = "Сохранение таблицы";

            if (save.ShowDialog() == true)
            {
                StreamWriter file = new StreamWriter(save.FileName);

                file.WriteLine(myArray.GetLength(0));
                file.WriteLine(myArray.GetLength(1));

                for (int i = 0; i < myArray.GetLength(0); i++)
                {
                    for (int j = 0; j < myArray.GetLength(1); j++)
                    {
                        file.WriteLine(myArray[i, j]);
                    }
                }
                file.Close();
            }
        }

        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Дана матрица размера M * N. Найти количество ее столбцов, элементы которых\r\nупорядочены по убыванию.\r\nВыполнил Крутолапов Валерий ИСП-31");
        }

        //Событие кнопки "Открыть"
        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".txt";
            open.Filter = "Все файлы (*.*) | *.* |Текстовые файлы | *.txt";
            open.FilterIndex = 2;
            open.Title = "Открытие таблицы";

            if (open.ShowDialog() == true)
            {
                StreamReader file = new StreamReader(open.FileName);

                int row = Convert.ToInt32(file.ReadLine());
                int column = Convert.ToInt32(file.ReadLine());

                myArray = new Int32[row, column];

                for (int i = 0; i < myArray.GetLength(0); i++)
                {
                    for (int j = 0; j < myArray.GetLength(1); j++)
                    {
                        myArray[i, j] = Convert.ToInt32(file.ReadLine());
                    }
                }

                file.Close();

                dataGrid.ItemsSource = VisualArray.ToDataTable(myArray).DefaultView;
            }
        }

        //Объявляем таймер
        DispatcherTimer timer;

        //Событие запуска окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //добавляем таймер
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.IsEnabled = true;

            WindowPassword pas = new WindowPassword();
            pas.Owner = this;//получение ссылки на родителя
            pas.ShowDialog();
        }

        private void ButtonOptions_Click(object sender, RoutedEventArgs e)
        {
            Options opt = new Options();
            opt.ShowDialog();
        }

        //Создание события таймера
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (myArray != null)
            {
                textBlockCurrentCell.Text = $"Разм.тб: {myArray.GetLength(0)}x{myArray.GetLength(1)}";
            }
            DataGrid x = (DataGrid)this.FindName("dataGrid");
            
            if (x.SelectedIndex != -1)
            {
                textBlockSizeTab.Text = $"Текущая ячейка: {x.SelectedIndex + 1}";
            }
        }

        //Событие кнопки "Очистка таблицы и результата при вводе новых данных"
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            tbResults.Clear();
            dataGrid.ItemsSource = null;
            myArray = null;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Вы уверены, что хотите выйти?",
                "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) Close();
        }

        //Событие кнопки "Заполнить"
        private void ButtonFill_Click(object sender, RoutedEventArgs e)
        {
            tbResults.Clear();

            try
            {
                int randMin, randMax, columnCount, rowCount;

                StreamReader streamReader = new StreamReader("config.ini");
                using (streamReader)
                {
                    rowCount = Convert.ToInt32(streamReader.ReadLine());
                    columnCount = Convert.ToInt32(streamReader.ReadLine());
                    randMin = Convert.ToInt32(streamReader.ReadLine());
                    randMax = Convert.ToInt32(streamReader.ReadLine());
                }

                Random rnd = new Random();

                if (randMin > randMax)
                {
                    MessageBox.Show("Минимальный диапозон не может быть больше максимального");
                }
                else if (rowCount <= 0 && columnCount <= 0)
                {
                    MessageBox.Show("Таблица не может быть размером 0х0!");
                }
                else
                {
                    myArray = new int[rowCount, columnCount];

                    for (int i = 0; i < myArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < myArray.GetLength(1); j++)
                        {
                            myArray[i, j] = rnd.Next(randMin, randMax + 1);
                        }
                    }

                    dataGrid.ItemsSource = VisualArray.ToDataTable(myArray).DefaultView;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутсвует файл конфигурации, создайте его в настройках");
            }
        }       
    }
}

