using MISA.Fresher.Core.Exceptions;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Service;
using MISA.Fresher.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Services
{
    public class BaseService<T> : IBaseService<T>
    {
        IBaseRepository<T> _baseRepository;
        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
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


                var propNotEmpties = prop.GetCustomAttributes(typeof(NotEmpty), true);
                // Lấy ra tên hiển thị nếu được đặt attr PropertyName
                var propPropertyNames = prop.GetCustomAttributes(typeof(PropertyName), true);
                var propMaxLengths = prop.GetCustomAttributes(typeof(MaxLength), true);
                var propUniques = prop.GetCustomAttributes(typeof(Unique), true);

                // Nếu được đặt attr PropertyName
                if (propPropertyNames.Length > 0)
                {
                    propNameDisplay = (propPropertyNames[0] as PropertyName).Name;
                }
                // Nếu được đặt attribute NotEmpty:
                if (propNotEmpties.Length > 0)
                {
                    // Nếu không hợp lệ thì hiển thị cảnh báo hoặc đánh dấu trạng thái không hợp lệ:
                    if (propValue == null || string.IsNullOrEmpty(propValue.ToString().Trim()))
                    {
                        errMsg.Add($"Thông tin {propNameDisplay} không được phép để trống.");
                    }
                }
                // Nếu được đặt attr Unique - duy nhất:
                if (propUniques.Length > 0 && !(propValue == null || string.IsNullOrEmpty(propValue.ToString().Trim())))
                {
                    if (_baseRepository.CheckExist(propNameOriginal, propValue.ToString().Trim()))
                    {
                        errMsg.Add($"Thông tin {propNameDisplay} đã tồn tại.");
                    }
                }
                // Nếu được đặt attr MaxLength
                if (propMaxLengths.Length > 0)
                {
                    var length = (propMaxLengths[0] as MaxLength).Length;
                    if (propValue != null && propValue.ToString().Length > length)
                    {
                        errMsg.Add($"Thông tin {propNameDisplay} không được dài quá {length} ký tự.");
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


                // Lấy ra tên hiển thị nếu được đặt attr PropertyName
                var propPropertyNames = prop.GetCustomAttributes(typeof(PropertyName), true);
                var propUniques = prop.GetCustomAttributes(typeof(Unique), true);

                // Nếu được đặt attr PropertyName
                if (propPropertyNames.Length > 0)
                {
                    propNameDisplay = (propPropertyNames[0] as PropertyName).Name;
                }
                // Nếu được đặt attr Unique - duy nhất:
                if (propUniques.Length > 0 && !(propValue == null || string.IsNullOrEmpty(propValue.ToString().Trim())))
                {
                    if (_baseRepository.CheckExist(propNameOriginal, propValue.ToString().Trim()))
                    {
                        errMsg.Add($"Thông tin {propNameDisplay} đã tồn tại.");
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
            //var isValid = ValidateObject(entity);

            //if (isValid)
            //{
            //    // Validate đặc thù cho từng đối tượng -> cho các services con tự xử lý
            //    if (ValidateObjCustom(entity))
            //    {
                    var res = _baseRepository.Update(entity, entityId);
                    return res;
            //    }
            //}

            //return 0;
        }
    }
}
