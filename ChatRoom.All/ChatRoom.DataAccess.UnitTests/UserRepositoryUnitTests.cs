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
    public class UserRepositoryUnitTests
    {
        private Mock<IDbFactory> dbFactoryMock;
        private Mock<IDatabase> dataBaseMock;
        private IUserRepository userRepository;

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
            userRepository = new UserRepository(dbFactoryMock.Object);
        }

        [Test]
        public void Insert_WhenUserIsValid_ReturnsSuccess()
        {
            var success = 1;
            var user = GetFakeUsers()[0];
            dataBaseMock.Setup(x => x.Insert<UserDto>(user)).Returns(success);

            var result = userRepository.Insert(user);

            Assert.AreEqual(success, result);
            dataBaseMock.Verify(x => x.Insert(user), Times.Once);
        }

        [Test]
        public void Insert_WhenUserIsNull_ReturnsArgumentNullException()
        {
            UserDto user = null;
            Assert.Throws<ArgumentNullException>(() => userRepository.Insert(user));
            dataBaseMock.Verify(x => x.Insert(user), Times.Never);
        }

        [Test]
        public void Update_WhenMessageIsValid_ReturnsSuccess()
        {
            var success = 1;
            var user = GetFakeUsers()[0];
            dataBaseMock.Setup(x => x.Update(user)).Returns(success);

            var result = userRepository.Update(user);

            Assert.AreEqual(success, result);
            dataBaseMock.Verify(x => x.Update(user), Times.Once);
        }

        [Test]
        public void Update_WhenUserIsNull_ReturnsArgumentNullException()
        {
            UserDto user = null;
            Assert.Throws<ArgumentNullException>(() => userRepository.Update(user));
            dataBaseMock.Verify(x => x.Update(user), Times.Never);
        }

        [Test]
        public void Delete_WhenUserIsValid_ReturnsSuccess()
        {
            var success = 1;
            var pokemon = GetFakeUsers()[0];
            dataBaseMock.Setup(x => x.Delete(pokemon)).Returns(success);

            var result = userRepository.Delete(pokemon);

            Assert.AreEqual(success, result);
            dataBaseMock.Verify(x => x.Delete(pokemon), Times.Once);
        }

        [Test]
        public void Delete_WhenUserIsNull_ReturnsArgumentNullException()
        {
            UserDto user = null;
            Assert.Throws<ArgumentNullException>(() => userRepository.Delete(user));
            dataBaseMock.Verify(x => x.Delete(user), Times.Never);
        }

        [Test]
        public void GetAll_WhenThereIsData_ReturnsUserList()
        {
            var users = GetFakeUsers();
            var query = @"SELECT * FROM [dbo].[User]";
            dataBaseMock.Setup(x => x.Fetch<UserDto>(query)).Returns(users);

            var result = userRepository.GetAll().ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual(users.Count, result.Count);
            dataBaseMock.Verify(x => x.Fetch<UserDto>(query), Times.Once);
        }

        [Test]
        public void GetAll_WhenThereIsNoData_ReturnsEmptyUsersList()
        {
            var users = new List<UserDto>();
            var query = @"SELECT * FROM [dbo].[User]";
            dataBaseMock.Setup(x => x.Fetch<UserDto>(query)).Returns(users);

            var result = userRepository.GetAll().ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 0);
            Assert.AreEqual(users.Count, result.Count);
            dataBaseMock.Verify(x => x.Fetch<UserDto>(query), Times.Once);
        }

        private List<UserDto> GetFakeUsers()
        {
            var users = new List<UserDto>();
            users.Add(new UserDto { Name = "Jaime", LastName = "Zamora", NickName = "jaimeyzv", ProfileCode = "ADMINISTRATOR" });
            users.Add(new UserDto { Name = "Johao", LastName = "Rosas", NickName = "johao", ProfileCode = "USER" });
            users.Add(new UserDto { Name = "Gustavo", LastName = "Guillen", NickName = "uriel4001", ProfileCode = "USER" });
            return users;
        }
    }
}