using Telegram.Bot;
using Telegram.Bot.Types;

Dictionary<long, string> IDs = new Dictionary<long, string>();
IDs[940765457] = "Danil";
IDs[2015829977] = "Валентина";

List<long> usedIDs = new List<long>();

Random random = new Random();
long companionId = 0;

Dictionary<long, bool> companionIsChanged = new Dictionary<long, bool>();
companionIsChanged[940765457] = false;
companionIsChanged[2015829977] = false;

var client = new TelegramBotClient();

client.StartReceiving(Update, Error);
Console.ReadKey();

async static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
{
    throw new NotImplementedException();
}

async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    var message = update.Message;
    if (!companionIsChanged[message.Chat.Id])
    {
        companionId = message.Chat.Id;
    }

    if (message.Text != null)
    {
        if (message.Text == "change companion")
        {
            usedIDs.Add(companionId);
            ChangeCompanion(message.Chat.Id);
            await botClient.SendTextMessageAsync(message.Chat.Id, "Companion changed!" + " Now you are talking to " + IDs[companionId]);
            companionIsChanged[message.Chat.Id] = true;
        }
        else if (message.Text == "who is my companion")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Now you are talking to " + IDs[companionId]);
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
void ChangeCompanion(long id)
{
    while (companionId == usedIDs[usedIDs.Count - 1])
    {
        companionId = IDs.Keys.ToList()[random.Next(0, IDs.Count)];
    }
}