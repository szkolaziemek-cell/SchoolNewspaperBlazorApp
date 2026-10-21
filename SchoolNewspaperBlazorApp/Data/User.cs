namespace SchoolNewspaperBlazorApp.Data
{
    /// <summary>Reprezentuje konto użytkownika aplikacji.</summary>
    public class User
    {    
        /// <summary>Unikalny identyfikator użytkownika.</summary>
        public int Id { get; set;}
        /// <summary>Login użytkownika.</summary>
        public required string Login { get; set; }
        /// <summary>Hasło zapisane w postaci skrótu.</summary>
        public required string Password { get; set; }
        /// <summary>Adres e-mail użytkownika.</summary>
        public required string Email { get; set; }
        /// <summary>Data utworzenia konta.</summary>
        public required DateTime RegisterDate { get; set; }
        /// <summary>Informacja o osobie, która utworzyła konto.</summary>
        public string AddBy {  get; set; } = string.Empty;
        /// <summary>Uprawnienia przypisane do użytkownika.</summary>
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}
