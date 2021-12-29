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
        /// <summary>
        /// Thực hiện lọc dữ liệu và phân trang
        /// </summary>
        /// <param name="limit">Số bản ghi/ trang</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="searchText">Điều kiện lọc (Mã nhân viên, tên nhân viên, số điện thoại)</param>
        /// <returns>Danh sách nhân viên thỏa mãn điều kiện lọc</returns>
        /// CreatedBy: CTKimYen (23/12/2021)
        public object GetPaging(int limit, int pageIndex, string searchText);
    }
}
