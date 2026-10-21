using System.ComponentModel.DataAnnotations;

namespace SchoolNewspaperBlazorApp.Data
{
    /// <summary>Reprezentuje artykuł opublikowany w gazetce szkolnej.</summary>
    public class Article
    {
        /// <summary>Unikalny identyfikator artykułu.</summary>
        public int Id { get; set; }
        /// <summary>Tytuł wyświetlany nad treścią artykułu.</summary>
        public required string Title { get; set; }
        /// <summary>Główna treść publikacji.</summary>
        public required string Text { get; set; }
        /// <summary>Imię lub podpis autora artykułu.</summary>
        public required string Author { get; set; }
        /// <summary>Data i godzina opublikowania artykułu.</summary>
        public DateTime PublishDate { get; set; }
        /// <summary>Identyfikator zdjęcia powiązanego z artykułem.</summary>
        public int FileId { get; set; }

    }

    
}
