namespace SchoolNewspaperBlazorApp.Data
{
    /// <summary>Przechowuje metadane pliku używanego przez aplikację.</summary>
    public class MediaFile
    {
        /// <summary>Identyfikator pliku, używany także jako klucz artykułu.</summary>
        public int Id { get; set; }
        /// <summary>Typ pliku, na przykład rozszerzenie lub MIME type.</summary>
        public required string FileType { get; set; }
        /// <summary>Nazwa pliku zapisana na serwerze.</summary>
        public required string FileName { get; set; }

    }
}
