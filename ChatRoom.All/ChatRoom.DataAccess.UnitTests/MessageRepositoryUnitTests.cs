using ChatRoom.DataAccess.Dtos;
using ChatRoom.DataAccess.Interfaces;
using ChatRoom.DataAccess.Repositories;
using Moq;
using NPoco;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatRoom.DataAccess.UnitTests
{
    [TestFixture]
    public class MessageRepositoryUnitTests
    {
        private Mock<IDbFactory> dbFactoryMock;
        private Mock<IDatabase> dataBaseMock;
        private IMessageRepository messageRepository;

        private void DatabaseMock()
        {
            dataBaseMock = new Mock<IDatabase>();
            dataBaseMock.VerifyAll();
        }

        private void DbFactoryMock()
        {
            dbFactoryMock = new Mock<IDbFactory>();
            dbFactoryMock.Setup(x => x.GetConnection()).Returns(dataBaseMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            DatabaseMock();
            DbFactoryMock();
            messageRepository = new MessageRepository(dbFactoryMock.Object);
        }

        [Test]
        public void Insert_WhenMessageIsValid_ReturnsSuccess()
        {
            var success = 1;
            var message = GetFakeMessages()[0];
            dataBaseMock.Setup(x => x.Insert<MessageDto>(message)).Returns(success);

            var result = messageRepository.Insert(message);

            Assert.AreEqual(success, result);
            dataBaseMock.Verify(x => x.Insert(message), Times.Once);
        }

        [Test]
        public void Insert_WhenMessageIsNull_ReturnsArgumentNullException()
        {
            MessageDto message = null;
            Assert.Throws<ArgumentNullException>(() => messageRepository.Insert(message));
            dataBaseMock.Verify(x => x.Insert(message), Times.Never);
        }

        [Test]
        public void Update_WhenMessageIsValid_ReturnsSuccess()
        {
            var success = 1;
            var message = GetFakeMessages()[0];
            dataBaseMock.Setup(x => x.Update(message)).Returns(success);

            var result = messageRepository.Update(message);

            Assert.AreEqual(success, result);
            dataBaseMock.Verify(x => x.Update(message), Times.Once);
        }

        [Test]
        public void Update_WhenMessageIsNull_ReturnsArgumentNullException()
        {
            MessageDto message = null;
            Assert.Throws<ArgumentNullException>(() => messageRepository.Update(message));
            dataBaseMock.Verify(x => x.Update(message), Times.Never);
        }

        [Test]
        public void Delete_WhenMessageIsValid_ReturnsSuccess()
        {
            var success = 1;
            var pokemon = GetFakeMessages()[0];
            dataBaseMock.Setup(x => x.Delete(pokemon)).Returns(success);

            var result = messageRepository.Delete(pokemon);

            Assert.AreEqual(success, result);
            dataBaseMock.Verify(x => x.Delete(pokemon), Times.Once);
        }

        [Test]
        public void Delete_WhenMessageIsNull_ReturnsArgumentNullException()
        {
            MessageDto message = null;
            Assert.Throws<ArgumentNullException>(() => messageRepository.Delete(message));
            dataBaseMock.Verify(x => x.Delete(message), Times.Never);
        }

        [Test]
        public void GetAll_WhenThereIsData_ReturnsMessageList()
        {
            var messages = GetFakeMessages();
            var query = @"SELECT * FROM [dbo].[Message]";
            dataBaseMock.Setup(x => x.Fetch<MessageDto>(query)).Returns(messages);

            var result = messageRepository.GetAll().ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual(messages.Count, result.Count);
            dataBaseMock.Verify(x => x.Fetch<MessageDto>(query), Times.Once);
        }

        [Test]
        public void GetAll_WhenThereIsNoData_ReturnsEmptyMessageList()
        {
            var messages = new List<MessageDto>();
            var query = @"SELECT * FROM [dbo].[Message]";
            dataBaseMock.Setup(x => x.Fetch<MessageDto>(query)).Returns(messages);

            var result = messageRepository.GetAll().ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 0);
            Assert.AreEqual(messages.Count, result.Count);
            dataBaseMock.Verify(x => x.Fetch<MessageDto>(query), Times.Once);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void GetById_WhenIdIsValid_ReturnsMessage(int id)
        {
            var query = @"SELECT * FROM [dbo].[Message] WHERE MessageId = @0";
            var message = GetFakeMessages()[0];
            dataBaseMock.Setup(x => x.SingleOrDefault<MessageDto>(query, id)).Returns(message);

            var result = messageRepository.GetById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(message.MessageId, result.MessageId);
            Assert.AreEqual(message.Content, result.Content);
            Assert.AreEqual(message.ChatroomId, result.ChatroomId);
            Assert.AreEqual(message.UserSender, result.UserSender);
            Assert.AreEqual(message.IsBot, result.IsBot);
            dataBaseMock.Verify(x => x.SingleOrDefault<MessageDto>(query, id), Times.Once);
        }

        [TestCase(0)]
        [TestCase(-4)]
        [TestCase(-10)]
        [TestCase(-100)]
        [TestCase(null)]
        public void GetById_WhenIdIsNotValid_ReturnsNull(int id)
        {
            var query = @"SELECT * FROM [dbo].[Message] WHERE MessageId = @0";
            MessageDto message = null;
            dataBaseMock.Setup(x => x.SingleOrDefault<MessageDto>(query, id)).Returns(message);

            var result = messageRepository.GetById(id);

            Assert.IsNull(result);
            dataBaseMock.Verify(x => x.SingleOrDefault<MessageDto>(query, id), Times.Once);
        }
        
        private List<MessageDto> GetFakeMessages()
        {
            var messages = new List<MessageDto>();
            messages.Add(new MessageDto { Content = "I would like to know where is the admin.", ChatroomId = 1, Date = DateTime.Now, UserSender = "jaimeyzv", IsBot = false });
            messages.Add(new MessageDto { Content = "I would like to know where is the admin.", ChatroomId = 2, Date = DateTime.Now, UserSender = "johao", IsBot = false });
            messages.Add(new MessageDto { Content = "I would like to know where is the admin.", ChatroomId = 3, Date = DateTime.Now, UserSender = "uriel4001", IsBot = false });

            return messages;
        }
    }
}