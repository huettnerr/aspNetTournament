﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChemodartsWebApp.Data;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using ChemodartsWebApp.ViewModel;

namespace ChemodartsWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChemodartsContext _context;

        public HomeController(ChemodartsContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            return View(new HomeViewModel());
        }
    }
}
