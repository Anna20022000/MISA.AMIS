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
        IEmployeeService _employeeService;
        IEmployeeRepository _employeeRepository;
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService)
            : base(employeeRepository, employeeService)
        {
            _employeeService = employeeService;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("filter")]
        public IActionResult GetPaging(int limit, int pageIndex, string searchText)
        {
            return Ok(_employeeService.GetPaging(limit, pageIndex, searchText));
        }

        [HttpGet("NewEmployeeCode")]
        public IActionResult GetNewCode()
        {
            return Ok(_employeeRepository.GetNewCode());
        }


        [HttpDelete("deleteMulti")]
        public IActionResult DeleteMulti([FromBody] string ListId)
        {
            var res = _employeeRepository.DeleteMultiRecord(ListId);
            if (res > 0)
            {
                return StatusCode(200, res);
            }
            return null;
        }

        [HttpGet("export")]
        public IActionResult Export()
        {
            // query data from database
            var list = _employeeRepository.GetAll().ToList();

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("MISA_nhan_vien");
                workSheet.Row(1).Style.Font.Bold = true; 
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                workSheet.DefaultRowHeight = 20;
                workSheet.Cells[1,1].Value = "STT";
                // customize tên header của file excel
                var employee = new Employee();
                // lấy các thuộc tính của nhân viên
                var properties = employee.GetType().GetProperties();

                int indexHeader = 2;
                
                // HEADER
                for (int i = 1; i < properties.Length; i++)
                {
                    // lấy tên hiển thị đầu tiên của thuộc tính
                    var PropertyNameAttributes = properties[i].GetCustomAttributes(typeof(PropertyName), true);
                    // loại bỏ các thuộc tính không có 'tên hiển thị', có kiểu Guid, Gender
                    if (PropertyNameAttributes.Length > 0 && properties[i].GetType() != typeof(Guid) && properties[i].GetType() != typeof(Gender))
                    {
                        // add vào header của file excel
                        workSheet.Cells[1, indexHeader].Value = (PropertyNameAttributes[0] as PropertyName).Name;
                        indexHeader++;
                    }
                }
                // BODY
                // lấy dữ liệu thêm vào excel
                // duyệt các nhân viên
                for (int i = 0; i < list.Count(); i++)
                {
                    int indexColBody = 2; // chỉ số cột
                    // lấy ra STT
                    workSheet.Cells[ i+2, 1].Value = i+1;

                    // duyệt các thuộc tính để tương tự với phần header
                    for (int j = 1; j < properties.Length; j++)
                    {
                        var propertyNameAttr = properties[j].GetCustomAttributes(typeof(PropertyName), true);

                        // nếu thuộc tính có xuất file: cột +1
                        if (propertyNameAttr.Length > 0 && properties[j].GetType() != typeof(Guid) && properties[j].GetType() != typeof(Gender))
                        {
                            // xử lí các kiểu dữ liệu datetime
                            if ((propertyNameAttr[0] as PropertyName).Name == "Ngày sinh" || (propertyNameAttr[0] as PropertyName).Name == "Ngày cấp")
                            {
                                if (properties[j].GetValue(list[i]) != null && !string.IsNullOrEmpty(properties[j].GetValue(list[i]).ToString().Trim()))
                                {
                                    DateTime dt = DateTime.ParseExact(properties[j].GetValue(list[i]).ToString(), "d/M/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                    workSheet.Cells[i+2, indexColBody].Value = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    //workSheet.Cells[i + 2, indexColBody].Style.Border.Top.Style = workSheet.Cells[i + 2, indexColBody].Style.Border.Left.Style = workSheet.Cells[i + 2, indexColBody].Style.Border.Right.Style = workSheet.Cells[i + 2, indexColBody].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                }
                            }
                            else // các kiểu dữ liệu khác datetime
                            {
                                workSheet.Cells[i+2, indexColBody].Value = properties[j].GetValue(list[i]);
                                //workSheet.Cells[i + 2, indexColBody].Style.Border.Top.Style = workSheet.Cells[i + 2, indexColBody].Style.Border.Left.Style = workSheet.Cells[i + 2, indexColBody].Style.Border.Right.Style = workSheet.Cells[i + 2, indexColBody].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            }
                            indexColBody++;
                        }
                        // độ rộng tự động fit với dữ liệu
                        workSheet.Column(j).AutoFit();
                    }
                }

                // lưu lại
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"MISA-Employees-{DateTime.Now.ToString("dd-MM-yyy HH-mm-ss")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }


    }
}
