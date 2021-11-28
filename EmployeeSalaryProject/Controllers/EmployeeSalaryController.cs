using EmployeeSalaryProject.Data;
using EmployeeSalaryProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeSalaryProject.Controllers
{
    public class EmployeeSalaryController : Controller
    {
        private readonly ApplicationDBContext _db;

        public EmployeeSalaryController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<EmployeeSalary> objList = _db.EmployeeSalary;
            return View(objList);
        }

        //Get-Create
        public IActionResult Create(Employee obj)
        {
            CreatePost(obj);
            EmployeeSalary employeeSalary= _db.EmployeeSalary.Where(d => d.EmpID == obj.EmpID).FirstOrDefault();
            return View(employeeSalary);
        }

        //POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(Employee obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            var employeeSalaryObj = new EmployeeSalary();
            EmployeeSalary empSalObj = CalculateSalaryComp(employeeSalaryObj, obj);

            _db.EmployeeSalary.Add(empSalObj);
            _db.SaveChanges();
            return RedirectToAction("Index","EmployeeDetails");
        }

        //Get-Update
        public IActionResult Update(Employee obj)
        {
            UpdatePost(obj);
            EmployeeSalary employeeSalary = _db.EmployeeSalary.Where(d => d.EmpID == obj.EmpID).FirstOrDefault();
            return View(employeeSalary);
        }

        //POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePost(Employee obj)
        {
            if (obj == null)
            {
                return NotFound();
            }

            EmployeeSalary employeeSalaryObj = _db.EmployeeSalary.Where(d => d.EmpID == obj.EmpID).FirstOrDefault();
            EmployeeSalary empSalObj = CalculateSalaryComp(employeeSalaryObj, obj);

            _db.EmployeeSalary.Update(empSalObj);
            _db.SaveChanges();
            return RedirectToAction("Index", "EmployeeDetails");
        }


        public EmployeeSalary CalculateSalaryComp(EmployeeSalary employeeSalaryObj, Employee empObj)
        {
            //Calculate Net Salary
            int empID = empObj.EmpID;
            double workingHourComp = (double)empObj.WorkingHours / 40;
            double grossBasedOnHour = (empObj.GrossSalary) * workingHourComp;
            double incomeTax = (grossBasedOnHour * 36) / (100 * 12);
            double pensionAmount = (grossBasedOnHour) / (100 * 12);
            double labourTaxCredit = ((grossBasedOnHour - (incomeTax + pensionAmount)) / 2) / (100 * 12);
            double netSalary = ((grossBasedOnHour / 12) 
                + labourTaxCredit) - (incomeTax + pensionAmount);

            //Assign Salary Components
            if(employeeSalaryObj.EmpID == 0)
            {
                employeeSalaryObj.EmpID = empID;
            }
            employeeSalaryObj.IncomeTax = Math.Round(incomeTax, 2);
            employeeSalaryObj.PensionAmount = Math.Round(pensionAmount, 2);
            employeeSalaryObj.LabourTaxCredit = Math.Round(labourTaxCredit, 2);
            employeeSalaryObj.NetSalary = Math.Round(netSalary, 2);

            return employeeSalaryObj;
        }
    }
}
