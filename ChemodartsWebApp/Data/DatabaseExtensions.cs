using ChemodartsWebApp.ModelHelper;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Numerics;

namespace ChemodartsWebApp.Data
{
    public static class DatabaseExtensions
    {
        private static DbContext GetContext<T>(this DbSet<T> set) where T : class
        {
            return set.GetService<ICurrentDbContext>().Context;
        }

        public static ValueTask<T?> QueryId<T>(this DbSet<T> dbSet, int? id) where T : class
        {
            if (id == null || dbSet == null) return new ValueTask<T?>(result: null);

            return dbSet.FindAsync(id);
        }

        public static async Task<T?> CreateWithFactory<T>(this DbSet<T> dbSet, ModelStateDictionary modelState, FactoryBase<T> factory) where T : class
        {
            if (!modelState.IsValid) return null;

            T? created = factory.Create();
            if (created is null) return null;

            dbSet.Add(created);
            await dbSet.GetContext().SaveChangesAsync();

            return created;
        }

        public static async Task<bool> EditWithFactory<T>(this DbSet<T> dbSet, int? id, ModelStateDictionary modelState, FactoryBase<T> factory) where T : class
        {
            //Check model state
            if (!modelState.IsValid) return false;

            //query database entry and try to update the values
            T? obj = await dbSet.QueryId(id);
            if(obj is null) return false;

            //Update the model
            factory.Update(ref obj);

            try
            {
                //Get and Update Context
                DbContext c = dbSet.GetContext();
                c.Update(obj);
                await c.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
    }
}
