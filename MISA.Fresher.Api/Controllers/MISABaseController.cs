using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MISA.Fresher.Infrastructure;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Service;

namespace MISA.Fresher.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MISABaseController<T> : ControllerBase
    {
        IBaseRepository<T> _baseRepository;
        IBaseService<T> _baseService;

        public MISABaseController(IBaseRepository<T> baseRepository, IBaseService<T> baseService)
        {
            _baseRepository = baseRepository;
            _baseService = baseService;
        }

        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _baseRepository.GetAll();
            return Ok(entities);
        }

        /// <summary>
        /// Lấy đối tượng theo khóa chính
        /// </summary>
        /// <param name="Id">Khóa chính</param>
        /// <returns>Đối tượng có khóa chính cần lấy</returns>
        [HttpGet("{Id}")]
        public IActionResult GetById(Guid Id)
        {
            var entity = _baseRepository.GetById(Id);
            return Ok(entity);
        }

        /// <summary>
        /// Thêm mới bản ghi vào trong cơ sở dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>Số bản ghi thêm mới thành công</returns>
        [HttpPost]
        public IActionResult Insert(T entity)
        {
            var res = _baseService.Insert(entity);
            return StatusCode(201, res);
        }

        /// <summary>
        /// Cập nhật bản ghi vào cơ sở dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần sửa</param>
        /// <param name="entityId">Khóa chính của đối tượng</param>
        /// <returns>Số bản ghi cập nhật thành công</returns>
        [HttpPut("{Id}")]
        public IActionResult Update(T entity, Guid Id)
        {
            var res = _baseService.Update(entity, Id);
            if (res > 0)
            {
                return StatusCode(200, res);
            }
            return null;
        }

        /// <summary>
        /// Xóa bản ghi dựa vào khóa chính
        /// </summary>
        /// <param name="entityId">Khóa chính</param>
        /// <returns>Số lượng bản ghi xóa thành công</returns>
        [HttpDelete]
        public IActionResult Delete(Guid entityId)
        {
            var res = _baseRepository.Delete(entityId);
            if (res > 0)
            {
                return StatusCode(200, res);
            }
            return null;
        }
    }
}
