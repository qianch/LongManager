using LongManagerWeb.Core.WebDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LongManagerWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly LongWebDbContext _longWebDbContext;
        public IndexModel(LongWebDbContext longWebDbContext)
        {
            _longWebDbContext = longWebDbContext;
        }
        public void OnGet()
        {
            var frameUsers = _longWebDbContext.FrameUsers.ToList();
        }
    }
}
