using MISA.Fresher.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Interfaces.Infrastructure
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// Thực hiện lấy dữ liệu phân trang của bảng nhân viên
        /// </summary>
        /// <param name="limit">Số bản ghi/ trang</param>
        /// <param name="pageIndex">Chỉ số trang hiện tại</param>
        /// <returns></returns>
        object GetPaging(int limit, int pageIndex);
    }
}
