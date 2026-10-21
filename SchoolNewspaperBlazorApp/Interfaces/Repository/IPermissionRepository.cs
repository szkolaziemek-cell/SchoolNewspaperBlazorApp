using SchoolNewspaperBlazorApp.Data.Enums;
using System.Security.Claims;

namespace SchoolNewspaperBlazorApp.Interfaces.Repository
{
    /// <summary>Definiuje operacje bazodanowe dla uprawnień.</summary>
    public interface IPermissionRepository
    {
        /// <summary>Sprawdza, czy zalogowany użytkownik ma wskazane uprawnienie.</summary>
        Task<bool> HasPermissionAsync(ClaimsPrincipal principal, EnumPermissions permission);
        /// <summary>Zastępuje uprawnienia użytkownika podaną listą.</summary>
        Task SavePermissionsAsync(List<EnumPermissions> permissions, int userId);
        /// <summary>Pobiera uprawnienia wybranego użytkownika.</summary>
        Task<List<EnumPermissions>> GetUserPermissionsAsync(int userId);
        /// <summary>Pobiera mapę wszystkich uprawnień i ich stan dla użytkownika.</summary>
        Task<Dictionary<EnumPermissions, bool>> GetAllUserPermissionsAsync(int userId);
    }
}
