using ChemodartsWebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChemodartsWebApp.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ChemodartsContext _context;

        public GroupsController(ChemodartsContext context)
        {
            _context = context;
        }
    }
}
