using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Подключатор.ViewModel;

namespace Подключатор.Model
{
    public class Department : IEquatable<Department>, INotifyPropertyChanged
    {
        private string _name;

        private bool _isExpanded;
        private bool _isSelected;

        private ObservableCollection<Worker> _workers;

        public static int counter = 1;
        public int Id { get; set; }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }

        public ObservableCollection<Worker> Workers { get { return _workers; } set { _workers = value; OnPropertyChanged(nameof(Workers)); } }

        public Department(string name, ObservableCollection<Worker> workers)
        {
            Id = counter++;
            Name = name;
            Workers = workers;
        }

        /// <summary>
        /// раскрыт ли департамент
        /// </summary>
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
        /// <summary>
        /// добавления сотрудника
        /// </summary>
        /// <param name="name"></param>
        /// <param name="computerName"></param>
        public void AddWorker(string name, string computerName)
        {
            Worker worker = new Worker(name, computerName) { Department = this};
            Workers.Add(worker);
        }

        public void RemoveWorker(Worker worker)
        {
            Workers.Remove(worker);
        }

        /// <summary>
        /// Команда пинг сотрудников отдела
        /// </summary>
        public void CheckDepartment()
        {
            foreach (var item in Workers)
            {
                item.Ping();
            }
        }
        /// <summary>
        /// выбран ли департамент
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                    MainWindowViewModel.CheckSelectedDepartmentEvent(value);
                }
            }
        }
        /// <summary>
        /// сравнение экземпляров класса
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Department other)
        {
            return Id == other.Id && Name == other.Name && Workers == other.Workers;
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
