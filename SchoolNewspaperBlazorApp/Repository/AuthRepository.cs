using Microsoft.EntityFrameworkCore;
using SchoolNewspaperBlazorApp.Data;
using SchoolNewspaperBlazorApp.Interfaces.Repository;

namespace SchoolNewspaperBlazorApp.Repository
{
    /// <summary>Zapewnia dostęp do kont użytkowników w bazie danych.</summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly IDbContextFactory<NewspaperDbContext> _contextFactory;
        public AuthRepository(IDbContextFactory<NewspaperDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Dictionary<int, string>> GetAllUsersAsync()
        {
            // Panel uprawnień potrzebuje identyfikatora i adresu e-mail każdego konta.
            await using var _context = await _contextFactory.CreateDbContextAsync();
            return await _context.Users
                .ToDictionaryAsync(u => u.Id, u => u.Email);
        }
        public async Task AddUserAsync(User user)
        {
            // Hasło jest już zahaszowane przez AuthService przed zapisem.
            await using var _context = await _contextFactory.CreateDbContextAsync();

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> LoginOrEmailExistsAsync(string login, string email)
        {
            // Porównanie wykonuje SQL Server, więc domyślnie nie rozróżnia wielkości liter.
            await using var _context = await _contextFactory.CreateDbContextAsync();
            return await _context.Users
                .AnyAsync(u => u.Login == login || u.Email == email);
        }
        public async Task<User> CheckIfRegisteredAsync(string login)
        {
            // Logowanie odbywa się na podstawie adresu e-mail przekazanego jako login.
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == login);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new Exception("Konto nie istnieje");
            }
        }
    }
}