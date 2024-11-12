using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using pupconect.Data;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace pupconect.Pages
{
    public class DashboardModel : PageModel
    {
        public string Username { get; set; }
        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
        }

    }
}