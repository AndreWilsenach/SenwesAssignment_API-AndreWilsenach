using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SenwesAssignment_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenwesAssignment_API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly LoadData _loadData;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
            _loadData = new LoadData();
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>Returns a list of all employees</returns>

        [HttpGet]
        public IActionResult Get()
        {
            var employeeData = _loadData.LoadEmployeeData();
            return Ok(employeeData);
        }


        [Route("GetById")]
        [HttpGet()]
        public IActionResult GetByEmployeeId([FromQuery] int empId)
        {
            try
            {
                var employee = _loadData.LoadEmployeeData().Where(x => x.EmpID == empId).FirstOrDefault();
                return Ok(employee);
            }
            catch (Exception err)
            {
                return BadRequest($"There was a probrem retriving client with ID : {empId}\r error : {err}");
            }
        }

        [Route("Get/EmployeesJoinLast5Years")]
        [HttpGet]
        public IActionResult GetLastFiveYears()
        {
            var startFilterDate = DateTime.Today.AddYears(-5);
            EmployeeListDTO returnObj = new EmployeeListDTO() { };
            try
            {
                var employees = _loadData.LoadEmployeeData().OrderByDescending(a => a.DateOfJoining).Where(a => Convert.ToDateTime(a.DateOfJoining) >= startFilterDate).Select(a => new Employee
                {
                    Age = a.Age,
                    City = a.City,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    EMail = a.EMail,
                    EmpID = a.EmpID,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastIncrease = a.LastIncrease,
                    LastName = a.LastName,
                    PhoneNo = a.PhoneNo,
                    Salary = a.Salary,
                    SSN = a.SSN,
                    State = a.State,
                    UserName = a.UserName,
                    YearsInCompany = a.YearsInCompany,
                    Zip = a.Zip

                }).ToList();

                if (employees.Count > 0)
                {
                    returnObj.EmployeeDTO = new List<Employee>() { };
                    employees.ForEach(a =>
                    {
                        
                        Employee validEmployee = new Employee();
                        validEmployee = a;
                        returnObj.EmployeeDTO.Add(validEmployee);
                    });
                }

                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving lastest employers of the last 5 years.\r the error : {ex}");
            }
        }

        [Route("Get/EmployeesOlderThan30")]
        [HttpGet]
        public IActionResult OlderThan30()
        {
            EmployeeListDTO returnObj = new EmployeeListDTO() { };
            try
            {
                var employees = _loadData.LoadEmployeeData().OrderBy(a => a.DateOfBirth).Where(a => GetEmployeeAge(a.DateOfBirth) >= 30).Select(a => new Employee
                {
                    Age = a.Age,
                    City = a.City,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    EMail = a.EMail,
                    EmpID = a.EmpID,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastIncrease = a.LastIncrease,
                    LastName = a.LastName,
                    PhoneNo = a.PhoneNo,
                    Salary = a.Salary,
                    SSN = a.SSN,
                    State = a.State,
                    UserName = a.UserName,
                    YearsInCompany = a.YearsInCompany,
                    Zip = a.Zip

                }).ToList();

                if (employees.Count > 0)
                {
                    returnObj.EmployeeDTO = new List<Employee>() { };
                    employees.ForEach(a =>
                    {

                        Employee validEmployee = new Employee();
                        validEmployee = a;
                        returnObj.EmployeeDTO.Add(validEmployee);
                    });
                }

                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving all employers over the age of 30.\r the error : {ex}");
            }
        }
        public static int GetEmployeeAge(string date)
        {
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            var dob = Convert.ToDateTime(date);
            var devideDate = int.Parse(dob.ToString("yyyyMMdd"));
            int age = (now - devideDate) / 10000;

            return age;
        }
        //remember to add route and params
        [Route("Get/GenderHighestEarners")]
        [HttpGet()]
        public IActionResult HighestPaidWorkers([FromQuery]string gender)
        {

            EmployeeListDTO returnObj = new EmployeeListDTO() { };
            IEnumerable<Employee> empMale;
            IEnumerable<Employee> empFemale;
            try
            {
                IEnumerable<Employee> emp = _loadData.LoadEmployeeData().OrderByDescending(a => a.Salary);
                if (gender != null)
                {
                    var forReturn = emp.Where(a => a.Gender.ToLower() == gender.ToLower()).Take(10).Select(a => new Employee
                    {
                        Age = a.Age,
                        City = a.City,
                        DateOfBirth = a.DateOfBirth,
                        DateOfJoining = a.DateOfJoining,
                        EMail = a.EMail,
                        EmpID = a.EmpID,
                        FirstName = a.FirstName,
                        Gender = a.Gender,
                        LastIncrease = a.LastIncrease,
                        LastName = a.LastName,
                        PhoneNo = a.PhoneNo,
                        Salary = a.Salary,
                        SSN = a.SSN,
                        State = a.State,
                        UserName = a.UserName,
                        YearsInCompany = a.YearsInCompany,
                        Zip = a.Zip

                    }).ToList();

                    if (forReturn.Count > 0)
                    {
                        returnObj.EmployeeDTO = new List<Employee>() { };
                        forReturn.ForEach(a =>
                        {

                            Employee validEmployee = new Employee();
                            validEmployee = a;
                            returnObj.EmployeeDTO.Add(validEmployee);
                        });
                    }

                    return Ok(returnObj);
                }
                empMale = emp.Where(a => a.Gender == "M").Select(a => new Employee
                {
                    Age = a.Age,
                    City = a.City,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    EMail = a.EMail,
                    EmpID = a.EmpID,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastIncrease = a.LastIncrease,
                    LastName = a.LastName,
                    PhoneNo = a.PhoneNo,
                    Salary = a.Salary,
                    SSN = a.SSN,
                    State = a.State,
                    UserName = a.UserName,
                    YearsInCompany = a.YearsInCompany,
                    Zip = a.Zip

                }).Take(10);
                empFemale = emp.Where(a => a.Gender == "F").Select(a => new Employee
                {
                    Age = a.Age,
                    City = a.City,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    EMail = a.EMail,
                    EmpID = a.EmpID,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastIncrease = a.LastIncrease,
                    LastName = a.LastName,
                    PhoneNo = a.PhoneNo,
                    Salary = a.Salary,
                    SSN = a.SSN,
                    State = a.State,
                    UserName = a.UserName,
                    YearsInCompany = a.YearsInCompany,
                    Zip = a.Zip

                }).Take(10);

                List<Employee> employees = CombineData(empFemale.ToList(), empMale.ToList());
                if (employees.Count > 0)
                {
                    returnObj.EmployeeDTO = new List<Employee>() { };
                    employees.ForEach(a =>
                    {

                        Employee validEmployee = new Employee();
                        validEmployee = a;
                        returnObj.EmployeeDTO.Add(validEmployee);
                    });
                }

                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving Highest Paying Genders.\r the error : {ex}");
            }
        }

        public static List<Employee> CombineData(List<Employee> female, List<Employee> male)
        {
            List<Employee> returnData = new List<Employee>();
            female.ForEach(a =>
            {
                returnData.Add(a);
            });
            male.ForEach(a =>
            {
                returnData.Add(a);
            });

            return returnData;
        }


        [Route("Get/SearchTenant/{city}")]
        [HttpGet()]
        public IActionResult SearhByNameAndCity([FromQuery] string name, [FromQuery] string surname, string city)
        {

            if (name == null && surname == null) 
            {
                return BadRequest("No name or surname Given ! please enter either one...");
            }
            EmployeeListDTO returnObj = new EmployeeListDTO() { };
            try
            {
                var employees = _loadData.LoadEmployeeData().Where(a => (
                ((name != null && a.FirstName.ToLower() == name.ToLower()) || (surname != null && a.LastName.ToLower() == surname.ToLower())) && a.City.ToLower() == city.ToLower())
                ).Select(a => new Employee
                {
                    Age = a.Age,
                    City = a.City,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    EMail = a.EMail,
                    EmpID = a.EmpID,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastIncrease = a.LastIncrease,
                    LastName = a.LastName,
                    PhoneNo = a.PhoneNo,
                    Salary = a.Salary,
                    SSN = a.SSN,
                    State = a.State,
                    UserName = a.UserName,
                    YearsInCompany = a.YearsInCompany,
                    Zip = a.Zip

                }).ToList();

                if (employees.Count > 0)
                {
                    returnObj.EmployeeDTO = new List<Employee>() { };
                    employees.ForEach(a =>
                    {

                        Employee validEmployee = new Employee();
                        validEmployee = a;
                        returnObj.EmployeeDTO.Add(validEmployee);
                    });
                }

                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving Data for search by name or surname and city.\r the error : {ex}");
            }
        }


        [Route("Get/ByNameTresure")]
        [HttpGet]
        public IActionResult TresureSalary()
        {
            List<TresureSalaryDTO> returnObj = new List<TresureSalaryDTO>() { };
            try
            {
                var employees = _loadData.LoadEmployeeData().Where(a => a.FirstName.ToLower() == "treasure");
                
                foreach (Employee T in employees) 
                {
                    TresureSalaryDTO em = new TresureSalaryDTO 
                    {
                        Firstame = T.FirstName,
                        LastName = T.LastName,
                        Salary = T.Salary
                    };
                    returnObj.Add(em);
                }
                if (returnObj.Count()>0) 
                {
                    return Ok(returnObj);
                }
                return Ok("No employee with first name treasure found !");
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving The salary of all Thresures.\r the error : {ex}");
            }
        }

        [AllowAnonymous]
        [Route("Get/CityList")]
        [HttpGet]
        public IActionResult UnAuthCitiesList()
        {
            var startFilterDate = DateTime.Today.AddYears(-5);
            List<string> returnObj = new List<string>() { };
            try
            {
                var employees = _loadData.LoadEmployeeData().OrderBy(a => a.DateOfJoining).ToList();

                foreach (Employee i in employees)
                {
                    returnObj.Add(i.City);
                }
                return Ok(returnObj.Distinct());
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving The list of cities.\r the error : {ex}");
            }
        }
    }
}
