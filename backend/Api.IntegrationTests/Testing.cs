using MediatR;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace Api.IntegrationTests;

[SetUpFixture]
[Timeout(36_000_000)]
public class Testing
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    
    static Testing()
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory
            .Services
            .GetRequiredService<IServiceScopeFactory>();
        _configuration = _factory
            .Services
            .GetRequiredService<IConfiguration>();
    }

    public static async Task ResetState()
    {
        var connectionString = _configuration
            .GetConnectionString("DefaultDb");

        if (CanResetDatabase(connectionString))
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope
                .ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            // Delete database (test) if exists
            await context.Database.EnsureDeletedAsync();

            // Create database based on Api/Database/Migrations folder
            await context.Database.MigrateAsync();
        }
    }

    private static bool CanResetDatabase(
        string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("ConnectionString should not be empty");
        }

        bool hasTestDbInText = connectionString
            .Contains("TestDb");
        return hasTestDbInText;
    }

    public static async Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var sender = scope.ServiceProvider
            .GetRequiredService<ISender>();

        return await sender.Send(request);
    }

    public static async Task<TEntity?> FindAsync<TEntity>(
        params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope
            .ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task<IEnumerable<TEntity>> GetList<TEntity>(
        Expression<Func<TEntity, bool>> predicate = null)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope
            .ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        IQueryable<TEntity> query = context
            .Set<TEntity>();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        IEnumerable<TEntity> rows = await query.ToListAsync();
        return rows;
    }

    public static async Task AddAsync<TEntity>(
        TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await context.AddAsync(entity);

        await context.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {

    }
}
