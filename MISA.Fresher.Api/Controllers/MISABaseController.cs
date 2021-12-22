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
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _baseRepository.GetAll();
            return Ok(entities);
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
    }
}
