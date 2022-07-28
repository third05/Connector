using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Подключатор.Infrastructure;
using Подключатор.View;
using Подключатор.ViewModel;
using Newtonsoft.Json;
using System.Collections;

namespace Подключатор.Model
{
    public class Worker : IEquatable<Worker>, INotifyPropertyChanged
    {
        public static event AddNewUser AddNewEmployee;
        public delegate void AddNewUser(string name, string computerName);

        private string _name;
        private string _ip;
        private string _computerName;
        private bool _isSelected;
        private Brush _color;
        private Visibility _ellipseSetting;

        public static int counter = 1;
        [JsonIgnore]
        public Department Department { get; set; }
        public string Name { get { return _name; } set { _name = value;  OnPropertyChanged(nameof(Name)); } }
        public string ComputerName { get { return _computerName; } set { _computerName = value; OnPropertyChanged(nameof(ComputerName)); } }
        public Visibility EllipseSetting { get { return _ellipseSetting; } set { _ellipseSetting = value; OnPropertyChanged(nameof(EllipseSetting)); } }
        public Brush Color { get { return _color; } set { _color = value; OnPropertyChanged(nameof(Color)); } }
        public string Ip { get { return _ip; } set { _ip = value; OnPropertyChanged(nameof(Ip)); } }

        public Worker(string name, string computerName)
        {
            AddNewEmployee += NewUser;
            Name = name;
            ComputerName = computerName;
            _color = new SolidColorBrush(Colors.Gray);
            _ellipseSetting = Visibility.Visible;
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                    MainWindowViewModel.CheckSelectedEmployeeEvent(value);
                }
            }
        }

        /// <summary>
        /// сравнение экземляров класса
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Worker other)
        {
            return this.Name == other.Name && this.ComputerName == other.ComputerName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Task GetPing(Action action)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.InvokeAsync(action, DispatcherPriority.Input);
            });
            return task;
        }

        /// <summary>
        /// Команда пинг
        /// </summary>
        public void Ping()
        {
            EllipseSetting = Visibility.Visible;
            var task = GetPing(() =>
            {
                Ping P = new Ping();
                try
                {
                    PingReply Status = P.Send($"{_computerName}", 1);
                    P.Dispose();
                    Color = Status.Status == IPStatus.Success ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

                }
                catch
                {
                    Color = new SolidColorBrush(Colors.Red);
                }
            });
        }

        #region кнопка добавить сотрудника

        RelayCommand _addEmployeeCommand;
        [JsonIgnore]
        public ICommand AddEmployeeCommand
        {
            get
            {
                if (_addEmployeeCommand == null)
                    _addEmployeeCommand = new RelayCommand(AddEmployeeCommandExecute, CanAddEmployeeCommandExecute);
                return _addEmployeeCommand;
            }
        }
        /// <summary>
        /// открытие окна 
        /// </summary>
        /// <param name="parameter"></param>
        private void AddEmployeeCommandExecute(object parameter)
        {
            AddEmployee addEmployee = new AddEmployee();
            addEmployee.ShowDialog();
        }

        /// <summary>
        /// проверка выбран ли счет
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool CanAddEmployeeCommandExecute(object parametr)
        {
            return true;
        }
        public static void AddUserMethod(string name, string computerName)
        {
            AddNewEmployee?.Invoke(name, computerName);
        }
        public void NewUser(string name, string computerName)
        {
            Department.AddWorker(name, computerName);
        }
        #endregion
       
    }
}
