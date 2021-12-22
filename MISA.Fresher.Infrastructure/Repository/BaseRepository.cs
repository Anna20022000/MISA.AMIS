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

        public bool CheckExist(string propName, string condition)
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                // Thực thi lấy dữ liệu trong db:
                var entitiy = sqlConnection.Query<T>(sql: $"SELECT * FROM {_className} WHERE {propName} = '{condition}'");
                if (entitiy.Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public int Delete(Guid entityId)
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                // Thực thi lấy dữ liệu trong db:
                var entitiy = sqlConnection.Query<T>(sql: $"DELETE FROM {_className} WHERE {_className}Id = '{entityId}'");
                return 1;
            }
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

        public IEnumerable<T> GetById(Guid entityId)
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                // Thực thi lấy dữ liệu trong db:
                var entitiy = sqlConnection.Query<T>(sql: $"SELECT * FROM {_className} WHERE {_className}Id = '{entityId}'");
                return entitiy;
            }
        }

        public int Insert(T entity)
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection mySqlConnection = new MySqlConnection(_connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                var columns = "";
                var columnParams = "";
                // Lấy ra  các property của đối tượng:
                var props = typeof(T).GetProperties();
                // Duyet tung property
                foreach (var prop in props)
                {
                    // Lay ra ten cua prop
                    var propName = prop.Name;
                    // Lay ra gia tri cua prop tuong ung voi doi tuong:
                    var propValue = prop.GetValue(entity);

                    // Nếu property đó là Khóa chính thì có giá trị = mã Guid mới
                    if (propName == $"{_className}Id" && prop.PropertyType == typeof(Guid))
                    {
                        propValue = Guid.NewGuid();
                    }
                    // Nếu property đó là Ngày tạo thì có giá trị = ngày hiện tại
                    if (propName == "CreatedDate" && prop.PropertyType == typeof(DateTime))
                    {
                        propValue = DateTime.Now;
                    }

                    // Nếu prop có chứa attribute ReadOnly thì không thêm vào câu lệnh SQL
                    var propReadOnly = prop.GetCustomAttributes(typeof(MISA.Fresher.Core.MISAAttribute.ReadOnly), true);
                    if (propReadOnly.Length == 0)
                    {
                        // Cập nhật vào chuỗi lệnh thêm mới và add các tham số tương ứng:
                        columns += $"{propName},";
                        columnParams += $"@{propName},";
                        parameters.Add($"@{propName}", propValue);
                    }
                }
                // Cắt bỏ dấu phẩy thừa ở cuối chuỗi
                columns = columns.Substring(0, columns.Length - 1);
                columnParams = columnParams.Substring(0, columnParams.Length - 1);

                // Tạo câu lệnh SQL
                var sql = $"INSERT INTO {_className}({columns}) VALUES ({columnParams})";
                // Thực thi
                var rowAffected = mySqlConnection.Execute(sql, param: parameters);

                return rowAffected;
            }
        }

        public int Update(T entity, Guid entityId)
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection mySqlConnection = new MySqlConnection(_connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                var setValues = "";

                // Lấy ra  các property của đối tượng:
                var props = typeof(T).GetProperties();
                // Duyet tung properties
                foreach (var prop in props)
                {
                    // Lay ra ten cua prop
                    var propName = prop.Name;
                    // Lay ra gia tri cua prop tuong ung voi doi tuong:
                    var propValue = prop.GetValue(entity);

                    // Nếu prop có chứa attribute ReadOnly thì không thêm vào câu lệnh SQL
                    var propReadOnly = prop.GetCustomAttributes(typeof(MISA.Fresher.Core.MISAAttribute.ReadOnly), true);

                    // Nếu property đó không là Khóa chính và không có attr ReadOnly
                    if (propReadOnly.Length == 0 && !(propName == $"{_className}Id" && prop.PropertyType == typeof(Guid)))
                    {
                        // Nếu prop là ModifiedDate thì set giá trị = ngày giờ hiện tại
                        if (propName == "ModifiedDate" && prop.PropertyType == typeof(DateTime))
                        {
                            propValue = DateTime.Now;
                        }
                        // Nếu prop có kiểu dữ liệu là int thì value không chứa dấu ''
                        if(prop.PropertyType == typeof(int))
                        {
                            // Cập nhật vào chuỗi lệnh Sửa và add các tham số = giá trị tương ứng:
                            setValues += $"{propName} = {propValue},";
                        }
                        else
                        {
                            // Cập nhật vào chuỗi lệnh Sửa và add các tham số = giá trị tương ứng:
                            setValues += $"{propName} = '{propValue}',";
                        }
                    }

                }

                // Cắt bỏ dấu phẩy thừa ở cuối chuỗi
                setValues = setValues.Substring(0, setValues.Length - 1);
                // Tạo câu lệnh SQL
                var sql = $"UPDATE { _className} SET {setValues} WHERE {_className}Id = '{entityId}'";
                // Thực thi
                var rowAffected = mySqlConnection.Execute(sql);

                return rowAffected;
            }
        }
    }
}
