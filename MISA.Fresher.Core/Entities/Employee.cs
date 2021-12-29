using MISA.Fresher.Core.Enum;
using MISA.Fresher.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Entities
{
    /// <summary>
    /// Thông tin nhân viên
    /// CreatedBy: CTKimYen (15/12/2021)
    /// </summary>
    public class Employee
    {

        #region Properties
        /// <summary>
        /// khóa chính
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [NotEmpty]
        [Unique]
        [PropertyName("Mã nhân viên")]
        [ColumnFileExport("Mã nhân viên")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        [NotEmpty]
        [PropertyName("Họ và tên")]
        [ColumnFileExport("Tên nhân viên")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// giới tính (0-nữ, 1-nam, 2-khác)
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Lấy ra tên giới tính (Nam/ Nữ/ Khác)
        /// </summary>
        [ReadOnly]
        [PropertyName("Giới tính")]
        [ColumnFileExport("Giới tính")]
        public string GenderName
        {
            get
            {
                var resourceText = MISAEnum.GetEnumTextByEnumName<Gender>(Gender);
                return resourceText;
            }
        }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [PropertyName("Ngày sinh")]
        [ColumnFileExport("Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        [PhoneNumber]
        [PropertyName("Số điện thoại di động")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        [PhoneNumber]
        [PropertyName("Số điện thoại cố định")]
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Email]
        public string Email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// khóa ngoại - mã phòng ban
        /// </summary>
        public Guid? DepartmentId { get; set; }


        /// <summary>
        /// Tên vị trí/ chức vụ
        /// </summary>
        [PropertyName("Chức danh")]
        [ColumnFileExport("Chức danh")]
        public string PositionName { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [ReadOnly]
        [PropertyName("Đơn vị")]
        [ColumnFileExport("Tên đơn vị")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Số CMND/ Căn cước
        /// </summary>
        [PropertyName("Số CMND")]
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Nơi cấp CMND
        /// </summary>
        public string IdentityPlace { get; set; }

        /// <summary>
        /// ngày nhận thẻ CMND/căn cước
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// tài khoản ngân hàng
        /// </summary>
        [PropertyName("Số tài khoản ngân hàng")]
        [ColumnFileExport("Số tài khoản")]
        public string BankAccount { get; set; }

        /// <summary>
        /// tên ngân hàng
        /// </summary>
        [PropertyName("Tên ngân hàng")]
        [ColumnFileExport("Tên ngân hàng")]
        public string BankName { get; set; }

        /// <summary>
        /// chi nhánh ngân hàng
        /// </summary>
        [PropertyName("Chi nhánh ngân hàng")]
        public string BankBranch { get; set; }

        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// ngày sửa gần nhất
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// người sửa
        /// </summary>
        public string ModifiedBy { get; set; }
        #endregion
    }
}
