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
        /// <param name="entityId">Khóa chính</param>
        /// <returns>Đối tượng có khóa chính cần lấy</returns>
        [HttpGet("GetById")]
        public IActionResult GetById(Guid entityId)
        {
            var entity = _baseRepository.GetById(entityId);
            if(entity.Count() > 0)
            {
                return Ok(entity);
            }
            return null;
        }

        /// <summary>
        /// Thêm mới bản ghi vào trong cơ sở dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert(T entity)
        {
            var res = _baseService.Insert(entity);
            return StatusCode(201, res);
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
            if(res > 0)
            {
                return StatusCode(200, res);
            }
            return null;
        }
    }
}
