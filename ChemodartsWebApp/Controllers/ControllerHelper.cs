using ChemodartsWebApp.Data;
using ChemodartsWebApp.Models;
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

        public static Dictionary<string, List<string>> ALLOWED_ROUTE_ATTRIBUTES = new Dictionary<string, List<string>>()
        {
            { "Players", new List<string>() { "action", "playerId" } },
            { "Tournament", new List<string>() { "action", "tournamentId", "" } },
            { "Round", new List<string>() { "action", "tournamentId", "roundId" } },
            { "Seed", new List<string>() { "action", "tournamentId", "seedId" } },
            { "Settings", new List<string>() { "action", "tournamentId", "id" } },
            { "Group", new List<string>() { "action", "tournamentId", "roundId", "groupId" } },
            { "Match", new List<string>() { "action", "tournamentId", "roundId", "matchId", "showAll", "editMatchId" } },
            { "Venue", new List<string>() { "action", "tournamentId", "roundId", "venueId" } },
        };

        public static bool IsRouteAttributeAllowed(string routeName, string attributeName)
        {
            if(!ALLOWED_ROUTE_ATTRIBUTES.ContainsKey(routeName))
            {
                //Route has no restrictions 
                return true;
            }

            return ALLOWED_ROUTE_ATTRIBUTES[routeName].Contains(attributeName);
        }
    }
}
