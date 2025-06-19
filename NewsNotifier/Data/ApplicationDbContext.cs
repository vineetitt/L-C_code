using Microsoft.EntityFrameworkCore;
using NewsAggregator.Server.Models.Entities;
using NewsNotifier.Models.Entities;

namespace NewsNotifier.Data
{
    public class ApplicationDbContext : DbContext

    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<NormalUser> NormalUsers { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<ExternalApiConfig> ExternalApis { get; set; }
        public DbSet<SavedArticle> SavedArticles { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
