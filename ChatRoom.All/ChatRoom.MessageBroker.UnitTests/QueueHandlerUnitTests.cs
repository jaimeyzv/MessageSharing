using ChatRoom.Common;
using ChatRoom.Common.QueueChatRoom;
using ChatRoom.MessageBroker.Interfaces;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using System;
using System.Text;

namespace ChatRoom.MessageBroker.UnitTests
{
    [TestFixture]
    public class QueueHandlerUnitTests
    {
        private Mock<IConnectionFactory> connectionFactoryMock;
        private Mock<IConnection> connectionMock;
        private Mock<IModel> modelMock;
        private IQueueHandler queueHandler;

        [SetUp]
        public void SetUp()
        {
            connectionFactoryMock = new Mock<IConnectionFactory>();
            connectionMock = new Mock<IConnection>();
            modelMock = new Mock<IModel>();
            queueHandler = new QueueHandler();
        }

        [Test]
        public void SendMessage_WhenMessageIsValid_RunSuccess()
        {
            var queueName = "RbTest";
            var queueMessage = new QueueMessage() { ChatroomId = 1, Date = DateTime.Now, Menssge = "This is a test", UserSender = "jaimeyzv" };
            var message = JsonSerializerHelper.JsonConvert(queueMessage);
            var body = Encoding.UTF8.GetBytes(message);
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);
            connectionMock.Setup(x => x.CreateModel()).Returns(modelMock.Object);
            modelMock.Setup(x => x.BasicPublish(It.IsAny<string>(), queueName, null, body));
            
            queueHandler.SendMessage(queueName, queueMessage);
            
            modelMock.Verify(x => x.BasicPublish(It.IsAny<string>(), queueName, null, body), Times.Once);
        }

        [TestCase("")]
        [TestCase(null)]
        public void SendMessage_WhenQueueNameIsInvalid_ThrowsException(string queueName)
        {
            Assert.Throws<Exception>(() => queueHandler.SendMessage(queueName, It.IsAny<QueueMessage>()));
        }

        [TestCase("")]
        [TestCase(null)]
        public void SendMessage_WhenQueueNameIsInvalid_ThrowsExceptionAndConnectionAndChannelAreNeverMeet(string queueName)
        {
            Assert.Throws<Exception>(() => queueHandler.SendMessage(queueName, It.IsAny<QueueMessage>()));
            connectionFactoryMock.Verify(x => x.CreateConnection(), Times.Never);
            connectionMock.Verify(x => x.CreateModel(), Times.Never);
        }

        [TestCase("Queuename1")]
        [TestCase("Queuename2")]
        public void SendMessage_WhenQueueMessageIsNull_ThrowsArgumentNullException(string queueName)
        {
            QueueMessage queueMessage = null;
            Assert.Throws<ArgumentNullException>(() => queueHandler.SendMessage(queueName, queueMessage));
        }

        [TestCase("Queuename1")]
        [TestCase("Queuename2")]
        public void SendMessage_WhenQueueMessageIsNull_ThrowsExceptionAndConnectionAndChannelAreNeverMeet(string queueName)
        {
            Assert.Throws<ArgumentNullException>(() => queueHandler.SendMessage(queueName, It.IsAny<QueueMessage>()));
            connectionFactoryMock.Verify(x => x.CreateConnection(), Times.Never);
            connectionMock.Verify(x => x.CreateModel(), Times.Never);
        }
    }
}