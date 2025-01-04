using FinLY.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinLY
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

          
            builder.Services.AddScoped<ITagsServices, TagsServices>();
            builder.Services.AddSingleton<AuthenticationStateService>();
            builder.Services.AddSingleton<IUserServices, UserServices>();
            builder.Services.AddSingleton<ITransactionsServices, TransactionsServices>();



#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
