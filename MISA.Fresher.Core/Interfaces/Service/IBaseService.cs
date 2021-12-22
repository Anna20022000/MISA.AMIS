using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Interfaces.Service
{
    public interface IBaseService<T>
    {
        /// <summary>
        /// Thực hiện thêm mới đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>Số lượng bản ghi thêm mới thành công</returns>
        /// createdBy: CTKimYen (15/12/2021)
        int? Insert(T entity);
        /// <summary>
        /// Thực hiện Sửa một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <param name="entityId">Khóa chính</param>
        /// <returns>Số lượng bản ghi sửa thành công</returns>
        /// createdBy: CTKimYen (15/12/2021)
        int Update(T entity, Guid entityId);
    }
}
