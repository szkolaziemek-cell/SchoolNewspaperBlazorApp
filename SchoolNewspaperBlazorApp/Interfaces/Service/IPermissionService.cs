using SchoolNewspaperBlazorApp.Data.Enums;
using System.Security.Claims;

namespace SchoolNewspaperBlazorApp.Interfaces.Service
{
    /// <summary>Udostępnia logikę sprawdzania i nadawania uprawnień.</summary>
    public interface IPermissionService
    {
        /// <summary>Sprawdza uprawnienie aktualnie zalogowanego użytkownika.</summary>
        Task<bool> HasPermissionAsync(EnumPermissions permission);
        /// <summary>Zapisuje uprawnienia wybranego użytkownika.</summary>
        Task SavePermissionsAsync(List<EnumPermissions> permissions, int userId);
        /// <summary>Pobiera uprawnienia wskazanego użytkownika.</summary>
        Task<List<EnumPermissions>> GetUserPermissionsAsync(int userId);
        /// <summary>Tworzy mapę uprawnień bieżącego użytkownika.</summary>
        Task<Dictionary<EnumPermissions, bool>> GetAllPermissionsAsync();
        /// <summary>Pobiera identyfikator użytkownika z jego claims.</summary>
        Task<int> GetUserId();
        /// <summary>Narzędzie deweloperskie: tworzy definicje uprawnień i nadaje użytkownikowi Master. Działa tylko w trybie Development.</summary>
        Task BootstrapMasterAsync(int userId);
    }
}