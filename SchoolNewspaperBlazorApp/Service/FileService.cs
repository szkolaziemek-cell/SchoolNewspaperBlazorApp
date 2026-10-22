using Microsoft.AspNetCore.Components.Forms;
using SchoolNewspaperBlazorApp.Data;
using SchoolNewspaperBlazorApp.Data.Enums;
using SchoolNewspaperBlazorApp.Interfaces.Repository;
using SchoolNewspaperBlazorApp.Interfaces.Service;

namespace SchoolNewspaperBlazorApp.Service
{
    /// <summary>Obsługuje podgląd, walidację, zapis i wyszukiwanie zdjęć artykułów.</summary>
    public class FileService : IFileService
    {
        // Maksymalny rozmiar jednego zdjęcia.
        private const long MaxImageSize = 5 * 1024 * 1024;

        // Zdjęcie po odczycie i potwierdzeniu, że jego zawartość jest obrazem.
        private sealed record ValidatedImage(byte[] Bytes, string Extension, string MimeType);

        private readonly IFileRepository _fileRepository;
        private readonly IPermissionService _permissionService;
        private readonly string _uploadPath;
        public FileService(IFileRepository fileRepository, IWebHostEnvironment environment, IPermissionService permissionService)
        {
            _fileRepository = fileRepository;
            _permissionService = permissionService;
            _uploadPath = Path.Combine(environment.ContentRootPath, "Images");
        }
        public async Task<string?> ValidateImageAsync(IBrowserFile file)
        {
            var (_, error) = await TryReadImageAsync(file);
            return error;
        }
        public async Task<string> GetPreviewAsync(IBrowserFile file)
        {
            // Podgląd jest kodem Data URL, dzięki czemu nie wymaga wcześniejszego zapisu pliku.
            var (image, _) = await TryReadImageAsync(file);
            if (image == null)
            {
                return string.Empty;
            }
            return $"data:{image.MimeType};base64,{Convert.ToBase64String(image.Bytes)}";
        }
        public async Task<int> UploadImage(IBrowserFile file)
        {
            // Formularz służy do dodawania i edycji, więc wystarczy któreś z tych uprawnień.
            bool canUpload = await _permissionService.HasPermissionAsync(EnumPermissions.AddArticle)
                          || await _permissionService.HasPermissionAsync(EnumPermissions.EditArticle);
            if (!canUpload)
            {
                throw new InvalidOperationException("Brak uprawnień do dodawania zdjęć!");
            }
            // Walidacja jest powtórzona po stronie serwera, niezależnie od tego, co zrobił komponent.
            var (image, error) = await TryReadImageAsync(file);
            if (image == null)
            {
                throw new InvalidOperationException(error);
            }
            Directory.CreateDirectory(_uploadPath);
            // Nazwę i rozszerzenie wybiera serwer na podstawie zawartości, a nie nazwy od klienta.
            string fileName = $"{Guid.NewGuid()}{image.Extension}";
            string fullPath = Path.Combine(_uploadPath, fileName);
            await File.WriteAllBytesAsync(fullPath, image.Bytes);
            try
            {
                var id = await _fileRepository.GetLastFileId();
                var mediaFile = new MediaFile
                {
                    Id = id,
                    FileName = fileName,
                    FileType = image.Extension
                };
                await _fileRepository.AddFileAsync(mediaFile);
                return id;
            }
            catch
            {
                // Bez wpisu w bazie plik na dysku byłby osierocony.
                File.Delete(fullPath);
                throw;
            }
        }
        public async Task<string> GetFileNameByIdAsync(int id)
        {
            // Komponenty budują z tej nazwy adres /Images/{nazwa}.
            var file = await _fileRepository.GetFileByIdAsync(id);
            return file.FileName;
        }

        /// <summary>Odczytuje plik do pamięci i potwierdza po sygnaturze, że to JPG, PNG lub WEBP.</summary>
        private async Task<(ValidatedImage? Image, string? Error)> TryReadImageAsync(IBrowserFile file)
        {
            if (file.Size <= 0)
            {
                return (null, "Wybrany plik jest pusty.");
            }
            if (file.Size > MaxImageSize)
            {
                return (null, "Zdjęcie jest za duże (maksymalnie 5 MB).");
            }
            try
            {
                await using var stream = file.OpenReadStream(MaxImageSize);
                using var memory = new MemoryStream();
                await stream.CopyToAsync(memory);
                var bytes = memory.ToArray();
                var type = DetectImageType(bytes);
                if (type == null)
                {
                    return (null, "Dozwolone są tylko zdjęcia w formacie JPG, PNG lub WEBP.");
                }
                return (new ValidatedImage(bytes, type.Value.Extension, type.Value.MimeType), null);
            }
            catch (IOException)
            {
                return (null, "Nie udało się odczytać zdjęcia (maksymalnie 5 MB).");
            }
        }

        /// <summary>Rozpoznaje format po pierwszych bajtach pliku, a nie po nazwie czy typie od klienta.</summary>
        private static (string Extension, string MimeType)? DetectImageType(byte[] b)
        {
            // JPEG: FF D8 FF
            if (b.Length >= 3 && b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF)
            {
                return (".jpg", "image/jpeg");
            }
            // PNG: 89 50 4E 47 0D 0A 1A 0A
            if (b.Length >= 8 && b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47
                && b[4] == 0x0D && b[5] == 0x0A && b[6] == 0x1A && b[7] == 0x0A)
            {
                return (".png", "image/png");
            }
            // WEBP: "RIFF" .... "WEBP"
            if (b.Length >= 12 && b[0] == 'R' && b[1] == 'I' && b[2] == 'F' && b[3] == 'F'
                && b[8] == 'W' && b[9] == 'E' && b[10] == 'B' && b[11] == 'P')
            {
                return (".webp", "image/webp");
            }
            return null;
        }
    }
}