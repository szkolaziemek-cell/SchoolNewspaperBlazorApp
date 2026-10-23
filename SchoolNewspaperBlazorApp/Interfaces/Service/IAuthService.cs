namespace SchoolNewspaperBlazorApp.Interfaces.Service
{
    /// <summary>Udostępnia operacje związane z kontami i logowaniem.</summary>
    public interface IAuthService
    {
        /// <summary>Tworzy konto z panelu administratora (wymaga uprawnienia AddUser). Zwraca true, gdy konto powstało.</summary>
        Task<bool> AddUserAsync(string login, string password, string email);
        /// <summary>Publiczna rejestracja. Nowe konto nie dostaje żadnych uprawnień. Zwraca true, gdy konto powstało.</summary>
        Task<bool> RegisterAsync(string login, string password, string email);
        /// <summary>Weryfikuje dane logowania i zwraca identyfikator użytkownika (0 przy porażce).</summary>
        Task<int> CheckIfRegisteredAsync(string login, string password);
        /// <summary>Pobiera użytkowników do panelu administracyjnego.</summary>
        Task<Dictionary<int, string>> GetAllUsersAsync();
    }
}