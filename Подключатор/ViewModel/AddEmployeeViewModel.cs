using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Подключатор.Infrastructure;
using Подключатор.Model;

namespace Подключатор.ViewModel
{
    public class AddEmployeeViewModel : ViewModelBase
    {
        private string _name;
        private string _computerName;


        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        public string ComputerName { get { return _computerName; } set { _computerName = value; OnPropertyChanged("ComputerName"); } }


        #region кнопка ok

        RelayCommand _okCommand;
        public ICommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                    _okCommand = new RelayCommand(OkCommandExecute, CanOkCommandExecute);
                return _okCommand;
            }
        }
        /// <summary>
        /// передача данных нового пользователя
        /// </summary>
        /// <param name="parameter"></param>
        private void OkCommandExecute(object parameter)
        {
            MainWindowViewModel.AddEmployeeMethod(Name, ComputerName);
        }

        /// <summary>
        /// проверка выбран ли счет
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool CanOkCommandExecute(object parametr)
        {
            if(!String.IsNullOrEmpty(_name) && !String.IsNullOrEmpty(_computerName))
                return true;
            return false;
        }

        #endregion

    }
}
