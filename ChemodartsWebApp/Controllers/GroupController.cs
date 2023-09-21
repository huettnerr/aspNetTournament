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
    public class GroupController : Controller
    {
        private readonly ChemodartsContext _context;

        public GroupController(ChemodartsContext context)
        {
            _context = context;
        }

        // GET: Gruppenansicht
        public async Task<IActionResult> Group(int? tournamentId, int? roundId, int? groupId)
        {
            Group? g = await ControllerHelper.QueryId(groupId, _context.Groups);
            if (g is null)
            {
                return NotFound();
            }
            else
            {
                return View("GroupOverview", g);
            }
        }
    }
}
