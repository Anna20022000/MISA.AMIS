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

        public object GetPaging(int limit, int pageIndex)
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                var sql = $"Proc_GetEmployeePaging";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("m_SearchText", "");
                parameters.Add("m_PageSize", limit);
                parameters.Add("m_PageIndex", pageIndex);
                parameters.Add("m_TotalRecord", direction: ParameterDirection.Output);
                // Thực thi lấy dữ liệu trong db:
                var entities = sqlConnection.Query<Employee>(sql, param: parameters, commandType: CommandType.StoredProcedure);
                var totalRecord = parameters.Get<int>("m_TotalRecord");

                return new {
                    TotalRecord = totalRecord,
                    Data = entities
                };
            }
        }
    }
}
