using LongManagerWeb.Core.LongBase;
using LongManagerWeb.Core.WebDataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LongManagerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameUserController : LongControllerBase
    {
        public FrameUserController(LongWebDbContext longWebDbContext)
            : base(longWebDbContext)
        { }

        [HttpPost]
        public ActionResult<IEnumerable<FrameUser>> Post()
        {
            return _longWebDbContext.FrameUsers.ToList();
        }
    }
}