using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSalaryProject.Models
{
    public class Employee
    {
        [Key]
        [DisplayName("Emp ID")]
        public int EmpID { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Employee Age")]
        public int? Age { get; set; }

        [DisplayName("Date Of Birth")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Working Hours/Week")]
        [Range(1,40,ErrorMessage ="Working hours in a week must be a number and 40 or less!")]
        public int WorkingHours { get; set; }

        [DisplayName("Gross Salary")]
        public double GrossSalary { get; set; }

    }
}
