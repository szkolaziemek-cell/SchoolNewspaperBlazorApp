using SchoolNewspaperBlazorApp.Data;
using SchoolNewspaperBlazorApp.Data.Enums;

namespace SchoolNewspaperBlazorApp.Interfaces.Service
{
    /// <summary>Definiuje kolejkę komunikatów prezentowanych w interfejsie.</summary>
    public interface INotificationService
    {
        /// <summary>Dodaje komunikat do kolejki i odświeża popup.</summary>
        void Show(string message, EnumNotificationType type);
        /// <summary>Pobiera następny komunikat z kolejki, jeśli istnieje.</summary>
        bool TryDequeue(out NotificationMessage? message);
        /// <summary>Informuje komponenty, że do kolejki dodano komunikat.</summary>
        event Action? OnChange;
    }
}
