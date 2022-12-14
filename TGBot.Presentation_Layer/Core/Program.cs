using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using Telegram.Bot;
using Telegram.Bot.Types;

using TGBot.Business_Layer.Core.Services;
using TGBot.Data.DTO;

using File = System.IO.File;

namespace TGBot.Core;

internal class Program
{
    private const string JsonFileName = "appsettings.json";
    private static ILogger _logger;

    private static void Main(string[] args)
    {
        _logger = DependencyInjection.Shared.GetService<ILogger<Program>>();
        ITelegramBotClient bot = DependencyInjection.Shared.GetService<ITelegramBotClient>();
        bot.StartReceiving(MessageHandler, ErrorHandler);
        Console.WriteLine("Bot started");
        Console.ReadLine();
    }

    private static async Task MessageHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        string? command = update.Message.Text;
        // Check if the command is a command
        if (!command.StartsWith("/"))
        {
            await client.SendTextMessageAsync(update.Message.Chat.Id, "This is not a command", cancellationToken: cancellationToken);
            return;
        }

        // Check if the command is a "/refresh" command
        if (command.StartsWith("/refresh"))
        {
            await client.SendTextMessageAsync(update.Message.Chat.Id, "Refresh requested", cancellationToken: cancellationToken);
            ILightCheckService service = DependencyInjection.Shared.GetService<ILightCheckService>();
            string? code = JObject.Parse(File.ReadAllText(JsonFileName))["LightCode"].ToString();
            Report? result = await service.GetReportAsync(code);
            await client.SendTextMessageAsync(update.Message.Chat.Id, result?.ToString() ?? "Result is bad", cancellationToken: cancellationToken);

            _logger.LogInformation("New info requested");
            _logger.LogInformation(result?.ToString() ?? "Error while updating");
        }
    }

    private static Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Error while receiving message");
        return Task.CompletedTask;
    }
}




