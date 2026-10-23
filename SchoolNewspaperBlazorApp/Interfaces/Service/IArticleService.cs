using SchoolNewspaperBlazorApp.Data;
using SchoolNewspaperBlazorApp.Data.Enums;

namespace SchoolNewspaperBlazorApp.Interfaces.Service
{
    /// <summary>Udostępnia logikę biznesową zarządzania artykułami.</summary>
    public interface IArticleService
    {
        /// <summary>Tworzy artykuł po sprawdzeniu uprawnienia użytkownika. Okładka i hasztagi są opcjonalne, a wydarzenie wymaga daty.</summary>
        Task AddArticleAsync(string title, string text, string author, int fileId, int? coverFileId = null, string? tags = null, DateTime? eventDate = null, EnumArticleType articleType = EnumArticleType.News);
        /// <summary>Zwraca wszystkie artykuły dostępne w systemie.</summary>
        Task<List<Article>> GetAllArticlesAsync();
        /// <summary>Zwraca artykuł o wskazanym identyfikatorze.</summary>
        Task<Article> GetArticleByIdAsync(int id);
        /// <summary>Usuwa artykuł i zwraca wynik operacji.</summary>
        Task<bool> RemoveArticleByIdAsync(int id);
        /// <summary>Aktualizuje treść artykułu po sprawdzeniu uprawnienia. Zero w fileId i null w coverFileId oznaczają brak zmiany zdjęcia.</summary>
        Task UpdateArticleByIdAsync(int id, string title, string text, string author, int fileId, int? coverFileId = null, string? tags = null, DateTime? eventDate = null, EnumArticleType articleType = EnumArticleType.News);

    }
}