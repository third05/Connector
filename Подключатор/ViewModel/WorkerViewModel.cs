using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Подключатор.Model;

namespace Подключатор.ViewModel
{
    internal class WorkerViewModel : ViewModelBase
    {
        readonly ReadOnlyCollection<WorkerViewModel> _workers;
        readonly Department _department;

        private WorkerViewModel()
        {

        }
        public WorkerViewModel(Department department)
        {
            _department = department;

            _workers = new ReadOnlyCollection<WorkerViewModel>(
                (from employee in _department.Workers
                 select new WorkerViewModel()).ToList<WorkerViewModel>());
        }
    }
}
