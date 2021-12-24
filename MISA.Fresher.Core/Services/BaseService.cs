using MISA.Fresher.Core.Exceptions;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Service;
using MISA.Fresher.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Services
{
    public class BaseService<T> : IBaseService<T>
    {
        IBaseRepository<T> _baseRepository;
        protected string _className;
        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
            _className = typeof(T).Name;
        }
        public int? Insert(T entity)
        {
            // validate chung - cho base xử lý
            var isValid = ValidateObject(entity);

            if (isValid)
            {
                // Validate đặc thù cho từng đối tượng -> cho các services con tự xử lý
                if (ValidateObjCustom(entity))
                {
                    var res = _baseRepository.Insert(entity);
                    return res;
                }
            }

            return null;
        }

        /// <summary>
        /// Thực hiện validate dữ liệu chung (VD: trống mã, trống thông tin, sai định dạng email...)
        /// </summary>
        /// <param name="entity">Đối tượng cần Validate</param>
        /// <returns>true - dữ liệu hợp lệ; false - dữ liệu không hợp lệ</returns>
        /// createdBy: CTKimYen (17/12/2021)
        bool ValidateObject(T entity)
        {
            List<string> errMsg = new List<string>();
            // Kiểm tra các thông tin bắt buộc nhập
            // 1. Kiểm tra tất cả các props của đối tượng
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                // Lấy ra tên gốc của Property đang duyệt
                var propNameOriginal = prop.Name;
                // Set tên hiển thị cho Prop đang duyệt
                var propNameDisplay = propNameOriginal;
                // Lấy ra giá trị của property đang duyệt
                var propValue = prop.GetValue(entity);


                //var propNotEmpties = prop.GetCustomAttributes(typeof(NotEmpty), true);
                // Lấy ra tên hiển thị nếu được đặt attr PropertyName
                var propPropertyNames = prop.GetCustomAttributes(typeof(PropertyName), true);
                var propMaxLengths = prop.GetCustomAttributes(typeof(MaxLength), true);

                // Nếu được đặt attr PropertyName
                if (prop.IsDefined(typeof(PropertyName), false))
                {
                    propNameDisplay = (propPropertyNames[0] as PropertyName).Name;
                }
                // Kiểm tra trường bắt buộc nhập
                // Nếu được đặt attribute NotEmpty:
                if (prop.IsDefined(typeof(NotEmpty), false))
                {
                    // Nếu không hợp lệ thì hiển thị cảnh báo hoặc đánh dấu trạng thái không hợp lệ:
                    if (propValue == null || string.IsNullOrEmpty(propValue.ToString().Trim()))
                    {
                        errMsg.Add(string.Format(Properties.Resources.Error_Msg_Empty, propNameDisplay));
                    }
                }
                // Kiểm tra Email
                if (propValue != null && prop.IsDefined(typeof(Email), false))
                {
                    Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                                                RegexOptions.CultureInvariant | RegexOptions.Singleline);
                    bool isValidEmail = regex.IsMatch(propValue.ToString().Trim());
                    if (!isValidEmail)
                    {
                        errMsg.Add(string.Format(Properties.Resources.Error_Msg_Invalid, propNameDisplay));
                    }
                }

                // Kiểm tra số điện thoại
                if (propValue != null && prop.IsDefined(typeof(PhoneNumber), false) && propValue != null)
                {
                    Regex regex = new Regex(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$",
                                                RegexOptions.CultureInvariant | RegexOptions.Singleline);

                    bool isValidPhoneNumber = regex.IsMatch(propValue.ToString().Trim());
                    if (!isValidPhoneNumber)
                    {
                        errMsg.Add(string.Format(Properties.Resources.Error_Msg_Invalid, propNameDisplay));
                    }
                }

                // Kiểm tra độ dài
                // Nếu được đặt attr MaxLength
                if (propMaxLengths.Length > 0)
                {
                    var length = (propMaxLengths[0] as MaxLength).Length;
                    if (propValue != null && propValue.ToString().Length > length)
                    {
                        errMsg.Add(string.Format(Properties.Resources.Error_Msg_Length, propNameDisplay, length));
                    }
                }

            }
            // Nếu có lỗi ném ra một ngoại lệ MISAResponseNotValidException
            if (errMsg.Count() > 0)
            {
                throw new MISAResponseNotValidException(errMsg);
            }
            return true;
        }

        /// <summary>
        /// Thực hiện Validate dữ liệu đặc thù cho từng đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>true - dữ liệu hợp lệ; false - dữ liệu không hợp lệ</returns>
        /// createdBy: CTKimYen (17/12/2021)
        protected virtual bool ValidateObjCustom(T entity)
        {
            // lấy ra tên thuộc tính Khóa chính
            var entityIdName = typeof(T).GetProperty($"{_className}Id");
            // Lấy ra giá trị Khóa chính của đối tượng Employee
            Guid propValueEnityId = (Guid)entityIdName.GetValue(entity);
            // Lấy ra đối tượng được cập nhật
            T entityCurrent = _baseRepository.GetById(propValueEnityId);

            // Danh sách các lỗi
            List<string> errMsg = new List<string>();

            // Kiểm tra tất cả các props của đối tượng
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                // Lấy ra tên gốc của Property đang duyệt
                var propNameOriginal = prop.Name;
                // Set tên hiển thị cho Prop đang duyệt
                var propNameDisplay = propNameOriginal;
                // Lấy ra giá trị của property đang duyệt
                var propValue = prop.GetValue(entity);

                // Lấy ra tên hiển thị nếu được đặt attr PropertyName
                var propPropertyNames = prop.GetCustomAttributes(typeof(PropertyName), true);

                var propUniques = prop.GetCustomAttributes(typeof(Unique), true);

                // Nếu được đặt attr PropertyName
                if (propPropertyNames.Length > 0)
                {
                    propNameDisplay = (propPropertyNames[0] as PropertyName).Name;
                }
                // Nếu được đặt attr Unique và không trống
                if (propUniques.Length > 0 && !(propValue == null || string.IsNullOrEmpty(propValue.ToString().Trim())))
                {

                    // SỬA - check dữ liệu đã bị thay đối
                    if (entityCurrent != null)
                    {
                        // lấy ra giá trị prop hiện tại của đối tượng cần sửa
                        var propValueEnityCurrent = prop.GetValue(entityCurrent);
                        int a = string.Compare(propValue.ToString().Trim(), propValueEnityCurrent.ToString().Trim(), false);
                        if (string.Compare(propValue.ToString().Trim(),propValueEnityCurrent.ToString().Trim(), false) != 0)
                        {
                            if (_baseRepository.CheckExist(propNameOriginal, propValue.ToString().Trim(), propValueEnityId.ToString()))
                            {
                                errMsg.Add(string.Format(Properties.Resources.Error_Msg_Duplicate,propNameDisplay, propValue));
                            }
                        }
                    }
                    // THÊM MỚI
                    else
                    {
                        if (_baseRepository.CheckExist(propNameOriginal, propValue.ToString().Trim(), string.Empty))
                        {
                            errMsg.Add(string.Format(Properties.Resources.Error_Msg_Duplicate, propNameDisplay, propValue));
                        }
                    }

                }
            }
            // Nếu có lỗi ném ra một ngoại lệ MISAResponseNotValidException
            if (errMsg.Count() > 0)
            {
                throw new MISAResponseNotValidException(errMsg);
            }
            return true;
        }

        public int Update(T entity, Guid entityId)
        {
            // validate chung - cho base xử lý
            var isValid = ValidateObject(entity);

            if (isValid)
            {
                isValid = ValidateObjCustom(entity);
                // Validate đặc thù cho từng đối tượng -> cho các services con tự xử lý
                if (isValid)
                {
                    var res = _baseRepository.Update(entity, entityId);
                    return res;
                }
            }

            return 0;
        }
    }
}
