using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Chemodarts.Models;

namespace Chemodarts.Data
{
    public class ChemodartsContext : DbContext
    {
        public ChemodartsContext (DbContextOptions<ChemodartsContext> options)
            : base(options)
        {
        }

        public DbSet<Chemodarts.Models.Player> Player { get; set; } = default!;
    }
}
