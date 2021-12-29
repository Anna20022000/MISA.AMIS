using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Enum;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Service;
using MISA.Fresher.Core.MISAAttribute;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Fresher.Api.Controllers
{
    public class EmployeesController : MISABaseController<Employee>
    {
        #region Declare and contructor
        IEmployeeService _employeeService;
        IEmployeeRepository _employeeRepository;
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService)
            : base(employeeRepository, employeeService)
        {
            _employeeService = employeeService;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Function
        /// <summary>
        /// Phân trang và tìm kiếm
        /// </summary>
        /// <param name="limit">số bản ghi trên trang</param>
        /// <param name="pageIndex">số trang hiện tại</param>
        /// <param name="searchText">điều kiện lọc</param>
        /// <returns>Dữ liệu thỏa mãn điều kiện lọc; Tổng số bản ghi; Tổng số trang</returns>
        /// CreatedBy: CTKimYen (23/12/2021)
        [HttpGet("filter")]
        public IActionResult GetPaging(int limit, int pageIndex, string searchText)
        {
            return Ok(_employeeService.GetPaging(limit, pageIndex, searchText));
        }

        /// <summary>
        /// Lấy ra mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// CreatedBy: CTKimYen (23/12/2021)
        [HttpGet("newEmployeeCode")]
        public IActionResult GetNewCode()
        {
            return Ok(_employeeRepository.GetNewCode());
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="ListId">Danh sách một chuỗi các khóa chính</param>
        /// <returns>Số bản ghi xóa thành công</returns>
        /// CreatedBy: CTKimYen (23/12/2021)
        [HttpDelete("deleteMulti")]
        public IActionResult DeleteMulti([FromBody] List<string> ListId)
        {
            string ids = null;
            foreach (var item in ListId)
            {
                ids += $"{item},";
            }
            ids = ids.Substring(0, ids.Length - 1);
            var res = _employeeRepository.DeleteMultiRecord(ids);
            if (res > 0)
            {
                return StatusCode(200, res);
            }
            return null;
        }

        /// <summary>
        /// Thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <returns>File excel</returns>
        /// CreatedBy: CTKimYen (23/12/2021)
        [HttpGet("export")]
        public IActionResult Export()
        {
            // lấy về stream để tạo file excel
            Stream stream = _employeeRepository.ExportToExcel();
            stream.Position = 0;
            // set filename cho file excel
            string excelName = $"Danh_sach_nhan_vien_{DateTime.Now.ToString("dd-MM-yyy HH-mm-ss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        #endregion

    }
}
