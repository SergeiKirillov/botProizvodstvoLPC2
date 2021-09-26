using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace botProizvodstvoLPC2
{
    class Program
    {
        private static string token { get; set; } = "2014272170:AAHDzsJOmmD0v4yWkAz9ttZC7JTzctKKBZ4";
        private static TelegramBotClient client;

        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            var me = client.GetMeAsync().Result;
            Console.WriteLine($"Username {me.FirstName}({me.Username}) c ID {me.Id}");


             
            client.StartReceiving();
            client.OnMessage += onMessageHandler;

             Console.ReadLine();
            client.StopReceiving();
        }

        private static async void onMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg != null)
            {
                //Console.WriteLine($"пришло сообщение с текстом: {msg.Text}");

                switch (msg.Text)
                {
                    case "/start":
                        var start = await client.SendTextMessageAsync(msg.Chat.Id, msg.Text, replyMarkup: GetButtons());
                        break;
                    case "Производство за":
                        GetDataProizvodstvo(e);
                        break;
                    default:
                        break;
                }
            };
        }

        private static async void GetDataProizvodstvo(MessageEventArgs e)
        {
            var msg = e.Message;
            string txtRulon = "";

            Console.WriteLine("Запрос к БД с данными по производству на сегодня ");
            Console.WriteLine("Содержит ссылки по рулонно(ссылка это сообщения при прокатке)");

            txtRulon = "1,2,3,4,5,";

            var ProizvodRulon = await client.SendTextMessageAsync(msg.Chat.Id, txtRulon, replyMarkup: returnMenu());
        }

        private static IReplyMarkup returnMenu()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
               {
                   new List<KeyboardButton>{ new KeyboardButton {Text="/start"}},
               }
            };
        }

        private static IReplyMarkup GetButtons() 
        {
            return new ReplyKeyboardMarkup
            {
               Keyboard = new List<List<KeyboardButton>>
               {
                   new List<KeyboardButton>{ new KeyboardButton {Text="Производство за"}},
               }
            };
            
        }
    }
}
