using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChemodartsWebApp.Data;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ChemodartsWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChemodartsContext _context;

        public HomeController(ChemodartsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            return View();
        }
        
        public async Task<IActionResult> Tournaments(int? id)
        {
            return View(await _context.Tournaments.ToListAsync());
        }
    }
}
