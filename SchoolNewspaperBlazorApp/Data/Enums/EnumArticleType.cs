namespace SchoolNewspaperBlazorApp.Data.Enums
{
    /// <summary>Określa, czy artykuł jest zwykłą aktualnością, czy wydarzeniem z datą.</summary>
    public enum EnumArticleType
    {
        /// <summary>Zwykła aktualność z życia szkoły.</summary>
        News,
        /// <summary>Wydarzenie, które ma datę i godzinę.</summary>
        Event
    }
}