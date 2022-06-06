using Microsoft.Extensions.DependencyInjection;
using VotingSystem.Database;

namespace VotingSystem.Integration.Tests.Infrastructure;

public static class DbContextUtils
{
    public static void ActionDatabase(IServiceProvider provider, Action<AppDbContext> action)
    {
        using (var scope = provider.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            action(ctx);
        }
    }
}
