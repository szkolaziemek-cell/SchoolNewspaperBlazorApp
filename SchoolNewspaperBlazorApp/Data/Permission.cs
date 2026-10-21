using SchoolNewspaperBlazorApp.Data.Enums;
using System.Runtime.Serialization;

namespace SchoolNewspaperBlazorApp.Data
{
    /// <summary>Definiuje uprawnienie dostępne w systemie.</summary>
    public class Permission
    {
        /// <summary>Unikalny identyfikator definicji uprawnienia.</summary>
        public int Id { get; set; }
        /// <summary>Techniczna nazwa uprawnienia używana w kodzie.</summary>
        public EnumPermissions PermissionName { get; set; }
        /// <summary>Nazwa przeznaczona do prezentacji użytkownikowi.</summary>
        public string PermissionViewName { get; set; } = string.Empty;
        /// <summary>Relacje użytkowników posiadających to uprawnienie.</summary>
        public ICollection<UserPermission> Permissions { get; set; } = new List<UserPermission>();
    }
}
