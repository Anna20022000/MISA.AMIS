using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.MISAAttribute
{
    /// <summary>
    /// Attribute cung cấp cho các properties bắt buộc nhập - sử dụng để đánh dấu phục vụ cho validate chung của các đối tượng
    /// CreatedBy: CTKYen (17/12/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEmpty : Attribute
    {

    }

    /// <summary>
    /// Attribute cung cấp cho các properties không được phép trùng - sử dụng để đánh dấu phục vụ cho validate
    /// CreatedBy: CTKYen (17/12/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Unique : Attribute
    {

    }

    /// <summary>
    /// Attribute để đặt tên cho property - sử dụng để đánh dấu phục vụ cho validate
    /// CreatedBy: CTKYen (17/12/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyName : Attribute
    {
        public string Name;
        public PropertyName(string name)
        {
            this.Name = name;
        }
    }

    /// <summary>
    /// Attribute để xác định độ dài tối đa cho property - sử dụng để đánh dấu phục vụ cho validate
    /// CreatedBy: CTKYen (17/12/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLength : Attribute
    {
        public int? Length;
        public MaxLength(int length)
        {
            this.Length = length;
        }
    }


    /// <summary>
    /// Attribute để xác định property chỉ hiển thị, không lưu vào csdl
    /// CreatedBy: CTKYen (19/12/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ReadOnly : Attribute
    {
    }


    /// <summary>
    /// Attribute để kiểm tra định dạng Email cho property - sử dụng để đánh dấu phục vụ cho validate
    /// CreatedBy: CTKYen (22/12/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Email : Attribute
    {

    }
    /// <summary>
    /// Attribute để kiểm tra định dạng Số điện thoại cho property - sử dụng để đánh dấu phục vụ cho validate
    /// CreatedBy: CTKYen (23/12/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneNumber : Attribute
    {

    }
    /// <summary>
    /// Attribute dùng để đánh dấu là cột thông tin trong file export
    /// CreateBy: CTKimYen (24/12/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnFileExport : Attribute
    {
        public string Name;
        public ColumnFileExport(string name)
        {
            this.Name = name;
        }
    }

}
