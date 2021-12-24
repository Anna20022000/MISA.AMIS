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
        /// Thực hiện lọc dữ liệu và phân trang
        /// </summary>
        /// <param name="limit">Số bản ghi/ trang</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="searchText">Điều kiện lọc (Mã nhân viên, tên nhân viên, số điện thoại)</param>
        /// <returns>Danh sách nhân viên thỏa mãn điều kiện lọc</returns>
        object GetPaging(int limit, int pageIndex, string searchText);

        /// <summary>
        /// Lấy ra mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        public string GetNewCode();

        /// <summary>
        /// Thực hiện xóa nhiều bản ghi
        /// </summary>
        /// <param name="ListEntityIds">Chuỗi các khóa chính của các đối tượng</param>
        /// <returns>Số lượng bản ghi xóa thành công</returns>
        public int DeleteMultiRecord(string ListEntityIds);


    }
}
