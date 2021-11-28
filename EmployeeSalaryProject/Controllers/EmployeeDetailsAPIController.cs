using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSalaryProject.Models;
using EmployeeSalaryProject.Data;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using AutoMapper;
using System.Net;
using System.Text;

namespace EmployeeSalaryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDetailsAPIController : ControllerBase
    {
        private readonly ApplicationDBContext _repository;

        public EmployeeDetailsAPIController(ApplicationDBContext repository)
        {
            _repository = repository;
        }


        //GET api/EmployeeDetailsAPI
        [HttpGet]
        public ActionResult<Employee> GetAllEmployees()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var employeeDetails = _repository.Employees.ToList();
                return Ok(employeeDetails);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error getting employee details from the Application");
            }

        }

        //Post api/EmployeeDetailsAPI/CreateEmployee
        [Route("/api/EmployeeDetailsAPI/CreateEmployee")]
        [HttpPost]        
        public ActionResult<Employee> CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                EmployeeSalary empSalaryObj = new EmployeeSalary();
                EmployeeDetailsAPIMethods empDetailMethods = new EmployeeDetailsAPIMethods(_repository);

                //Call to create new Employee in DB
                employee = empDetailMethods.createEmployeeDetails(employee);

                //Call to Create Employee Salary Component data in DB
                empSalaryObj = empDetailMethods.createEmployeeSalary(employee, empSalaryObj);

                return Ok(employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Creating employee in the Application");
            }
        }

        //Post api/EmployeeDetailsAPI/UpdateEmployee
        [Route("/api/EmployeeDetailsAPI/UpdateEmployee")]
        [HttpPost]
        public ActionResult<Employee> UpdateEmployee([FromBody] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (employee.EmpID == null || employee.EmpID == 0)
                {
                    return BadRequest();
                }

                if (employee.WorkingHours == 0 || employee.GrossSalary == 0)
                {
                    return BadRequest();
                }

                EmployeeSalary empSalaryObj = new EmployeeSalary();
                EmployeeDetailsAPIMethods empDetailMethods = new EmployeeDetailsAPIMethods(_repository);

                //Call to update Employee in DB
                employee = empDetailMethods.updateEmployeeDetails(employee);

                return Ok(employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating employee in the Application");
            }
        }

        //DELETE api/EmployeeDetailsAPI/DeleteEmployee
        [Route("/api/EmployeeDetailsAPI/DeleteEmployee/{id}")]
        [HttpDelete("{id:int}")]
        public ActionResult<String> DeleteEmployee(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (id == null || id == 0)
                {
                    return BadRequest();
                }

                EmployeeSalary empSalaryObj = new EmployeeSalary();
                EmployeeDetailsAPIMethods empDetailMethods = new EmployeeDetailsAPIMethods(_repository);

                //Call to delete Employee in DB
                Employee resultObj = empDetailMethods.deleteEmployeeDetails(id);

                return Ok(resultObj);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting employee from the Application");
            }
        }


        [Route("/api/EmployeeDetailsAPI/ExportSalaryForEmployees/{id}")]
        [HttpGet("{id:int}")]
        public ActionResult<Employee> ExportSalaryForEmployees(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                // Get list of Employee Salary History from Database
                SalaryHistory salaryHistory = new SalaryHistory();
                List<object> listSalaryHistory =
                    (from SalaryHistory in _repository.SalaryHistory.Where(d => d.EmpID == id).ToList()
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

                // Generate CSV
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error getting salary details from the Application");
            }

        }
    }
}
