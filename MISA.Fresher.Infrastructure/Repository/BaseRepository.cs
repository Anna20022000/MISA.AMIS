using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        protected string _connectionString = string.Empty;
        protected string _className;
        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CukCuk");
            _className = typeof(T).Name;
        }
        public int Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> GetAll()
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                // Thực thi lấy dữ liệu trong db:
                var entities = sqlConnection.Query<T>(sql: $"SELECT * FROM {_className}");
                return entities;
            }
        }

        public int GetById()
        {
            throw new NotImplementedException();
        }

        public int Insert(T entity)
        {
            //
            using (MySqlConnection mySqlConnection = new MySqlConnection(_connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                var columns = "";
                var columnParams = "";
                // Lấy ra  các property của đối tượng:
                var props = typeof(T).GetProperties();
                // Duyet tung properties
                foreach (var prop in props)
                {
                    // Lay ra ten cua prop
                    var propName = prop.Name;
                    // Lay ra gia tri cua prop tuong ung voi doi tuong:
                    var propValue = prop.GetValue(entity);
                    if (propName == $"{_className}Id" && prop.PropertyType == typeof(Guid))
                    {
                        propValue = Guid.NewGuid();
                    }
                    // Cập nhật vào chuỗi lệnh thêm mới và add các tham số tương ứng:
                    columns += $"{propName},";
                    columnParams += $"@{propName},";
                    parameters.Add($"@{propName}", propValue);
                }
                columns = columns.Substring(0, columns.Length - 1);
                columnParams = columnParams.Substring(0, columnParams.Length - 1);

                var sql = $"INSERT INTO {_className}({columns}) VALUES ({columnParams})";
                var rowAffected = mySqlConnection.Execute(sql, param: parameters);
                return rowAffected;
            }
        }

        public int Update(T entity, Guid entityId)
        {
            throw new NotImplementedException();
        }
    }
}
