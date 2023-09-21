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
    public class MatchController : Controller
    {
        private readonly ChemodartsContext _context;

        public MatchController(ChemodartsContext context)
        {
            _context = context;
        }
    }
}
