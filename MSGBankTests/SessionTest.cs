using Moq;
using MSGBank.Data;
using MSGBank.Manager;
using NSubstitute;
using NUnit.Framework;
using System;

namespace MSGBankTests
{
    public class SessionTest
    {
        private Mock<IDatabase> MockDatabase;
        private IBankManager BankManager;
        [SetUp]
        public void Setup()
        {
            MockDatabase = new Mock<IDatabase>();
            BankManager = new BankManager(MockDatabase.Object);
        }

        [Test]
        public void LoginUserDoesNotExist()
        {
            MockDatabase.Setup(x => x.FindUser(It.IsAny<string>())).Returns((User)null);

            var id = BankManager.Login("TestUser");
            Assert.IsTrue(id is null);
        }

        [Test]
        public void LoginUserExists()
        {
            MockDatabase.Setup(x => x.FindUser(It.IsAny<string>())).Returns(new User(1, "TestUser"));

            var id = BankManager.Login("TestUser");
            Assert.IsTrue(id is not null);
        }

        [Test]
        public void MultiLoginUser()
        {
            User testUser = new User(1, "TestUser");
            MockDatabase.Setup(x => x.FindUser(It.IsAny<string>())).Returns(testUser);

            var id1 = BankManager.Login("TestUser");
            var id2 = BankManager.Login("TestUser");

            Assert.IsTrue(id1 is not null);
            Assert.IsTrue(id2 is not null);
            Assert.IsTrue(id1.Equals(id2));
        }

        [Test]
        public void LogoutSessionExists()
        {
            User testUser = new User(1, "TestUser");
            MockDatabase.Setup(x => x.FindUser(It.IsAny<string>())).Returns(testUser);

            Guid? sessionID = BankManager.Login("TestUser");
            bool ok = BankManager.Logout(sessionID.Value);

            Assert.IsTrue(ok);
        }
        [Test]
        public void LogoutSessiondoesNotExists()
        {
            User testUser = new User(1, "TestUser");
            MockDatabase.Setup(x => x.FindUser(It.IsAny<string>())).Returns(testUser);

            bool ok = BankManager.Logout(Guid.NewGuid());

            Assert.IsFalse(ok);
        }
        [Test]
        public void doubleLogout()
        {
            User testUser = new User(1, "TestUser");
            MockDatabase.Setup(x => x.FindUser(It.IsAny<string>())).Returns(testUser);

            Guid? sessionID = BankManager.Login("TestUser");
            bool ok = BankManager.Logout(sessionID.Value);

            Assert.IsTrue(ok);

            ok = BankManager.Logout(sessionID.Value);

            Assert.IsFalse(ok);
        }
    }
}