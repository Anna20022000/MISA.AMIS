using MISA.Fresher.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Interfaces.Service
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        public object GetPaging(int limit, int pageIndex, string searchText);
    }
}
