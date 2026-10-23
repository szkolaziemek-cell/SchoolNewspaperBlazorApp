using SchoolNewspaperBlazorApp.Data;

namespace SchoolNewspaperBlazorApp.Interfaces.Repository
{
    /// <summary>Definiuje dostęp do danych kont użytkowników.</summary>
    public interface IAuthRepository
    {
        /// <summary>Zapisuje nowe konto użytkownika.</summary>
        Task AddUserAsync(User user);
        /// <summary>Znajduje konto po adresie e-mail podanym przy logowaniu.</summary>
        Task<User> CheckIfRegisteredAsync(string login);
        /// <summary>Pobiera identyfikatory i adresy e-mail wszystkich użytkowników.</summary>
        Task<Dictionary<int, string>> GetAllUsersAsync();
        /// <summary>Sprawdza, czy istnieje już konto z takim loginem lub adresem e-mail.</summary>
        Task<bool> LoginOrEmailExistsAsync(string login, string email);
    }
}