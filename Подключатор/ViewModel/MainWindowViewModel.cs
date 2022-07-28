using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Подключатор.Model;
using Подключатор.Infrastructure;
using Подключатор.View;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Threading;

namespace Подключатор.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public static event ChangeName ChNa;
        public delegate void ChangeName(Worker name);

        public static event CheckSelectedDepartment CheckSelectDepartment;
        public delegate void CheckSelectedDepartment(bool value);

        public static event CheckSelectedEmployee CheckSelectEmployee;
        public delegate void CheckSelectedEmployee(bool value);

        public static event AddNewEmployee AddNewEmployeeEvent;
        public delegate void AddNewEmployee(string name, string computerName);

        public static event Action ChangeSelectEmployee;

        private static bool _isExpanded;
        private static bool _isSelected;
        private static bool _isSelectedDepartment;
        private static bool _isSelectedEmployee;

        private int _countDepartment;
        private int _countEmployee;

        private Department _selectedDepartment;

        private ObservableCollection<Department> _departments;


        private Worker _employee;

        private string _computerName;

        private string _searchText = String.Empty;

        private string _numberComp;

        private bool _checkConnect;
        private delegate void CLickBtn();
        private string _tempName;
        private string _tempComputerName;




        public string SearchText { get { return _searchText; } set { _searchText = value; OnPropertyChanged("SearchText"); СountEmployee = 0; СountDepartment = 0; } }
        public string NumberComp { get { return _numberComp; } set { _numberComp = value; OnPropertyChanged("NumberComp"); } }
        public Worker Employee { get { return _employee; } set { _employee = value; ComputerName = _employee.ComputerName; OnPropertyChanged("Employee"); } }
        public string ComputerName { get { return _computerName; } set { _computerName = value; OnPropertyChanged("ComputerName"); } }
        public int СountDepartment { get { return _countDepartment; } set { _countDepartment = value; OnPropertyChanged(nameof(СountDepartment)); } }
        public int СountEmployee { get { return _countEmployee; } set { _countEmployee = value; OnPropertyChanged(nameof(СountEmployee)); } }
        public bool CheckConnect { get { return _checkConnect; } set { _checkConnect = value; OnPropertyChanged(nameof(CheckConnect)); } }


        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

            }
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
                }
            }
        }

        public bool IsSelectedDepartment
        {
            get { return _isSelectedDepartment; }
            set
            {
                if (value != _isSelectedDepartment)
                {
                    _isSelectedDepartment = value;
                    this.OnPropertyChanged("IsSelectedDepartment");
                }
            }
        }

        public bool IsSelectedEmployee
        {
            get { return _isSelectedEmployee; }
            set
            {
                if (value != _isSelectedEmployee)
                {
                    _isSelectedEmployee = value;
                    this.OnPropertyChanged("IsSelectedEmployee");
                }
            }
        }

        public ObservableCollection<Department> AllDepartments
        {
            get
            {
                if (_departments == null)
                {
                    _departments = Departmments.Complite2();
                }
                return _departments;
            }
            set
            {
                _departments = value;
                OnPropertyChanged(nameof(AllDepartments));
            }
        }

        public Department SelectedDepartment
        {
            get
            {
                return _selectedDepartment;
            }
            set
            {
                _selectedDepartment = value; OnPropertyChanged(nameof(SelectedDepartment));
            }
        }

        #region кнопка connect

        RelayCommand _connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                if (_connectCommand == null)
                    _connectCommand = new RelayCommand(ConnectCommandExecute, CanConnectCommandExecute);
                return _connectCommand;
            }
        }
        /// <summary>
        /// открытие окна 
        /// </summary>
        /// <param name="parameter"></param>
        private void ConnectCommandExecute(object parameter)
        {
            Process pc = new Process();
            pc.StartInfo.WorkingDirectory = "C:\\WINDOWS\\system32\\"; //путь для запуска файла "FIleName"
            pc.StartInfo.FileName = "cmd.exe";    //если UseShellExecute = true, указываем имя запускаемого файла 
            pc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pc.StartInfo.Arguments = $@"/k cd C:\Program Files (x86)\Microsoft Configuration Manager\AdminConsole\bin\i386 && CmRcViewer.exe {_computerName}";
            pc.Start();
        }

        /// <summary>
        /// проверка выбран ли счет
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool CanConnectCommandExecute(object parametr)
        {
            if(Employee != null)
            {
                if (!String.IsNullOrEmpty(Employee.ComputerName))
                    return true;
            }
            return false;
        }
        #endregion

        #region кнопка поиск

        RelayCommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new RelayCommand(SearchCommandExecute, CanSearchCommandExecute);
                return _searchCommand;
            }
        }
        /// <summary>
        /// открытие окна 
        /// </summary>
        /// <param name="parameter"></param>
        private void SearchCommandExecute(object parameter)
        {
            ChangeSelect();
            bool CheckFound = false;

            for (int i = СountDepartment; ; i++)
            {
                if (CheckFound)
                    break;
                for (int j = СountEmployee; ; j++)
                {
                    if (AllDepartments[i].Workers[j].Name.ToLower().StartsWith(SearchText.ToLower()) || AllDepartments[i].Workers[j].ComputerName.Contains(SearchText))
                    {
                        CheckFound = true;
                        if (j < AllDepartments[i].Workers.Count - 1)
                        {
                            СountEmployee = j + 1;
                            СountDepartment = i;
                        }
                        else
                        {
                            СountEmployee = 0;
                            if (i < AllDepartments.Count - 1)
                                СountDepartment = i + 1;
                            else
                                СountDepartment = 0;
                        }

                        Employee = AllDepartments[i].Workers[j];
                        AllDepartments[i].IsExpanded = true;
                        AllDepartments[i].Workers[j].IsSelected = true;
                        break;
                    }
                    if (j >= AllDepartments[i].Workers.Count - 1)
                    {
                        СountEmployee = 0;
                        break;
                    }
                }
                if (i >= AllDepartments.Count - 1 && !CheckFound)
                {
                    СountDepartment = 0;
                    break;
                }
            }
        }

        /// <summary>
        /// проверка выбран ли счет
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool CanSearchCommandExecute(object parametr)
        {
            if (!String.IsNullOrEmpty(SearchText))
                return true;
            return false;
        }
        #endregion

        #region кнопка добавить сотрудника

        RelayCommand _addEmployeeCommand;
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
            if (IsSelectedDepartment)
                return true;
            return false;
        }

        #endregion

        #region кнопка удалить сотрудника

        RelayCommand _removeEmployeeCommand;
        public ICommand RemoveEmployeeCommand
        {
            get
            {
                if (_removeEmployeeCommand == null)
                    _removeEmployeeCommand = new RelayCommand(RemoveEmployeeCommandExecute, CanRemoveEmployeeCommandExecute);
                return _removeEmployeeCommand;
            }
        }
        /// <summary>
        /// открытие окна 
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveEmployeeCommandExecute(object parameter)
        {
            foreach (var item in AllDepartments)
            {
                if (item.Workers.Contains(Employee))
                {
                    item.Workers.Remove(Employee);
                    break;
                }
            }
        }

        /// <summary>
        /// проверка выбран ли счет
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool CanRemoveEmployeeCommandExecute(object parametr)
        {
            if (IsSelectedEmployee)
                return true;
            return false;
        }

        #endregion

        #region кнопка edit

        RelayCommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(EditCommandExecute, CanEditCommandExecute);
                return _editCommand;
            }
        }
        /// <summary>
        /// открытие окна 
        /// </summary>
        /// <param name="parameter"></param>
        private void EditCommandExecute(object parameter)
        {
            _tempName = Employee.Name;
            _tempComputerName = Employee.ComputerName;

            //ObservableCollection<Department> tempDepartments = Departmments.Complite2();
            //if (AllDepartments != tempDepartments)
            //{
            //    AllDepartments = tempDepartments;
            //}
            //Thread thread = new Thread(EditMethod);
            //thread.Start();
        }

        /// <summary>
        /// проверка выбран ли счет
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool CanEditCommandExecute(object parametr)
        {
            return true;
        }
        #endregion

        #region кнопка save

        RelayCommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(SaveCommandExecute, CanSaveCommandExecute);
                return _saveCommand;
            }
        }
        /// <summary>
        /// открытие окна 
        /// </summary>
        /// <param name="parameter"></param>
        private void SaveCommandExecute(object parameter)
        {
            bool checkComplite = false;
            _tempName = String.Empty;
            _tempComputerName = String.Empty;
           
            string json = JsonConvert.SerializeObject(_departments);
            //File.WriteAllText("_listWorker.json", json);
            File.WriteAllText(@"W:\Третьяков\_listWorker.json", json);

        }

        /// <summary>
        /// проверка выбран ли счет
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool CanSaveCommandExecute(object parametr)
        {
            return true;
        }
        #endregion

        #region кнопка cancel

        RelayCommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(CancelCommandExecute, CanCancelCommandExecute);
                return _cancelCommand;
            }
        }
        /// <summary>
        /// открытие окна 
        /// </summary>
        /// <param name="parameter"></param>
        private void CancelCommandExecute(object parameter)
        {
            ChangeSelect();
        }

        /// <summary>
        /// проверка выбран ли счет
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool CanCancelCommandExecute(object parametr)
        {
            return true;
        }
        #endregion

        #region кнопка проверка доступности


        RelayCommand _checkConnectCommand;
        public ICommand CheckConnectCommand
        {
            get
            {
                if (_checkConnectCommand == null)
                    _checkConnectCommand = new RelayCommand(CheckConnectExecute, CanCheckConnectExecute);
                return _checkConnectCommand;
            }
        }
        /// <summary>
        /// открытие окна 
        /// </summary>
        /// <param name="parameter"></param>
        private async void CheckConnectExecute(object parameter)
        {
            for (int i = 0; i < AllDepartments.Count; i++)
            {
                for (int j = 0; j < AllDepartments[i].Workers.Count; j++)
                {
                    AllDepartments[i].Workers[j].Ping();
                }
            };
        }

        /// <summary>
        /// проверка выбран ли счет
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool CanCheckConnectExecute(object parametr)
        {
            return true;
        }
        #endregion

        #region кнопка проверка доступности отдела


        RelayCommand _checkConnectDepartmentCommand;
        public ICommand CheckConnectDepartmentCommand
        {
            get
            {
                if (_checkConnectDepartmentCommand == null)
                    _checkConnectDepartmentCommand = new RelayCommand(CheckConnectDepartmentExecute, CanCheckConnectDepartmentExecute);
                return _checkConnectDepartmentCommand;
            }
        }
        /// <summary>
        /// открытие окна 
        /// </summary>
        /// <param name="parameter"></param>
        private async void CheckConnectDepartmentExecute(object parameter)
        {
            bool check = true;
            for (int i = 0; i < AllDepartments.Count; i++)
            {
                if (!check)
                {
                    break;
                }
                if(AllDepartments[i].IsSelected && AllDepartments[i].IsExpanded)
                {
                    AllDepartments[i].CheckDepartment();
                    break;
                }
                else if (AllDepartments[i].IsExpanded)
                {
                    for (int j = 0; j < AllDepartments[i].Workers.Count; j++)
                    {
                        if (AllDepartments[i].Workers[j].IsSelected)
                        {
                            AllDepartments[i].CheckDepartment();
                            check = false;
                            break;
                        }
                    }
                }
            };
        }

        /// <summary>
        /// проверка выбран ли счет
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool CanCheckConnectDepartmentExecute(object parametr)
        {
            return true;
        }
        #endregion

        public void SearchBtn()
        {

            bool CheckFound = false;

            for (int i = СountDepartment; ; i++)
            {
                if (CheckFound)
                    break;
                for (int j = СountEmployee; ; j++)
                {
                    if (AllDepartments[i].Workers[j].Name.ToLower().StartsWith(SearchText.ToLower()))
                    {
                        CheckFound = true;
                        if (j < AllDepartments[i].Workers.Count - 1)
                        {
                            СountEmployee = j + 1;
                            СountDepartment = i;
                        }
                        else
                        {
                            СountEmployee = 0;
                            if (i < AllDepartments.Count - 1)
                                СountDepartment = i + 1;
                            else
                                СountDepartment = 0;
                        }

                        Employee = AllDepartments[i].Workers[j];
                        AllDepartments[i].IsExpanded = true;
                        AllDepartments[i].Workers[j].IsSelected = true;
                        break;
                    }
                    if (j >= AllDepartments[i].Workers.Count - 1)
                    {
                        СountEmployee = 0;
                        break;
                    }
                }
                if (i >= AllDepartments.Count - 1 && !CheckFound)
                {
                    СountDepartment = 0;
                    break;
                }
            }

        }
        


        public MainWindowViewModel()
        {
            ChNa += ChangeValue;
            AddNewEmployeeEvent += AddNewEmployeeMethod;
            CheckSelectDepartment += CheckSelectedDepartmentMethod;
            CheckSelectEmployee += CheckSelectedEmployeeMethod;
            ChangeSelectEmployee += ChangeSelect;
            CLickBtn _clickBtn = SearchBtn;
        }

        /// <summary>
        /// Запуск события изменения сотрудника
        /// </summary>
        /// <param name="name"></param>
        public static void OnChangeValue(Worker name)
        {
            ChNa?.Invoke(name);
        }
        /// <summary>
        /// Изменение сотрудника, данные которого отображаются
        /// </summary>
        /// <param name="name"></param>
        public void ChangeValue(Worker name)
        {
            Employee = name;
        }
        /// <summary>
        /// изменение значения выбора отдела в списке
        /// </summary>
        /// <param name="value"></param>
        public void CheckSelectedDepartmentMethod(bool value)
        {
            IsSelectedDepartment = value;
        }

        /// <summary>
        /// Запуск события проверки выделения отдела
        /// </summary>
        /// <param name="value"></param>
        public static void CheckSelectedDepartmentEvent(bool value)
        {
            CheckSelectDepartment?.Invoke(value);
        }

        /// <summary>
        /// изменение значения выбора сотрудника в списке
        /// </summary>
        /// <param name="value"></param>
        public void CheckSelectedEmployeeMethod(bool value)
        {
            IsSelectedEmployee = value;
        }
        /// <summary>
        /// Запуск события проверки выделения сотрудника
        /// </summary>
        /// <param name="value"></param>
        public static void CheckSelectedEmployeeEvent(bool value)
        {
            CheckSelectEmployee?.Invoke(value);
        }

        /// <summary>
        /// Метод принимабщий значения для передачи в метод добавления сотрудника
        /// </summary>
        /// <param name="name"></param>
        /// <param name="computerName"></param>
        public void AddNewEmployeeMethod(string name, string computerName)
        {
            foreach (var item in AllDepartments)
            {
                if (item.IsSelected)
                {
                    item.AddWorker(name, computerName);
                }
            }
            string json = JsonConvert.SerializeObject(AllDepartments);
            File.WriteAllText(@"file_path", json);
        }

        /// <summary>
        /// Метод для запуска события добавления сотрудника
        /// </summary>
        /// <param name="name"></param>
        /// <param name="computerName"></param>
        public static void AddEmployeeMethod(string name, string computerName)
        {
            AddNewEmployeeEvent?.Invoke(name, computerName);
        }

        /// <summary>
        /// запуск события изменения выбраного элеманта
        /// </summary>
        public static void ChangeSelectMethod()
        {
            ChangeSelectEmployee?.Invoke();
        }
        /// <summary>
        /// метод изменения выбранного элемента
        /// </summary>
        public void ChangeSelect()
        {
            if (!String.IsNullOrEmpty(_tempName) && !String.IsNullOrEmpty(_tempComputerName))
            {
                Employee.Name = _tempName;
                Employee.ComputerName = _tempComputerName;
            }
            _tempName = String.Empty;
            _tempComputerName = String.Empty;
        }


    }
}
