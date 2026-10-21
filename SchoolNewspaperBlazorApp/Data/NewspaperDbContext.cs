using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace SchoolNewspaperBlazorApp.Data
{
    /// <summary>Główny kontekst Entity Framework dla danych gazetki.</summary>
    public class NewspaperDbContext : DbContext
    {
        /// <summary>Tworzy kontekst na podstawie konfiguracji bazy danych.</summary>
        public NewspaperDbContext(DbContextOptions<NewspaperDbContext> options)
            : base(options) {}
        /// <summary>Zbiór artykułów.</summary>
        public DbSet<Article> Articles { get; set; }
        /// <summary>Zbiór użytkowników.</summary>
        public DbSet<User> Users { get; set; }
        /// <summary>Zbiór plików multimedialnych.</summary>
        public DbSet<MediaFile> Files { get; set; }
        /// <summary>Zbiór definicji uprawnień.</summary>
        public DbSet<Permission> Permissions { get; set; }
        /// <summary>Zbiór relacji użytkownik-uprawnienie.</summary>
        public DbSet<UserPermission> UserPermissions { get; set; }
        /// <summary>Konfiguruje klucze i relacje pomiędzy tabelami.</summary>
        protected override void OnModelCreating(ModelBuilder mb)
        {
            // Id pliku jest nadawane przez FileService, dlatego baza go nie generuje.
            mb.Entity<MediaFile>()
                .Property(x => x.Id)
                .ValueGeneratedNever();
            // Para UserId i PermissionId tworzy klucz złożony relacji wiele-do-wielu.
            mb.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });
            mb.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(u => u.UserId);
            mb.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.Permissions)
                .HasForeignKey(up => up.PermissionId);
        }
    }

}
