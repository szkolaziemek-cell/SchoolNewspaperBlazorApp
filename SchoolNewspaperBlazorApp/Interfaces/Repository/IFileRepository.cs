using SchoolNewspaperBlazorApp.Data;

namespace SchoolNewspaperBlazorApp.Interfaces.Repository
{
    /// <summary>Definiuje operacje zapisu i odczytu metadanych plików.</summary>
    public interface IFileRepository
    {
        /// <summary>Pobiera ostatni użyty identyfikator pliku.</summary>
        Task<int> GetLastFileId();
        /// <summary>Zapisuje metadane przesłanego pliku.</summary>
        Task AddFileAsync(MediaFile file);
        /// <summary>Pobiera metadane pliku po identyfikatorze.</summary>
        Task<MediaFile> GetFileByIdAsync(int id);
    }
}
