using System.ComponentModel.DataAnnotations;
using SchoolNewspaperBlazorApp.Data.Enums;

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
        /// <summary>Identyfikator opcjonalnego zdjęcia okładkowego pokazywanego na kartach artykułów.</summary>
        public int? CoverFileId { get; set; }
        /// <summary>Hasztagi artykułu zapisane jako lista małych liter oddzielonych przecinkami, bez znaku #.</summary>
        public string Tags { get; set; } = string.Empty;
        /// <summary>Data i godzina wydarzenia. Jeśli jest ustawiona, artykuł trafia do zakładki Wydarzenia.</summary>
        public DateTime? EventDate { get; set; }
        /// <summary>Rodzaj artykułu: aktualność albo wydarzenie.</summary>
        public EnumArticleType ArticleType { get; set; } = EnumArticleType.News;

        /// <summary>Czy artykuł jest wydarzeniem (właściwość tylko do odczytu, nie trafia do bazy).</summary>
        public bool IsEvent => ArticleType == EnumArticleType.Event;
        /// <summary>Nazwa rodzaju artykułu do wyświetlania użytkownikowi.</summary>
        public string TypeLabel => IsEvent ? "Wydarzenie" : "Aktualność";

        /// <summary>Zwraca hasztagi jako listę gotową do wyświetlenia.</summary>
        public IEnumerable<string> GetTagList()
        {
            return Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

    }


}