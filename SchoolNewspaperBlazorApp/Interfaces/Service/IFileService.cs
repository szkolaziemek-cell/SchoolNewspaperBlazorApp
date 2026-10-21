using Microsoft.AspNetCore.Components.Forms;
using SchoolNewspaperBlazorApp.Data;

namespace SchoolNewspaperBlazorApp.Interfaces.Service
{
    /// <summary>Udostępnia obsługę zdjęć przesyłanych przez formularze Blazor.</summary>
    public interface IFileService
    {
        /// <summary>Tworzy podgląd wybranego pliku w przeglądarce.</summary>
        Task<string> GetPreviewAsync(IBrowserFile file);
        /// <summary>Przesyła zdjęcie i zwraca jego identyfikator.</summary>
        Task<int> UploadImage(IBrowserFile file);
        /// <summary>Pobiera nazwę pliku na podstawie identyfikatora.</summary>
        Task<string> GetFileNameByIdAsync(int id);
    }
}
