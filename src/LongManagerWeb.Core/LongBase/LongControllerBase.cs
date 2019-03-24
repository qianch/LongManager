using LongManagerWeb.Core.WebDataBase;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerWeb.Core.LongBase
{
    public class LongControllerBase : ControllerBase
    {
        protected readonly LongWebDbContext _longWebDbContext;

        public LongControllerBase(LongWebDbContext longWebDbContext)
        {
            _longWebDbContext = longWebDbContext;
        }
    }
}
