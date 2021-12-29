using MISA.Fresher.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// CreatedBy: CTKimYen (23/12/2021)
        object GetPaging(int limit, int pageIndex, string searchText);

        /// <summary>
        /// Lấy ra mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// CreatedBy: CTKimYen (22/12/2021)
        public string GetNewCode();

        /// <summary>
        /// Thực hiện xóa nhiều bản ghi
        /// </summary>
        /// <param name="ListEntityIds">Chuỗi các khóa chính của các đối tượng</param>
        /// <returns>Số lượng bản ghi xóa thành công</returns>
        /// CreatedBy: CTKimYen (25/12/2021)
        public int DeleteMultiRecord(string ListEntityIds);

        /// <summary>
        /// Thực hiện tạo sheet excel
        /// </summary>
        /// <param name="stream">luồng dữ liệu thao tác với file - khởi tạo là null</param>
        /// <returns>Trả về stream thao tác với file</returns>
        /// CreatedBy: CTKimYen (27/12/2021)
        public Stream ExportToExcel(Stream stream = null);
    }
}
