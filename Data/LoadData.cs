using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SenwesAssignment_API.Data
{
    public class LoadData
    {
        private List<Employee> _empData;

        public LoadData()
        {
            _empData = null;
        }
        

        public IEnumerable<Employee> LoadEmployeeData()
        {
            if (_empData == null)
            {
                string jsonFilePath = @"Data\Employee.json";
                string json = File.ReadAllText(jsonFilePath);

                _empData = JsonConvert.DeserializeObject<List<Employee>>(json);
            }
            
            return _empData;
        }
    }
}
