using Telegram.Bot;
using Telegram.Bot.Types;

Dictionary<long, string> IDs = new Dictionary<long, string>();
long lastId;

Random random = new Random();
long companionId = 0;

Dictionary<long, bool> companionIsChanged = new Dictionary<long, bool>();


var client = new TelegramBotClient("5505267533:AAFSPmp71MjJZLKR4YXyvxxlBc_OCZ3V4EQ");

client.StartReceiving(Update, Error);
Console.ReadKey();

async static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
{
    throw new ArgumentException("Error");
}

async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    var message = update.Message;
    if (message != null)
    {
        companionIsChanged[message.Chat.Id] = false;
        foreach (var key in IDs.Keys)
        {
            if (key == message.Chat.Id)
            {
                companionIsChanged[message.Chat.Id] = true;
            }
        }
        IDs[message.Chat.Id] = message.Chat.FirstName;

        if (!companionIsChanged[message.Chat.Id])
        {
            companionId = message.Chat.Id;
        }

        if (message.Text != null)
        {
            if (message.Text.ToLower().Contains("change companion"))
            {
                lastId = companionId;
                if (ChangeCompanion(message.Chat.Id))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Companion changed!" + " Now you are talking to " + IDs[companionId]);
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Not enough companions");
                }
                companionIsChanged[message.Chat.Id] = true;
            }
            else if (message.Text.ToLower().Contains("who is my companion"))
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Now you are talking to " + IDs[companionId]);
            }
            else if (message.Text.ToLower().Contains("list of companions"))
            {
                foreach (var name in IDs.Values)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, name);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(companionId, message.Text);
            }
            Console.WriteLine(IDs[message.Chat.Id] + " " + message.Text + " " + IDs[companionId]);
        }

        if (message.Sticker != null)
        {
            await botClient.SendStickerAsync(companionId, message.Sticker.FileId);
        }
    }
}
bool ChangeCompanion(long id)
{
    while (companionId == lastId)
    {
        if (IDs.Count > 1)
        {
            companionId = IDs.Keys.ToList()[random.Next(0, IDs.Count)];
            return true;
        }
        else
            break;
    }
    return false;
}