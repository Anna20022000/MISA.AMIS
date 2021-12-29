using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.MISAAttribute;
using MISA.Fresher.Core.Properties;
using MySqlConnector;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region Contructor
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }
        #endregion

        #region Function
        public override IEnumerable<Employee> GetAll()
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                // Thực thi lấy dữ liệu trong db:
                var entities = sqlConnection.Query<Employee>(sql: $"SELECT * FROM View_{_className}");
                return entities;
            }
        }

        public object GetPaging(int limit, int pageIndex, string searchText)
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                var sql = "Proc_GetEmployeePaging";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("m_SearchText", searchText);
                parameters.Add("m_PageSize", limit);
                parameters.Add("m_PageIndex", pageIndex);
                parameters.Add("m_TotalRecord", direction: ParameterDirection.Output);
                parameters.Add("m_TotalPage", direction: ParameterDirection.Output);
                // Thực thi lấy dữ liệu trong db:
                var entities = sqlConnection.Query<Employee>(sql, param: parameters, commandType: CommandType.StoredProcedure);
                var totalRecord = parameters.Get<int>("m_TotalRecord");
                var totalPage = parameters.Get<int>("m_TotalPage");

                return new {
                    TotalRecord = totalRecord,
                    TotalPage = totalPage,
                    Data = entities
                };
            }
        }

        public string GetNewCode()
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                var sql = $"Proc_GetNewEmployeeCode";
                // Thực thi lấy dữ liệu trong db:
                var newCode = sqlConnection.Query<string>(sql, commandType: CommandType.StoredProcedure);
                return newCode.FirstOrDefault();
            }
        }

        public int DeleteMultiRecord(string ListEntityIds)
        {
            var sql = "Proc_DeleteMultiEmployee";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("m_ListEmployeeId", ListEntityIds, DbType.String);

            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                // mở kết nối
                sqlConnection.Open();
                MySqlTransaction transaction = sqlConnection.BeginTransaction();

                var rowAffected = sqlConnection.Execute(sql, param: parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
                transaction.Commit();
                // đóng kết nối db
                sqlConnection.Close();
                return rowAffected;
            }
        }

        public Stream ExportToExcel(Stream stream = null)
        {
            var list = this.GetAll().ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Truyền vào một stream hoặc Memory Stream để thao tác với file Excel.
            using (var package = new ExcelPackage(stream ?? new MemoryStream()))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                // Add Sheet vào file Excel
                var workSheet = package.Workbook.Worksheets.Add(Resources.ExcelSheetName);

                // lấy các thuộc tính của nhân viên
                var properties = typeof(Employee).GetProperties();
                int col = 1;
                int indexHeader = 2;
                // TABLE HEADER cho file excel
                foreach (var prop in properties)
                {
                    // lấy tên hiển thị đầu tiên của thuộc tính
                    var PropertyNameAttributes = prop.GetCustomAttributes(typeof(ColumnFileExport), true);

                    if (PropertyNameAttributes.Length > 0)
                    {
                        // add vào header của file excel
                        workSheet.Cells[3, indexHeader].Value = (PropertyNameAttributes[0] as ColumnFileExport).Name;
                        // tăng số cột lên 1
                        col++;
                        indexHeader++;
                    }
                }
                // Lấy ra tên cột cuối cùng
                var columnEnd = this.ConvertNumberToLetterExcel(col);

                // TABLE BODY
                // đổ dữ liệu vào excel
                // duyệt các nhân viên
                for (int i = 0; i < list.Count(); i++)
                {
                    int indexColBody = 2; // chỉ số cột
                    // lấy ra STT
                    workSheet.Cells[i + 4, 1].Value = i + 1;

                    // duyệt các thuộc tính để tương tự với phần header
                    for (int j = 1; j < properties.Length; j++)
                    {
                        var propertyNameAttr = properties[j].GetCustomAttributes(typeof(ColumnFileExport), true);

                        // nếu thuộc tính có xuất file: cột +1
                        if (propertyNameAttr.Length > 0)
                        {
                            // xử lí các kiểu dữ liệu datetime
                            if ((propertyNameAttr[0] as ColumnFileExport).Name == "Ngày sinh")
                            {
                                if (properties[j].GetValue(list[i]) != null && !string.IsNullOrEmpty(properties[j].GetValue(list[i]).ToString().Trim()))
                                {
                                    DateTime dt = DateTime.ParseExact(properties[j].GetValue(list[i]).ToString(), "d/M/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                    workSheet.Cells[i + 4, indexColBody].Value = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                            }
                            else // các kiểu dữ liệu khác datetime
                            {
                                workSheet.Cells[i + 4, indexColBody].Value = properties[j].GetValue(list[i]);

                            }
                            indexColBody++;
                        }
                        // độ rộng tự động fit với dữ liệu
                        workSheet.Column(j).AutoFit();
                    }
                }

                // Table style
                workSheet.DefaultRowHeight = 20;
                // Set default width cho tất cả column
                workSheet.Cells.AutoFitColumns();
                // Tự động xuống hàng khi text quá dài
                //workSheet.Cells.Style.WrapText = true;
                // Gộp cột
                workSheet.Cells[$"A1:{columnEnd}1"].Merge = true;
                workSheet.Cells[$"A1:{columnEnd}1"].Value = Resources.ExcelSheetName;
                workSheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Size = 16;
                workSheet.Row(1).Style.Font.Name = workSheet.Row(3).Style.Font.Name = "Arial";
                workSheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells[$"A2:{columnEnd}2"].Merge = true;

                workSheet.Row(3).Style.Font.Bold = true;
                workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(3).Style.Font.Size = 10;

                workSheet.Cells[$"A3:{columnEnd}3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[$"A3:{columnEnd}3"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D8D8D8"));
                // border
                workSheet.Cells[$"A3:{columnEnd}{list.Count() + 3}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[$"A3:{columnEnd}{list.Count() + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[$"A3:{columnEnd}{list.Count() + 3}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[$"A3:{columnEnd}{list.Count() + 3}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[3, 1].Value = "STT";

                // lưu lại
                package.Save();
                return package.Stream;
            }
        }

        /// <summary>
        /// Thực hiện convert chữ số sang tên cột trong excel
        /// </summary>
        /// <param name="col">chữ số</param>
        /// <returns>Ký tự tên cột trong excel</returns>
        private string ConvertNumberToLetterExcel(int col)
        {
            string columnName = "";

            while (col > 0)
            {
                int modulo = (col - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                col = (col - modulo) / 26;
            }

            return columnName;
        }
        
        #endregion
    }
}
