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
            var languageCode = Common.Common.LanguageCode;

            var resourceText = MISA.Fresher.Core.Properties.Resources.ResourceManager.GetString($"{languageCode}_Enum_{enumName}_{enumPropertyName}");
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

}
