using LongManagerWeb.Core.LongBase;
using LongManagerWeb.Core.WebDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LongManagerWeb.Pages
{
    public class LoginHistoryModel : LongPageModel
    {
        public LoginHistoryModel(LongWebDbContext longWebDbContext)
            : base(longWebDbContext)
        { }

        public IList<LoginHistory> Historys { get; set; }

        public async Task OnGetAsync()
        {
            Historys = await _longWebDbContext.LoginHistory.ToListAsync();
        }
    }
}