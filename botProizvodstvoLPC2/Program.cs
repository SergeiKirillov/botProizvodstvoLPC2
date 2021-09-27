using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Data.SqlClient;
using Telegram.Bot.Types;

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
                Console.WriteLine($"пришло сообщение с текстом: {msg.Text}");

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

           // Console.WriteLine("Запрос к БД с данными по производству на сегодня ");
           // Console.WriteLine("Содержит ссылки по рулонно(ссылка это сообщения при прокатке)");

            string ConString = "Data Source = 192.168.0.46; Initial Catalog = standb; User ID = readDBstan; Password = 123456";
            string strQuery = "SELECT [dtsave],[RulonName],[StartRulon],[StopRulon],[h5],[b],[ves],[dlina] FROM [standb].[dbo].[stanwork] where dtsave between convert(datetime,convert(varchar,'20210823 0:0:00',20)) and convert(datetime,convert(varchar,'20210823 23:59:59',20))order by dtsave";

            SqlConnection con = new SqlConnection(ConString);
            con.Open();
            SqlCommand cmd = new SqlCommand(strQuery, con);
            SqlDataReader reader = cmd.ExecuteReader();
            int countQuery=0;
            Console.WriteLine($"Начало выдачи данных {DateTime.Now}");
            Console.WriteLine($"по запросу:{strQuery}");    
            while (reader.Read())
            {
                //Console.WriteLine(reader[1].ToString());
                //var ProizvodRulon = await client.SendTextMessageAsync(msg.Chat.Id, reader[1].ToString(), replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Check","https://core.telegram.org/bots/api#sendmessage")));
                var ProizvodRulon = await client.SendTextMessageAsync(msg.Chat.Id, reader[1].ToString()+ " https://t.me/TestingFAQBot?startgroup=test{}");
                countQuery = countQuery + 1;
            }

            
            //Message[] messages = await client.SendMediaGroupAsync(
            //chatId: msg.Chat.Id,
            //media: new IAlbumInputMedia[]
            //{
            //    new InputMediaPhoto("https://cdn.pixabay.com/photo/2017/06/20/19/22/fuchs-2424369_640.jpg"),
            //    new InputMediaPhoto("https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg"),
            //}
            //);

            Console.WriteLine($"Время окончания выводв запрса {DateTime.Now}. Выдано {countQuery.ToString()} записей.");


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
