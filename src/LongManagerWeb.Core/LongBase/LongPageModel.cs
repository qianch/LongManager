using LongManagerWeb.Core.WebDataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerWeb.Core.LongBase
{
    public class LongPageModel : PageModel
    {
        protected readonly LongWebDbContext _longWebDbContext;

        public LongPageModel(LongWebDbContext longWebDbContext)
        {
            _longWebDbContext = longWebDbContext;
        }
    }
}
