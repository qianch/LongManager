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
    public class LoginHistoryController : LongControllerBase
    {
        public LoginHistoryController(LongWebDbContext longWebDBContext)
            : base(longWebDBContext)
        { }

        [HttpPost]
        public void Post([FromBody] IEnumerable<LoginHistory> loginHistorys)
        {
            _longWebDbContext.LoginHistory.AddRange(loginHistorys);
            _longWebDbContext.SaveChanges();
        }
    }
}