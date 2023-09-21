using Microsoft.EntityFrameworkCore;

namespace ChemodartsWebApp.Controllers
{
    public static class ControllerHelper
    {
        public static ValueTask<T?> QueryId<T>(int? id, DbSet<T> set) where T : class
        {
            if (id == null || set == null) return new ValueTask<T?>(result: null);

            return set.FindAsync(id);
        }
    }
}
