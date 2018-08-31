namespace UsersRolesSandboxWeb.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Agency> Agencies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<UserGroup>()
                .HasKey(t => new { t.GroupId, t.UserId });
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCD EFGHIJK LMNO PQRST UV WXYZ0 1234 567 89";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static async Task SeedAndMigrateAsync(ApplicationDbContext dbContext)
        {
            await dbContext.Database.MigrateAsync().ConfigureAwait(false);

            if (await dbContext.Agencies.AnyAsync().ConfigureAwait(false))
            {
                return;
            }

            dbContext.Agencies.AddRange(
                new Agency {Name = "Agency1"},
                new Agency {Name = "Agency2"},
                new Agency {Name = "Agency3"}
            );

            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            var agencies = await dbContext.Agencies.ToArrayAsync().ConfigureAwait(false);

            foreach (var agency in agencies)
            {
                dbContext.Users.AddRange(
                    new User {Name = $"User{agency.Id}.1", AgencyId = agency.Id, Description = RandomString(30)},
                    new User {Name = $"User{agency.Id}.2", AgencyId = agency.Id, Description = RandomString(30)},
                    new User {Name = $"User{agency.Id}.3", AgencyId = agency.Id, Description = RandomString(30)}
                );
            }

            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            var users = await dbContext.Users.ToArrayAsync().ConfigureAwait(false);

            dbContext.Groups.AddRange(
                new Group { Name = "Group1" },
                new Group { Name = "Group2" },
                new Group { Name = "Group3" },
                new Group { Name = "Group4" },
                new Group { Name = "Group5" }
            );

            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            var groups = await dbContext.Groups.ToArrayAsync().ConfigureAwait(false);

            foreach (var user in users)
            {
                var beginId = new Random().Next(groups.Min(g => g.Id), groups.Max(g => g.Id));
                var endId = new Random().Next(beginId, groups.Max(g => g.Id));

                dbContext.UserGroups.AddRange(
                    groups
                        .Where(g => g.Id >= beginId && g.Id <= endId)
                        .Select(g => new UserGroup {UserId = user.Id, GroupId = g.Id}));
            }

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
