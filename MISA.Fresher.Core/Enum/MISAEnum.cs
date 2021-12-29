using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Enum
{
    public static class MISAEnum
    {
        /// <summary>
        /// Thực thi lấy ra Resources tương ứng với Enum
        /// </summary>
        /// <typeparam name="T">Đối tượng Enum</typeparam>
        /// <param name="misaEnum">Đối tượng Enum</param>
        /// <returns>Resources tương ứng</returns>
        public static string GetEnumTextByEnumName<T>(T misaEnum)
        {
            var enumPropertyName = misaEnum.ToString();
            var enumName = misaEnum.GetType().Name;
            //var languageCode = Common.Common.LanguageCode;

            var resourceText = Properties.Resources.ResourceManager.GetString($"VI_Enum_{enumName}_{enumPropertyName}");
            return resourceText;
        }
    }

    /// <summary>
    /// Giới tính
    /// </summary>
    /// CreatedBy: CTKimYen (20/12/2021)
    public enum Gender
    {
        /// <summary>
        /// Nữ
        /// </summary>
        Female = 0,
        /// <summary>
        /// Nam
        /// </summary>
        Male = 1,
        /// <summary>
        /// Khác
        /// </summary>
        Other = 2
    }

    /// <summary>
    /// mã trạng thái HTTP res
    /// </summary>
    /// createdBy: CTKimYen (27/12/2021)
    public enum HttpStatusCode
    {
        /// <summary>
        /// Thành công
        /// </summary>
        Ok = 200,
        /// <summary>
        /// Thêm mới thành công
        /// </summary>
        Created = 201,
        /// <summary>
        /// Yêu cầu được chấp nhận nhưng việc xử lý chưa hoàn thành
        /// </summary>
        Accepted = 202,
        /// <summary>
        /// Yêu cầu đã được xử lý thành công nhưng phản hổi trống
        /// </summary>
        NoContent = 204,
        /// <summary>
        /// Máy chủ không hiểu được yêu cầu - lỗi dữ liệu gửi về
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// Request resource không tồn tại trên máy chủ
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// Lỗi xử lý phía máy chủ
        /// </summary>
        InternalServerError = 500
    }

}
