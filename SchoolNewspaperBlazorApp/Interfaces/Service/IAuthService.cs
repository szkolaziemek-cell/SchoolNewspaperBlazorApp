namespace SchoolNewspaperBlazorApp.Interfaces.Service
{
    /// <summary>Udostępnia operacje związane z kontami i logowaniem.</summary>
    public interface IAuthService
    {
        /// <summary>Tworzy konto nowego użytkownika.</summary>
        Task AddUserAsync(string login, string password, string email);
        /// <summary>Weryfikuje dane logowania i zwraca identyfikator użytkownika.</summary>
        Task<int> CheckIfRegisteredAsync(string login, string password);
        /// <summary>Pobiera użytkowników do panelu administracyjnego.</summary>
        Task<Dictionary<int, string>> GetAllUsersAsync();
    }
}
