using SchoolNewspaperBlazorApp.Data;

namespace SchoolNewspaperBlazorApp.Interfaces.Repository
{
    /// <summary>Definiuje operacje bazodanowe dotyczące artykułów.</summary>
    public interface IArticleRepository
    {
        /// <summary>Dodaje artykuł i zwraca informację, czy zapis się udał.</summary>
        Task<bool> AddArticleAsync(Article article);
        /// <summary>Pobiera wszystkie artykuły.</summary>
        Task<List<Article>> GetAllArticlesAsync();
        /// <summary>Pobiera jeden artykuł po identyfikatorze.</summary>
        Task<Article> GetArticleByIdAsync(int id);
        /// <summary>Usuwa artykuł o wskazanym identyfikatorze.</summary>
        Task RemoveArticleByIdAsync(int id);
        /// <summary>Aktualizuje dane istniejącego artykułu.</summary>
        Task UpdateArticleByIdAsync(Article article);
    }
}
