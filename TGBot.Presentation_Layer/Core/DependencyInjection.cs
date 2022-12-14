using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Linq;

using RestSharp;

using Serilog;

using Telegram.Bot;

using TGBot.Business_Layer.Core.Services;

namespace TGBot.Core;

internal class DependencyInjection
{
    private const string JsonFileName = "appsettings.json";
    private static readonly Lazy<DependencyInjection> Instance = new(static () => new DependencyInjection());
    private readonly IServiceProvider _serviceProvider;
    private DependencyInjection()
    {
        ServiceCollection services = new();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }
    public static DependencyInjection Shared => Instance.Value;

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<RestClient>(static _ => CreateRestClient());
        services.AddScoped<ILightCheckService, LightCheckService>();
        services.AddLogging(static builder => builder.AddSerilog(CreateSerilogLogger()));
        services.AddScoped<ITelegramBotClient, TelegramBotClient>(static _ => CreateTelegramBotClient());
    }


    private static ILogger CreateSerilogLogger()
    {
        return new LoggerConfiguration()
            .WriteTo.File("log.json")
            .CreateLogger();
    }

    private static RestClient CreateRestClient()
    {
        RestClient client = new("https://poweralertua.azurewebsites.net");
        client.AddDefaultHeader("Accept", "application/json");
        client.AddDefaultHeader("Content-Type", "application/json");
        return client;
    }

    private static TelegramBotClient CreateTelegramBotClient()
    {
        string jsonText = File.ReadAllText(JsonFileName);
        JObject? jsonObject = JObject.Parse(jsonText);
        string? token = jsonObject["TelegramBotToken"].ToString();
        return new TelegramBotClient(token);
    }

    /// <summary>
    ///     Get service from DI container
    /// </summary>
    /// <typeparam name="T">
    ///     Type of service
    /// </typeparam>
    /// <returns>
    ///     Service of <typeparamref name="T" /> type
    /// </returns>
    public T GetService<T>()
    {
        return _serviceProvider.GetService<T>();
    }
}



