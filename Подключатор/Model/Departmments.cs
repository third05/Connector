using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Подключатор.Model
{
    public class Departmments
    {
        string Name { get; set; }

        private static ObservableCollection<Department> _departments;
        public static ObservableCollection<Department> Departments { get; set; }


        public Departmments()
        {

        }
        /// <summary>
        /// заполнение коллекции из файла
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Department> Complite2()
        {
            string json = File.ReadAllText("file_name.json");

            _departments = JsonConvert.DeserializeObject<ObservableCollection<Department>>(json);
            return _departments;
        }



    }
}
