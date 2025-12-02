using Microsoft.Extensions.Logging;

namespace CetTodoApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			// ... diğer ayarlar

            // *** Burası ÖNEMLİ: Veritabanı Servisini DI'ya ekleme ***
            .Services.AddSingleton<TodoDatabase>(s =>
            {
                // Veritabanı dosya adını belirle
                var dbName = "TodoSQLite.db3"; 
                // Platforma özel kalıcı uygulama veri dizinini al
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, dbName); 
                
                // TodoDatabase örneğini oluştur ve DI'ya kaydet
                return new TodoDatabase(dbPath);
            });
            // --------------------------------------------------------

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
namespace CetTodoApp;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    
}
