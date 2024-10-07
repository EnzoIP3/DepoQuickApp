using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess.Test;

internal sealed class DbContextBuilder
{
    private static readonly SqliteConnection Connection = new("Data Source=:memory:");

    public static Context BuildTestDbContext()
    {
        DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>()
            .UseSqlite(Connection)
            .Options;

        Connection.Open();

        var context = new Context(options);

        return context;
    }
}
