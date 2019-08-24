using KLTN.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace KLTN.DataAccess
{
    public class DbInitializer
    {
        public static void Initialize(EcommerceDbContext context)
        {
            if (!((RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>()).Exists())
            {
                context.Database.EnsureCreated();
            }
            else
            {
                context.Database.Migrate();
            }
        }
    }
}
