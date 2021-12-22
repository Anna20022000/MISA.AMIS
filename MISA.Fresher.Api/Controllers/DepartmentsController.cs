using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Fresher.Api.Controllers
{
    public class DepartmentsController : MISABaseController<Department>
    {
        public DepartmentsController(IBaseRepository<Department> baseRepository, IBaseService<Department> baseService)
            :base(baseRepository, baseService)
        {

        }

    }
}
