using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SchoolNewspaperBlazorApp.Components;
using SchoolNewspaperBlazorApp.Data;
using SchoolNewspaperBlazorApp.Data.Enums;
using SchoolNewspaperBlazorApp.Interfaces.Repository;
using SchoolNewspaperBlazorApp.Interfaces.Service;
using SchoolNewspaperBlazorApp.Repository;
using SchoolNewspaperBlazorApp.Service;
using System.Security.Claims;

namespace SchoolNewspaperBlazorApp
{
    /// <summary>Punkt wejścia oraz konfiguracja usług i potoku HTTP aplikacji.</summary>
    public class Program
    {
        /// <summary>Buduje host Blazor Server i uruchamia aplikację webową.</summary>
        public static void Main(string[] args)
        {
            // Builder ładuje konfigurację, środowisko uruchomieniowe i kontener DI.
            var builder = WebApplication.CreateBuilder(args);
            // Rejestrujemy komponenty Razor oraz obsługę interakcji po stronie serwera.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            // Fabryka kontekstów zapewnia niezależny NewspaperDbContext dla operacji.
            builder.Services.AddDbContextFactory<NewspaperDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("NewspaperConnectionString")));

            // Repozytoria izolują zapytania do bazy od logiki biznesowej serwisów.
            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IFileRepository, FileRepository>();
            builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

            // Serwisy zawierają reguły biznesowe i są wstrzykiwane do komponentów.
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // Ciasteczko HttpOnly i Secure ogranicza możliwość jego odczytu przez skrypty.
                    options.LoginPath = "/login";
                    options.Cookie.Name = "SchoolNewspaper.Auth";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.SlidingExpiration = true;
                });
            builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();

            var app = builder.Build();

            // Konfigurujemy potok żądań zależnie od środowiska uruchomieniowego.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
               
                app.UseHsts();
            }
            app.MapPost("/auth/login", async (
                HttpContext context,
                IAuthService authService
                ) =>
            {
                // Endpoint obsługuje klasyczny formularz HTML strony logowania.
                var form = await context.Request.ReadFormAsync();
                string email = form["Email"].ToString();
                string password = form["Password"].ToString();
                int userId = await authService.CheckIfRegisteredAsync(email, password);
                if (userId == 0)
                {
                    // Błędne dane wracają do strony logowania z kodem komunikatu.
                    return Results.Redirect("/login?notification=loginError");
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
                    new Claim(ClaimTypes.Name,email)
                };
                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                // Claims są później używane przez PermissionService do sprawdzania dostępu.
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);
                return Results.Redirect("/adminPanel?notification=login");
            });
            var imagesPath = Path.Combine(
                builder.Environment.ContentRootPath,
                "Images"
            );
            // Folder zdjęć musi istnieć przed obsługą przesyłania i plików statycznych.
            Directory.CreateDirectory(imagesPath);
            // Udostępniamy zdjęcia z folderu Images pod adresem /Images.
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(imagesPath),
                RequestPath = "/Images"
            });

            // Wymuszamy bezpieczne połączenie HTTPS.
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            // Mapujemy główny komponent aplikacji i włączamy renderowanie interaktywne.
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
