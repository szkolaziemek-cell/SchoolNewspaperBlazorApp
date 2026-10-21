using Microsoft.Data.SqlClient;
using SchoolNewspaperBlazorApp.Data;
using SchoolNewspaperBlazorApp.Data.Enums;
using SchoolNewspaperBlazorApp.Interfaces.Service;

namespace SchoolNewspaperBlazorApp.Service
{
    /// <summary>Implementuje kolejkę krótkich komunikatów dla interfejsu użytkownika.</summary>
    public class NotificationService : INotificationService
    {
        // Kolejka pozwala wyświetlać kilka komunikatów po kolei, bez ich utraty.
        private readonly Queue<NotificationMessage> _queue = new();
        /// <summary>Informuje popup, że powinien rozpocząć obsługę kolejki.</summary>
        public event Action? OnChange;
        /// <summary>Dodaje komunikat do kolejki i wywołuje zdarzenie odświeżenia.</summary>
        public void Show(string message, EnumNotificationType type)
        {
            _queue.Enqueue(new NotificationMessage { Message = message, Type = type });
            OnChange?.Invoke();
        }
        /// <summary>Pobiera pierwszy komunikat z kolejki.</summary>
        public bool TryDequeue(out NotificationMessage? message)
        {
            if (_queue.Count > 0)
            {
                message = _queue.Dequeue();
                return true;
            }
            message = null;
            return false;
        }
    }
}
