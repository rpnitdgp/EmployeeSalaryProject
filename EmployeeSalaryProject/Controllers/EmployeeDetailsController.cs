using EmployeeSalaryProject.Data;
using EmployeeSalaryProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmployeeSalaryProject.Controllers
{
    public class EmployeeDetailsController : Controller
    {
        private readonly ApplicationDBContext _db;


        public EmployeeDetailsController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Employee> objList = _db.Employees;
            return View(objList);
        }

        //Get-Details
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var salaryHistoryObjList = _db.SalaryHistory.Where(d => d.EmpID == id).ToList();

            return View(salaryHistoryObjList);
        }

        //Get-Create
        public IActionResult Create()
        {
            return View();
        }

        //POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee obj)
        {
            if(obj != null)
            {
                int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                int dob = int.Parse(obj.DateOfBirth.ToString("yyyyMMdd"));
                int ageOfEmp = (now - dob) / 10000;
                obj.Age = ageOfEmp;
            }
            
            _db.Employees.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Create", "EmployeeSalary", obj);
        }

        //POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Employee obj)
        {
            var obj2 = _db.Employees.Find(obj.EmpID);
            //var flag=0;
            if (obj2 == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                obj2.WorkingHours = obj.WorkingHours;
                obj2.GrossSalary = obj.GrossSalary;
                _db.Employees.Update(obj2);
                _db.SaveChanges();
                    
                return RedirectToAction("Update", "EmployeeSalary", obj);
            }
            return View(obj2);
            
        }

        //GET-Update
        public IActionResult Update(int? id)
        {
            
            if (id == null || id == 0)
            {
                return NotFound();
            }
            if(_db != null)
            {
                var obj = _db.Employees.Find(id);
                if (obj == null)
                {
                    return NotFound();
                }
                return View(obj);
            }
            return NotFound();            
        }

        //POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? empId)
        {
            var obj = _db.Employees.Find(empId);
            if (obj == null)
            {
                return NotFound();
            }
            var salaryHistoryList = _db.SalaryHistory.Where(d => d.EmpID == empId).ToList();
            if(salaryHistoryList != null)
            {
                _db.SalaryHistory.RemoveRange(salaryHistoryList);
                
            }
            var emplyeeSalaryObj = _db.EmployeeSalary.Where(d => d.EmpID == empId).FirstOrDefault();
            if (emplyeeSalaryObj != null)
            {
                _db.EmployeeSalary.Remove(emplyeeSalaryObj);
            }
            _db.Employees.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET-Delete
        public IActionResult Delete(int? id)
        {
            Console.WriteLine("Value of emp id : " + id);
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Employees.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        //Download Employee To CSV File
        [HttpPost]
        public FileResult ExportToCSV()
        {
            //Get list of Employee from Database

            Employee employees = new Employee();
            List <Object> listEmployees = (from Employee in _db.Employees.ToList()
                                          select new[]
                                          {
                                              //Employee.EmpID.ToString(),
                                              Employee.FirstName,
                                              Employee.LastName,
                                              Employee.Age.ToString(),
                                              Employee.DateOfBirth.ToString(),
                                              Employee.WorkingHours.ToString(),
                                              Employee.GrossSalary.ToString()
                                          }).ToList<object>();

            //Create Name of Columns
            var names = typeof(Employee).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();

            listEmployees.Insert(0, names.Where(x => x != names[0]).ToArray());


            //Generate CSV
            StringBuilder sb = new StringBuilder();
            foreach (var item in listEmployees)
            {
                string[] arrEmployee = (string[])item;
                foreach (var data in arrEmployee)
                {
                    //Append data with comma(,) separator.
                    sb.Append(data + ',');
                }
                //Append new line character.
                sb.Append("\r\n");
            }


            //Download CSV
            return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Employees.csv");

        }

        //Download Employee Salary Details to CSV File
        [HttpPost]
        public FileResult ExportDetailsToCSV(int? empId)
        {
            if(empId != null){
                Console.WriteLine("Received id is : " + empId);
            }
            

            //Get list of Employee Salary History from Database
            SalaryHistory salaryHistory = new SalaryHistory();
            List<object> listSalaryHistory = 
                (from SalaryHistory in _db.SalaryHistory.Where(d => d.EmpID == empId).ToList()
                                          select new[]
                                          {
                                              SalaryHistory.EmpID.ToString(),
                                              SalaryHistory.Month.ToString(),
                                              SalaryHistory.Year.ToString(),
                                              SalaryHistory.GrossSalary.ToString(),
                                              SalaryHistory.SalaryAmount.ToString(),
                                              SalaryHistory.IncomeTax.ToString(),
                                              SalaryHistory.PensionAmount.ToString(),
                                              SalaryHistory.LabourTaxCredit.ToString(),
                                              SalaryHistory.WorkingHours.ToString()
                                        }).ToList<object>();

            //Create Name of Columns
            var names = typeof(SalaryHistory).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();

            listSalaryHistory.Insert(0, names.Where(x => x != names[0]).ToArray());


            //Generate CSV
            StringBuilder sb = new StringBuilder();
            foreach (var item in listSalaryHistory)
            {
                string[] arrSalaryHistory = (string[])item;
                foreach (var data in arrSalaryHistory)
                {
                    //Append data with comma(,) separator.
                    sb.Append(data + ',');
                }
                //Append new line character.
                sb.Append("\r\n");
            }

            //Download CSV
            return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "EmployeeSalaryHistory.csv");

        }
    }
}
