using Libmas;
using System;
using System.IO;
using System.Windows;

namespace WpfApp13
{
    /// <summary>
    /// Логика взаимодействия для Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader streamReader = new StreamReader("config.ini");//сохранение настроек размера таблицы
                                                                           //в файл конфигурации «config.ini»
                using (streamReader)
                {
                    textBoxRowCount.Text = streamReader.ReadLine();
                    textBoxColumnCount.Text = streamReader.ReadLine();
                    textBoxDiapozonMin.Text = streamReader.ReadLine();
                    textBoxMaxDiapozon.Text = streamReader.ReadLine();
                }
            }
            catch (Exception)
            {
                textBoxColumnCount.Text = "0";
                textBoxRowCount.Text = "0";
                textBoxDiapozonMin.Text = "0";
                textBoxMaxDiapozon.Text = "0";
            }
        }

        private void ButtonSaveOptions_Click(object sender, RoutedEventArgs e)
        {
            if ((int.TryParse(textBoxColumnCount.Text, out int columnCount) && columnCount > 0) && (int.TryParse(textBoxRowCount.Text, out int rowCount) && rowCount > 0))
            {
                try
                {
                    StreamWriter streamWriter = new StreamWriter("config.ini"); //запись настроек размера таблицы
                                                                                //в файл конфигурации «config.ini»
                    using (streamWriter)
                    {
                        streamWriter.WriteLine(rowCount);
                        streamWriter.WriteLine(columnCount);
                        streamWriter.WriteLine(textBoxDiapozonMin.Text);
                        streamWriter.WriteLine(textBoxMaxDiapozon.Text);
                        Close();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Данные введены неверно", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Данные введены неверно, кол-во столбцов и кол-во строк должны быть больше 0");
            }
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonOptions_Click(object sender, RoutedEventArgs e)
        {
            Options opt = new Options();
            opt.ShowDialog();
        }        
    }
}

