using EmployeeSalaryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSalaryProject.Data
{
    public interface IApplicationDBContext
    {
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<EmployeeSalary> EmployeeSalary { get; set; }
        //public DbSet<SalaryHistory> SalaryHistory { get; set; }
    }
}