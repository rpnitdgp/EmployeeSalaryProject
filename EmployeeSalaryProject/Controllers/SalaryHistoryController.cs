using EmployeeSalaryProject.Data;
using EmployeeSalaryProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmployeeSalaryProject.Controllers
{
    public class SalaryHistoryController : Controller
    {
        private readonly ApplicationDBContext _db;

        public SalaryHistoryController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<SalaryHistory> objList = _db.SalaryHistory;
            return View(objList);
        }

        //Get-Create
        public IActionResult Create()
        {
            //IEnumerable<Employee> objList = _db.Employees;

            //var model = new SalaryHistory();
            //model.DropDownList = new SelectList(objList, "EmpId", "FirstName");
            return View();
        }

        //POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SalaryHistory obj)
        {
            if (ModelState.IsValid)
            {
                Employee empObj = _db.Employees.Where(d => d.EmpID == obj.EmpID).FirstOrDefault();
                if(empObj == null)
                {
                    ModelState.AddModelError("EmpID", "Invalid Employee ID");
                    return View(obj);
                }
                if (obj.Year == DateTime.Now.Year && Int32.Parse(obj.Month) > DateTime.Now.Month)
                {
                    ModelState.AddModelError("Month", "Future Month can't be added");
                    return View(obj);
                }
                SalaryHistory salHistObj = CalculateSalaryComp(obj);
                _db.SalaryHistory.Add(salHistObj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? salaryId)
        {
            var obj = _db.SalaryHistory.Find(salaryId);
            if(obj == null)
            {
                return NotFound();
            }
            _db.SalaryHistory.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET-Delete
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.SalaryHistory.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(SalaryHistory obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Year == DateTime.Now.Year && Int32.Parse(obj.Month) > DateTime.Now.Month)
                {
                    ModelState.AddModelError("Month", "Future Month can't be added");
                    return View(obj);
                }
                SalaryHistory salHistObj = CalculateSalaryComp(obj);
                _db.SalaryHistory.Update(salHistObj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET-Update
        public IActionResult Update(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.SalaryHistory.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        public SalaryHistory CalculateSalaryComp(SalaryHistory salObj)
        {
            //Calculate Net Salary
            double workingHourComp = (double)salObj.WorkingHours / 40;
            double grossBasedOnHour = (salObj.GrossSalary) * workingHourComp;
            double incomeTax = (grossBasedOnHour * 36) / (100 * 12);
            double pensionAmount = (grossBasedOnHour) / (100 * 12);
            double labourTaxCredit = ((grossBasedOnHour - (incomeTax + pensionAmount)) / 2) / (100 * 12);
            double netSalary = ((grossBasedOnHour / 12)
                + labourTaxCredit) - (incomeTax + pensionAmount);

            //Assign Salary Components

            salObj.IncomeTax = Math.Round(incomeTax, 2);
            salObj.PensionAmount = Math.Round(pensionAmount, 2);
            salObj.LabourTaxCredit = Math.Round(labourTaxCredit, 2);
            salObj.SalaryAmount = Math.Round(netSalary, 2);

            return salObj;
        }

        //Download Employee To CSV File
        [HttpPost]
        public FileResult ExportToCSV()
        {
            #region Get list of Employee from Database

            SalaryHistory salHistory = new SalaryHistory();
            List<Object> listSalaryHistory = (from SalaryHistory in _db.SalaryHistory.ToList()
                                          select new[]
                                          {
                                              SalaryHistory.EmpID.ToString(),
                                              SalaryHistory.Month.ToString(),
                                              SalaryHistory.Year.ToString(),
                                              SalaryHistory.SalaryAmount.ToString(),
                                              SalaryHistory.GrossSalary.ToString(),
                                              SalaryHistory.IncomeTax.ToString(),
                                              SalaryHistory.PensionAmount.ToString(),
                                              SalaryHistory.LabourTaxCredit.ToString(),
                                              SalaryHistory.WorkingHours.ToString()
                                              
                                          }).ToList<object>();

            #endregion

            #region Create Name of Columns

            var names = typeof(SalaryHistory).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();

            listSalaryHistory.Insert(0, names.Where(x => x != names[0]).ToArray());

            #endregion

            #region Generate CSV

            StringBuilder sb = new StringBuilder();
            foreach (var item in listSalaryHistory)
            {
                string[] arrSalHistory = (string[])item;
                foreach (var data in arrSalHistory)
                {
                    //Append data with comma(,) separator.
                    sb.Append(data + ',');
                }
                //Append new line character.
                sb.Append("\r\n");
            }

            #endregion

            #region Download CSV

            return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "SalaryHistory.csv");

            #endregion
        }
    }
}
