using ChatRoom.Business.Entities;
using ChatRoom.Business.Interfaces;
using ChatRoom.Common.QueueChatRoom;
using ChatRoom.Injector;
using ChatRoom.MessageBroker.Processor.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ChatRoom.MessageBroker.Processor
{
    public class CsvExternalApi : IExternalApiHandler
    {
        private readonly IMessageBusiness messageBusiness;
        
        public CsvExternalApi()
        {
            this.messageBusiness = SvcLocator.GetInstance<IMessageBusiness>();
        }

        public void SendMessageFromRemoteResource(QueueMessage queueMessage)
        {
            List<string> splitted = new List<string>();
            string data = GetCSV("https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv");
            var price = data.Split(',')[10];
            var content = $"APPL quote is ${price} per share.";
            var message = new MessageEntity()
            {
                Content = content,
                ChatroomId = queueMessage.ChatroomId,
                UserSender = queueMessage.UserSender,
                IsBot = true,
                Date = DateTime.Now
            };

            messageBusiness.CreateMessageFromWorker(message);
        }

        public static string GetCSV(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sReader = new StreamReader(response.GetResponseStream());
            string results = sReader.ReadToEnd();
            sReader.Close();

            return results;
        }
    }
}