using SchoolNewspaperBlazorApp.Data.Enums;

namespace SchoolNewspaperBlazorApp.Data
{
    /// <summary>Wiadomość przekazywana z logiki aplikacji do popupu powiadomień.</summary>
    public class NotificationMessage
    {
        /// <summary>Tekst prezentowany użytkownikowi.</summary>
        public string? Message { get; set; }
        /// <summary>Rodzaj komunikatu decydujący o jego znaczeniu i wyglądzie.</summary>
        public EnumNotificationType? Type { get; set; }
    }
}
