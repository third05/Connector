using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Подключатор.ViewModel;
using Подключатор.Model;

namespace Подключатор.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Worker worker;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MainWindowViewModel.ChangeSelectMethod();
            if (e.NewValue is Worker)
                MainWindowViewModel.OnChangeValue((Worker)e.NewValue);
            else
            {
                worker = new Worker("", "");
                MainWindowViewModel.OnChangeValue(worker);
            }
            BackState();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ComputerNameBox.IsReadOnly = false;
            
            TexBoxName.BorderBrush = new SolidColorBrush(Color.FromRgb(157, 180, 250));
            TexBoxName.Background = new SolidColorBrush(Color.FromRgb(182, 244, 244));
            TexBoxName.Foreground = new SolidColorBrush(Colors.White);
            ComputerNameBox.BorderBrush = new SolidColorBrush(Color.FromRgb(157, 180, 250));
            ComputerNameBox.Background = new SolidColorBrush(Color.FromRgb(182, 244, 244));
            ComputerNameBox.Foreground = new SolidColorBrush(Colors.White);
            TexBoxName.IsReadOnly = false;
            ComputerNameBox.Focus();
            ComputerNameBox.SelectionStart = ComputerNameBox.Text.Length;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BackState();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BackState();
        }

        //private void TextBlockName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    TexBoxName.Focus();
        //    TexBoxName.SelectionStart = TexBoxName.Text.Length;
        //}

        private void BackState()
        {
            ComputerNameBox.IsReadOnly = true;
            TexBoxName.IsReadOnly = true;
            TexBoxName.BorderBrush = new SolidColorBrush(Color.FromRgb(240, 255, 255));
            TexBoxName.Background = new SolidColorBrush(Colors.Azure);
            TexBoxName.Foreground = new SolidColorBrush(Colors.Black);
            ComputerNameBox.BorderBrush = new SolidColorBrush(Color.FromRgb(240, 255, 255));
            ComputerNameBox.Background = new SolidColorBrush(Colors.Azure);
            ComputerNameBox.Foreground = new SolidColorBrush(Colors.Black);
            ComputerNameBox.IsReadOnly = true;
        }
        private void MWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}
