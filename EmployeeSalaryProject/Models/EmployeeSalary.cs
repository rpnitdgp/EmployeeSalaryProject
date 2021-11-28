using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSalaryProject.Models
{
    public class EmployeeSalary
    {
        public int Id { get; set; }

        [ForeignKey("EmpId")]
        public int EmpID { get; set; }

        [DisplayName("Net Salary")]
        public double NetSalary { get; set; }

        [DisplayName("Income Tax")]
        public double IncomeTax { get; set; }

        [DisplayName("Pension Amount")]
        public double PensionAmount { get; set; }

        [DisplayName("Labour Tax Credit")]
        public double LabourTaxCredit { get; set; }
    }
}
