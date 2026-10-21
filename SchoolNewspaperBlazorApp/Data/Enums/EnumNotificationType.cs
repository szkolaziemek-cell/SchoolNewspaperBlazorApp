namespace SchoolNewspaperBlazorApp.Data.Enums
{
    /// <summary>Określa rezultat operacji prezentowany w powiadomieniu.</summary>
    public enum EnumNotificationType
    {
        /// <summary>Operacja zakończyła się prawidłowo.</summary>
        Success,
        /// <summary>Użytkownik nie ma wymaganego uprawnienia.</summary>
        Failure,
        /// <summary>Wystąpił nieoczekiwany błąd aplikacji.</summary>
        Error
    }
}
