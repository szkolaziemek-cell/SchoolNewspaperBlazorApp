using Microsoft.EntityFrameworkCore;
using SchoolNewspaperBlazorApp.Data;
using SchoolNewspaperBlazorApp.Interfaces.Repository;

namespace SchoolNewspaperBlazorApp.Repository
{
    /// <summary>Obsługuje metadane zdjęć zapisane w bazie danych.</summary>
    public class FileRepository : IFileRepository
    {
        private readonly IDbContextFactory<NewspaperDbContext> _contextFactory;
        public FileRepository(IDbContextFactory<NewspaperDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<int> GetLastFileId()
        {
            // Id jest generowane ręcznie, więc wybieramy następny numer po największym.
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var id = await _context.Files
                .OrderByDescending(x => x.Id)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            return id + 1;
        }
        public async Task<MediaFile> GetFileByIdAsync(int id)
        {
            // Artykuł przechowuje tylko FileId, dlatego tutaj odnajdujemy nazwę pliku.
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var fileId = await _context.Files.FirstOrDefaultAsync(a => a.Id == id);
            if(fileId == null)
            {
                throw new Exception("[FileRepository]Brak zdjecia w bazie danych");
            }
            return fileId;
        }
        public async Task AddFileAsync(MediaFile file)
        {
            // Plik fizyczny jest zapisany przez FileService, a tutaj zapisujemy jego metadane.
            await using var _context = await _contextFactory.CreateDbContextAsync();

            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
        }

    }
}
