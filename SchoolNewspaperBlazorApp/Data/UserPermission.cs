namespace SchoolNewspaperBlazorApp.Data
{
    /// <summary>Łączy użytkownika z jednym przypisanym uprawnieniem.</summary>
    public class UserPermission
    {
        /// <summary>Identyfikator użytkownika w relacji.</summary>
        public int UserId { get; set; }
        /// <summary>Identyfikator uprawnienia w relacji.</summary>
        public int PermissionId { get; set; }
        /// <summary>Nazwa uprawnienia przechowywana pomocniczo.</summary>
        public string PermissionName { get; set; } = string.Empty;

        /// <summary>Nawigacja do użytkownika.</summary>
        public User User { get; set; } = null!;
        /// <summary>Nawigacja do definicji uprawnienia.</summary>
        public Permission Permission { get; set; } = null!;
    }
}
