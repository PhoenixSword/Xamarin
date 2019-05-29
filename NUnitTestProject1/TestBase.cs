using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace NUnitTestProject1
{
    public abstract class TestBase
    {
        public ApplicationDbContext GetDbContext()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlite("DataSource=:memory:", x=> { });
            var dbContext = new ApplicationDbContext(builder.Options);
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
