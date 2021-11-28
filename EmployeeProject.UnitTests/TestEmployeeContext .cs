using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeSalaryProject.Models;
using EmployeeSalaryProject.Data;

namespace EmployeeProject.UnitTests
{
    class TestEmployeeContext
    {
        public TestEmployeeContext()
        {
            this.Employees = new TestEmployeeDbSet();
        }

        public DbSet<Employee> Employees { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Employee item) { }
        public void Dispose() { }
    }
}
