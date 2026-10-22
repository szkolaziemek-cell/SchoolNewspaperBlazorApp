using Microsoft.EntityFrameworkCore;
using SchoolNewspaperBlazorApp.Data;
using SchoolNewspaperBlazorApp.Interfaces.Repository;

namespace SchoolNewspaperBlazorApp.Repository
{
    /// <summary>Wykonuje operacje CRUD artykułów przy użyciu Entity Framework.</summary>
    public class ArticleRepository : IArticleRepository
    {
        // Fabryka tworzy osobny kontekst dla każdej operacji asynchronicznej.
        private readonly IDbContextFactory<NewspaperDbContext> _contextFactory;

        /// <summary>Inicjalizuje repozytorium fabryką kontekstu bazy.</summary>
        public ArticleRepository(IDbContextFactory<NewspaperDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task UpdateArticleByIdAsync(Article article)
        {
            // Pobieramy istniejący rekord, aby aktualizować tylko jego dane.
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var existingArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Id == article.Id);
            if (existingArticle == null)
            {
                return;
            }
            if (article.FileId != 0)
            {
                existingArticle.FileId = article.FileId;
            }
            // Brak nowej okładki oznacza pozostawienie dotychczasowej.
            if (article.CoverFileId != null)
            {
                existingArticle.CoverFileId = article.CoverFileId;
            }
            existingArticle.Title = article.Title;
            existingArticle.Text = article.Text;
            existingArticle.Author = article.Author;

            await _context.SaveChangesAsync();

        }
        public async Task<bool> AddArticleAsync(Article article)
        {
            // Wynik false pozwala serwisowi wyświetlić użytkownikowi komunikat błędu.
            await using var _context = await _contextFactory.CreateDbContextAsync();
            try
            {
                await _context.Articles.AddAsync(article);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async Task<List<Article>> GetAllArticlesAsync()
        {
            // Lista jest używana zarówno na stronie głównej, jak i w panelu administratora.
            await using var _context = await _contextFactory.CreateDbContextAsync();

            return await _context.Articles.ToListAsync();

        }
        public async Task<Article> GetArticleByIdAsync(int id)
        {
            // Brak rekordu jest sygnalizowany wyjątkiem obsługiwanym wyżej w aplikacji.
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
            if (article == null)
            {
                throw new Exception($"Article with ID {id} not found.");
            }
            return article;
        }
        public async Task RemoveArticleByIdAsync(int id)
        {
            // Usunięcie odbywa się w dwóch krokach: wyszukanie rekordu i zapis zmian.
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
            if (article == null)
            {
                throw new Exception($"Article with ID {id} not found.");
            }
            else
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }


        }
    }
}