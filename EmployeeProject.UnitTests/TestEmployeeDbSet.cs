using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeSalaryProject.Models;

namespace EmployeeProject.UnitTests
{
    class TestEmployeeDbSet : TestDbSet<Employee>
    {
        public override Employee Find(params object[] keyValues)
        {
            return this.SingleOrDefault(employee => employee.EmpID == (int)keyValues.Single());
        }
    }
}
