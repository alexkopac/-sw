using Auth.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Identity;


namespace Auth.Auth.InteractionTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private DbConnection _connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                services.RemoveAll(typeof(DbContextOptions<DatabaseContext>));
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();
                services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(_connection));

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                db.Database.EnsureCreated();

                var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<AppRole>>();

                SeedRoles(roleManager).GetAwaiter().GetResult();
            });

            builder.UseSetting("Jwt:Key", "Test_Chimin");
            builder.UseSetting("Jwt:Issuer", "AuthJwtApi");
            builder.UseSetting("Jwt:Audience", "AuthJwtApiClient");
            builder.UseSetting("Jwt:AccessTokenMinutes", "30");
            builder.UseSetting("Jwt:RefreshTokenDays", "7");
    
            base.ConfigureWebHost(builder);

        }

        private static async Task SeedRoles(RoleManager<AppRole> roleManager)
        {
            string[] roles = { "User", "Admin", "Manager" };
            foreach (var role in roles)
            {
                var exist = await roleManager.RoleExistsAsync(role);
                if (!exist)
                {
                    await roleManager.CreateAsync(new AppRole
                    {
                        Id = Guid.NewGuid(),
                        Name = role
                    });
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
