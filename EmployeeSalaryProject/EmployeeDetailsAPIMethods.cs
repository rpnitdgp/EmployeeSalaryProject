using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSalaryProject.Models;
using EmployeeSalaryProject.Data;

namespace EmployeeSalaryProject
{
    public class EmployeeDetailsAPIMethods
    {
        private readonly ApplicationDBContext _repository;

        public EmployeeDetailsAPIMethods(ApplicationDBContext repository)
        {
            _repository = repository;
        }
        public Employee createEmployeeDetails(Employee newEmployee)
        {
            if (newEmployee.DateOfBirth != DateTime.MinValue)
            {
                int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                int dob = int.Parse(newEmployee.DateOfBirth.ToString("yyyyMMdd"));
                int ageOfEmp = (now - dob) / 10000;
                newEmployee.Age = ageOfEmp;
            }

            _repository.Employees.Add(newEmployee);
            _repository.SaveChanges();

            return newEmployee;
        }

        public Employee deleteEmployeeDetails(int empId)
        {
            Employee empObj = _repository.Employees.Where(d => d.EmpID == empId)
                .FirstOrDefault();

            EmployeeSalary empSalObj = _repository.EmployeeSalary.Where(d => d.EmpID == empId)
                .FirstOrDefault();

            var salHistObj = _repository.SalaryHistory.Where(d => d.EmpID == empId)
                .ToList();

            if (empObj != null)
            {
                if(salHistObj != null)
                {
                    _repository.SalaryHistory.RemoveRange(salHistObj);
                    _repository.SaveChanges();
                }
                if (empSalObj != null)
                {
                    _repository.EmployeeSalary.Remove(empSalObj);
                    _repository.SaveChanges();
                }
                _repository.Employees.Remove(empObj);
                _repository.SaveChanges();
            }
            return empObj;
        }

        public Employee updateEmployeeDetails(Employee newEmployee)
        {
            Employee empObj = _repository.Employees.Where(d => d.EmpID == newEmployee.EmpID)
                .FirstOrDefault();

            EmployeeSalary empSalObj = _repository.EmployeeSalary.Where(d => d.EmpID == newEmployee.EmpID)
                .FirstOrDefault();

            if (empObj != null || empSalObj != null)
            {

                empSalObj = CalculateSalaryComp(empSalObj, empObj);
                _repository.EmployeeSalary.Update(empSalObj);

                empObj.GrossSalary = newEmployee.GrossSalary;
                empObj.WorkingHours = newEmployee.WorkingHours;
                _repository.Employees.Update(empObj);
                _repository.SaveChanges();
            }
            return empObj;
        }

        public EmployeeSalary createEmployeeSalary(Employee emp, EmployeeSalary empSalary)
        {
            //Calculate Salary Components
            EmployeeSalary empSalaryObj = CalculateSalaryComp(empSalary, emp);
            
            _repository.EmployeeSalary.Add(empSalaryObj);
            _repository.SaveChanges();

            return empSalaryObj;
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
            if (employeeSalaryObj.EmpID == 0)
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
