using LongManagerWeb.Core.LongBase;
using LongManagerWeb.Core.WebDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LongManagerWeb.Pages
{
    public class LoginModel : LongPageModel
    {
        public LoginModel(LongWebDbContext longWebDbContext)
            : base(longWebDbContext)
        {

        }
        public void OnGet()
        {

        }
    }
}