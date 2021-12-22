using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Services
{
    public class EmployeeService : BaseService<Employee> , IEmployeeService
    {
        IEmployeeRepository _employeeRepository;
        public EmployeeService(IBaseRepository<Employee> baseRepository, IEmployeeRepository employeeRepository) : base(baseRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public object GetPaging(int limit, int pageIndex)
        {
            return _employeeRepository.GetPaging(limit, pageIndex);
        }

        protected override bool ValidateObjCustom(Employee entity)
        {
            // Tên nhân viên không được phép để trống
            if (string.IsNullOrEmpty(entity.EmployeeCode))
            {
                throw new Exception("Mã nhân viên không được để trống.");
            };
            return true;
        }
    }
}
