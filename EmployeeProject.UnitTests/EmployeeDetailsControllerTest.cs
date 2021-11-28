using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Results;
using System.Net;
using EmployeeSalaryProject.Controllers;
using EmployeeSalaryProject.Models;
using EmployeeSalaryProject.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace EmployeeProject.UnitTests
{
    public interface IDBContext
    {
        Employee Update(int empId);
        Employee Details(int empId);
    }

    [TestFixture]
    public class EmployeeDetailsControllerTest
    {
        private IMapper _mapper;
        private ApplicationDBContext _db;

        //[SetUp]
        //public void Setup()
        //{
        //    var dbContextOptions = new DbContextOptionsBuilder<ExampleContext>().UseInMemoryDatabase("Test");
        //    _context = new ExampleContext(dbContextOptions.Options);
        //    _context.Database.EnsureCreated();

        //    _createCustomerService = new CreateCustomerService(_context);
        //}


        [Test]
        public void Create_WhenCalled_ReturnsOk()
        {
            // arrange
            EmployeeDetailsController controller = new EmployeeDetailsController(_db);

            // act
            ViewResult result = controller.Create() as ViewResult;

            // assert
            Assert.That(result, Is.Not.Null);

        }

        [Test]
        [TestCase(27)]
        [TestCase(0)]
        public void Details_WhenCalled_ReturnsOk(IDBContext dbContext, int id)
        {
            // arrange
            EmployeeDetailsController controller = new EmployeeDetailsController(_db);

            // act
            var result = controller.Details(id);

            // assert
            Assert.That(result, Is.Not.Null);

        }

        [Test]
        [TestCase(27)]
        [TestCase(0)]
        public void Update_WhenCalled_ReturnsOk(int id)
        {
            // arrange
            EmployeeDetailsController controller = new EmployeeDetailsController(_db);

            // act
            var result = controller.Update(id) as ViewResult;

            // assert
            if (id != 0)
                Assert.That(result, Is.Not.Null);
            else
                Assert.That(result, Is.Null);

        }
    }
}
