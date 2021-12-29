using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Exceptions;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Service;
using MISA.Fresher.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region Declare and Contructor
        IEmployeeRepository _employeeRepository;
        public EmployeeService(IBaseRepository<Employee> baseRepository, IEmployeeRepository employeeRepository) : base(baseRepository)
        {
            _employeeRepository = employeeRepository;
        }
        #endregion
        public object GetPaging(int limit, int pageIndex, string searchText)
        {
            return _employeeRepository.GetPaging(limit, pageIndex, searchText);
        }

    }
}
