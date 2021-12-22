using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Fresher.Api.Controllers
{
    public class EmployeesController : MISABaseController<Employee>
    {
        IEmployeeService _employeeService;
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService)
            : base(employeeRepository, employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("paging")]
        public IActionResult GetPaging(int limit, int pageIndex)
        {
            return Ok(_employeeService.GetPaging(limit, pageIndex));
        }
    }
}
