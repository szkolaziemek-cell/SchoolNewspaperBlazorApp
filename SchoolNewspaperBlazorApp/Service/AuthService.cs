using Microsoft.AspNetCore.Identity;
using SchoolNewspaperBlazorApp.Data;
using SchoolNewspaperBlazorApp.Data.Enums;
using SchoolNewspaperBlazorApp.Interfaces.Repository;
using SchoolNewspaperBlazorApp.Interfaces.Service;
using System.Net.Mail;

namespace SchoolNewspaperBlazorApp.Service
{
    /// <summary>Obsługuje tworzenie kont, rejestrację i weryfikację danych logowania.</summary>
    public class AuthService : IAuthService
    {
        // Limity są w jednym miejscu, żeby łatwo je zmienić.
        private const int MinLoginLength = 3;
        private const int MaxLoginLength = 50;
        private const int MinPasswordLength = 8;
        private const int MaxPasswordLength = 100;
        private const int MaxEmailLength = 256;

        private readonly IAuthRepository _authRepository;
        private readonly IPermissionService _permissionService;
        private readonly INotificationService _notificationService;
        public AuthService(IAuthRepository authRepository, IPermissionService permissionService, INotificationService notificationService)
        {
            _authRepository = authRepository;
            _permissionService = permissionService;
            _notificationService = notificationService;
        }
        public async Task<Dictionary<int, string>> GetAllUsersAsync()
        {
            // Lista jest wykorzystywana do wyboru użytkownika w panelu uprawnień.
            return await _authRepository.GetAllUsersAsync();
        }
        public async Task<bool> AddUserAsync(string login, string password, string email)
        {
            // Dodawanie kont z panelu jest dostępne tylko dla użytkownika z odpowiednim uprawnieniem.
            bool hasPermission = await _permissionService.HasPermissionAsync(EnumPermissions.AddUser);
            if (!hasPermission)
            {
                _notificationService.Show("Brak uprawnień do dodawania użytkowników!", EnumNotificationType.Failure);
                return false;
            }
            var creatorId = await _permissionService.GetUserId();
            return await CreateUserAsync(login, password, email, $"user:{creatorId}");
        }
        public async Task<bool> RegisterAsync(string login, string password, string email)
        {
            // Rejestracja jest publiczna, ale konto nie dostaje żadnych uprawnień.
            return await CreateUserAsync(login, password, email, "rejestracja");
        }
        public async Task<int> CheckIfRegisteredAsync(string login, string password)
        {
            // Puste dane odrzucamy od razu, bez zapytania do bazy.
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                return 0;
            }
            // PasswordHasher porównuje podane hasło z bezpiecznym skrótem w bazie.
            var hasher = new PasswordHasher<string>();
            try
            {
                var user = await _authRepository.CheckIfRegisteredAsync(login.Trim());
                var result = hasher.VerifyHashedPassword(login, user.Password, password);
                if (result == PasswordVerificationResult.Success)
                {
                    return user.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>Wspólna ścieżka tworzenia konta: walidacja, unikalność, haszowanie i zapis.</summary>
        private async Task<bool> CreateUserAsync(string? login, string? password, string? email, string addBy)
        {
            // Normalizacja: login i e-mail bez spacji na brzegach, e-mail małymi literami.
            login = (login ?? string.Empty).Trim();
            email = (email ?? string.Empty).Trim().ToLowerInvariant();
            password ??= string.Empty;

            var error = Validate(login, password, email);
            if (error != null)
            {
                _notificationService.Show(error, EnumNotificationType.Failure);
                return false;
            }
            try
            {
                if (await _authRepository.LoginOrEmailExistsAsync(login, email))
                {
                    _notificationService.Show("Konto z takim loginem lub adresem e-mail już istnieje!", EnumNotificationType.Failure);
                    return false;
                }
                var hasher = new PasswordHasher<string>();
                var user = new User
                {
                    Login = login,
                    Password = hasher.HashPassword(null!, password),
                    Email = email,
                    RegisterDate = DateTime.Now,
                    AddBy = addBy
                };
                await _authRepository.AddUserAsync(user);
                _notificationService.Show("Pomyślnie utworzono konto!", EnumNotificationType.Success);
                return true;
            }
            catch (Exception)
            {
                _notificationService.Show("Podczas tworzenia konta wystąpił błąd!", EnumNotificationType.Error);
                return false;
            }
        }

        /// <summary>Zwraca komunikat o pierwszym znalezionym błędzie albo null, gdy dane są poprawne.</summary>
        private static string? Validate(string login, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(login))
                return "Login nie może być pusty!";
            if (login.Length < MinLoginLength || login.Length > MaxLoginLength)
                return $"Login musi mieć od {MinLoginLength} do {MaxLoginLength} znaków!";

            if (string.IsNullOrWhiteSpace(email))
                return "Adres e-mail nie może być pusty!";
            if (email.Length > MaxEmailLength || !MailAddress.TryCreate(email, out var parsed) || parsed.Address != email)
                return "Podaj poprawny adres e-mail!";

            if (string.IsNullOrWhiteSpace(password))
                return "Hasło nie może być puste!";
            if (password.Length < MinPasswordLength || password.Length > MaxPasswordLength)
                return $"Hasło musi mieć od {MinPasswordLength} do {MaxPasswordLength} znaków!";

            return null;
        }
    }
}