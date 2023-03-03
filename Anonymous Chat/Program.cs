using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

Dictionary<long, string> IDs = new Dictionary<long, string>();
Dictionary<long, long> companions = new Dictionary<long, long>();
List<InputOnlineFile> onlineFiles;

long lastId;
const string change = "Change companion";
const string list = "List of companions";
Random random = new Random();

var client = new TelegramBotClient("5505267533:AAFSPmp71MjJZLKR4YXyvxxlBc_OCZ3V4EQ");
client.StartReceiving(Update, Error);
Console.ReadKey();


async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    var message = update.Message;

    if (message != null)
    {
        if (!IDs.ContainsKey(message.Chat.Id))
        {
            companions[message.Chat.Id] = message.Chat.Id;
            IDs[message.Chat.Id] = message.Chat.FirstName;
            await botClient.SendTextMessageAsync(message.Chat.Id, "...", replyMarkup: GetButtons());
        }

        if (message.Text != null)
        {
            Console.WriteLine(IDs[message.Chat.Id] + " " + message.Text);

            if (message.Text.ToLower().Contains("change"))
            {
                lastId = companions[message.Chat.Id];
                if (ChangeCompanion(message.Chat.Id))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Companion changed!" + " Now you are talking to " + IDs[companions[message.Chat.Id]]);
                    await botClient.SendTextMessageAsync(companions[message.Chat.Id], "New companion!");
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Not enough companions");
                }
                companions[companions[message.Chat.Id]] = message.Chat.Id;
            }
            else if (message.Text.ToLower().Contains("list"))
            {
                foreach (var name in IDs.Values)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, name);
                }
            }
            else if (!message.Text.ToLower().Contains("/start"))
            {
                await botClient.SendTextMessageAsync(companions[message.Chat.Id], message.Text);
            }
            Console.WriteLine(IDs[message.Chat.Id] + " " + message.Text + " " + IDs[companions[message.Chat.Id]]);
        }

        if (message.Sticker != null)
        {
            await botClient.SendStickerAsync(companions[message.Chat.Id], message.Sticker.FileId);
        }

        if (message.Photo != null)
        {
            await using var stream = File.Opdfte("C:/Users/Данил/Desktop");
            stream.Write(message.Audio);
            await botClient.SendAudioAsync()
        }
    }
}

IReplyMarkup? GetButtons()
{
    return new ReplyKeyboardMarkup
    (
        new List<List<KeyboardButton>>
        {
            new List<KeyboardButton> {new KeyboardButton(change)},
            new List<KeyboardButton> {new KeyboardButton(list)},
        }
    );
}

bool ChangeCompanion(long id)
{
    if (IDs.Count <= 1)
    {
        return false;
    }
    long newId = lastId;
    while (newId == lastId)
    {
        newId = IDs.Keys.ToList()[random.Next(0, IDs.Count)];
    }
    companions[id] = newId;
    return true;
}

async static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
{
    throw new ArgumentException("Error");
}