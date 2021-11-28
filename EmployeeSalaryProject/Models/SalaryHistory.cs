using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSalaryProject.Models
{
    public class SalaryHistory
    {
        [Key]
        [DisplayName("Salary Id")]
        public int SalaryID { get; set; }

        //Foreign Key
        [DisplayName("Emp Id")]
        public int EmpID { get; set; }

        [DisplayName("Salary Month")]
        [Required]
        [Range(1,12,ErrorMessage ="Month number should be between 1-12")]
        //[BindProperty, DataType(DataType.Date)]
        public string Month { get; set; }

        [DisplayName("Salary Year")]
        [Required]
        public int Year { get; set; }

        [DisplayName("Gross Salary")]
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Salary must be greater than 0!")]
        public double GrossSalary { get; set; }

        [DisplayName("Net Salary")]
        public double SalaryAmount { get; set; }

        [DisplayName("Income Tax")]
        public double IncomeTax { get; set; }

        [DisplayName("Pension Amount")]
        public double PensionAmount { get; set; }

        [DisplayName("Labour Tax Credit")]
        public double LabourTaxCredit { get; set; }

        [DisplayName("Working Hours/Week")]
        [Range(1, 40, ErrorMessage = "Working hours in a week must be a number and 40 or less!")]
        public int WorkingHours { get; set; }
    }   
}
