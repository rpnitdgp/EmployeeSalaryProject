using EmployeeSalaryProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeSalaryProject.Data
{
    public class ApplicationDBContext :DbContext, IApplicationDBContext
    {
        private readonly ApplicationDBContext _db;
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalary { get; set; }
        public DbSet<SalaryHistory> SalaryHistory { get; set; }
    }
}
