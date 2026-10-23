using Microsoft.EntityFrameworkCore;
using SchoolNewspaperBlazorApp.Data;
using SchoolNewspaperBlazorApp.Data.Enums;
using SchoolNewspaperBlazorApp.Interfaces.Repository;
using System.Security.Claims;

namespace SchoolNewspaperBlazorApp.Repository
{
    /// <summary>Odczytuje i zapisuje relacje użytkowników z uprawnieniami.</summary>
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IDbContextFactory<NewspaperDbContext> _contextFactory;
        public PermissionRepository(IDbContextFactory<NewspaperDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<EnumPermissions>> GetUserPermissionsAsync(int userId)
        {
            // Zwracamy wartości enumów, aby UI nie musiało znać struktury tabel.
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission.PermissionName)
                .ToListAsync();
        }
        public async Task<Dictionary<EnumPermissions, bool>> GetAllUserPermissionsAsync(int userId)
        {
            // Mapa zawiera także uprawnienia nieprzypisane, co upraszcza obsługę checkboxów.
            await using var context = await _contextFactory.CreateDbContextAsync();
            var permissionsList = await GetUserPermissionsAsync(userId);
            return Enum.GetValues<EnumPermissions>()
        .ToDictionary(
            permission => permission,
            permission => permissionsList.Contains(permission)
        );

        }
        public async Task<bool> HasPermissionAsync(ClaimsPrincipal principal, EnumPermissions permission)
        {
            // Id użytkownika jest zapisane w claims utworzonych podczas logowania.
            var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            if (!int.TryParse(id, out var permissionId))
            {
                return false;
            }
            await using var _context = await _contextFactory.CreateDbContextAsync();
            return await _context.UserPermissions
             .AnyAsync(up =>
                 up.UserId == permissionId &&
                 (up.Permission.PermissionName == permission ||
                  up.Permission.PermissionName == EnumPermissions.Master));
        }
        public async Task EnsureMasterAsync(int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            // Dodajemy tylko te definicje, których jeszcze nie ma w tabeli Permissions.
            var existing = await context.Permissions.Select(p => p.PermissionName).ToListAsync();
            foreach (var name in Enum.GetValues<EnumPermissions>().Where(n => !existing.Contains(n)))
            {
                context.Permissions.Add(new Permission
                {
                    PermissionName = name,
                    PermissionViewName = GetViewName(name)
                });
            }
            await context.SaveChangesAsync();

            var masterId = await context.Permissions
                .Where(p => p.PermissionName == EnumPermissions.Master)
                .Select(p => p.Id)
                .FirstAsync();
            var hasMaster = await context.UserPermissions
                .AnyAsync(up => up.UserId == userId && up.PermissionId == masterId);
            if (!hasMaster)
            {
                context.UserPermissions.Add(new UserPermission
                {
                    UserId = userId,
                    PermissionId = masterId,
                    PermissionName = nameof(EnumPermissions.Master)
                });
                await context.SaveChangesAsync();
            }
        }
        private static string GetViewName(EnumPermissions permission) => permission switch
        {
            EnumPermissions.AddArticle => "Dodawanie artykułów",
            EnumPermissions.EditArticle => "Edycja artykułów",
            EnumPermissions.DeleteArticle => "Usuwanie artykułów",
            EnumPermissions.AddUser => "Dodawanie użytkowników",
            EnumPermissions.ManageUser => "Zarządzanie użytkownikami",
            EnumPermissions.AddFile => "Dodawanie plików",
            EnumPermissions.Master => "Administrator",
            _ => permission.ToString()
        };
        public async Task SavePermissionsAsync(List<EnumPermissions> permissions, int userId)
        {
            // Najpierw usuwamy stare uprawnienia, a następnie zapisujemy aktualny wybór.
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var masterId = await _context.Permissions
                .Where(p => p.PermissionName == EnumPermissions.Master)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
            var existingPermissions = _context.UserPermissions
                .Where(up => up.UserId == userId && up.PermissionId != masterId);

            _context.UserPermissions.RemoveRange(existingPermissions);
            foreach (var permission in permissions)
            {
                var permissionId = await _context.Permissions
                    .Where(p => p.PermissionName == permission)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync();
                if (permissionId == 0)
                    continue;
                _context.UserPermissions.Add(new UserPermission
                {
                    UserId = userId,
                    PermissionId = permissionId,
                    PermissionName = permission.ToString()
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}