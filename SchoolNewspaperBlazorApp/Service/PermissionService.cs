using Microsoft.AspNetCore.Components.Authorization;
using SchoolNewspaperBlazorApp.Data.Enums;
using SchoolNewspaperBlazorApp.Interfaces.Repository;
using SchoolNewspaperBlazorApp.Interfaces.Service;
using System.Security.Claims;

namespace SchoolNewspaperBlazorApp.Service
{
    /// <summary>Łączy stan uwierzytelnienia z operacjami uprawnień.</summary>
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly AuthenticationStateProvider _asp;
        private readonly INotificationService _notificationService;
        public PermissionService(IPermissionRepository permissionRepository, AuthenticationStateProvider asp, INotificationService notificationService)
        {
            _permissionRepository = permissionRepository;
            _asp = asp;
            _notificationService = notificationService;
        }

        public async Task<List<EnumPermissions>> GetUserPermissionsAsync(int userId)
        {
            // Metoda jest używana podczas otwierania panelu konkretnego użytkownika.
            return await _permissionRepository.GetUserPermissionsAsync(userId);
        }
        public async Task<Dictionary<EnumPermissions,bool>> GetAllPermissionsAsync()
        {
            // Pobieramy uprawnienia aktualnie zalogowanego użytkownika do menu panelu.
            var id = await GetUserId();
            return await _permissionRepository.GetAllUserPermissionsAsync(id);
        }
        public async Task<bool> HasPermissionAsync(EnumPermissions permission)
        {
            // AuthenticationStateProvider dostarcza claims bieżącej sesji.
            var authState = await _asp.GetAuthenticationStateAsync();
            var principal = authState.User;
            return await _permissionRepository.HasPermissionAsync(principal, permission);
        }
        public async Task SavePermissionsAsync(List<EnumPermissions> permissions, int userId)
        {
            bool hasPermission = await HasPermissionAsync(EnumPermissions.Master);
            if (!hasPermission)
            {
                _notificationService.Show("Brak uprawnień do nadawania uprawnień!", EnumNotificationType.Failure);
                return;
            }

            try
            {
                // Uprawnienia są zapisywane jako jeden zestaw dla wybranego użytkownika.
                await _permissionRepository.SavePermissionsAsync(permissions, userId);
                _notificationService.Show("Pomyślnie nadano uprawnienia użytkownikowi!", EnumNotificationType.Success);
            }
            catch (Exception)
            {
                _notificationService.Show("Podczas nadawania uprawnień wystąpił błąd!", EnumNotificationType.Error);
            }
        }
        public async Task<int> GetUserId()
        {
            // Claims są tekstowe, dlatego identyfikator trzeba bezpiecznie sparsować.
            var authState = await _asp.GetAuthenticationStateAsync();
            var principal = authState.User;
            var stringId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(stringId, out int id))
            {
                return id;
            }

            return 0;
        }

    }
}
