using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }
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
                // Thực thi lấy dữ liệu trong db:
                var rowAffected = sqlConnection.Execute(sql, param: parameters, commandType: CommandType.StoredProcedure);
                return rowAffected;
            }
        }
    }
}
