using Microsoft.AspNetCore.Components.Forms;
using SchoolNewspaperBlazorApp.Data;

namespace SchoolNewspaperBlazorApp.Interfaces.Service
{
    /// <summary>Udostępnia obsługę zdjęć przesyłanych przez formularze Blazor.</summary>
    public interface IFileService
    {
        /// <summary>Sprawdza, czy plik jest dozwolonym zdjęciem. Zwraca komunikat błędu albo null, gdy plik jest poprawny.</summary>
        Task<string?> ValidateImageAsync(IBrowserFile file);
        /// <summary>Tworzy podgląd wybranego zdjęcia w przeglądarce (pusty tekst, gdy plik jest niepoprawny).</summary>
        Task<string> GetPreviewAsync(IBrowserFile file);
        /// <summary>Waliduje i zapisuje zdjęcie, a następnie zwraca jego identyfikator. Przy błędzie rzuca InvalidOperationException z komunikatem.</summary>
        Task<int> UploadImage(IBrowserFile file);
        /// <summary>Pobiera nazwę pliku na podstawie identyfikatora.</summary>
        Task<string> GetFileNameByIdAsync(int id);
    }
}