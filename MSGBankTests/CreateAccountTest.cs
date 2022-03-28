using Microsoft.AspNetCore.Mvc;
using Moq;
using MSGBank.Data;
using MSGBank.Manager;
using NSubstitute;
using NUnit.Framework;
using System;

namespace MSGBankTests
{
    public class CreateAccountTest
    {
        private Mock<IDatabase> MockDatabase;
        private IBankManager BankManager;
        private User User;
        private Customer Customer;
        [SetUp]
        public void Setup()
        {
            MockDatabase = new Mock<IDatabase>();
            BankManager = new BankManager(MockDatabase.Object);
            User = new User(1, "TestUser");
            Customer = new Customer(1, "TestCustomer");
        }

        [Test]
        public void CustomerDoesNotExist()
        {
            MockDatabase.Setup(x => x.FindCustomer(It.IsAny<string>())).Returns((Customer)null);

            var actionResult = BankManager.CreateAccount("TestUser", 10 , User);
            var baderequest = actionResult as BadRequestObjectResult;
            Assert.IsTrue(actionResult is BadRequestObjectResult);
            Assert.IsTrue(baderequest.Value.Equals($"Customer TestUser not found"));
        }

        [Test]
        public void DepositNegativ()
        {
            MockDatabase.Setup(x => x.FindCustomer(It.IsAny<string>())).Returns(Customer);

            var actionResult = BankManager.CreateAccount("TestUser", -10, User);
            var baderequest = actionResult as BadRequestObjectResult;
            Assert.IsTrue(actionResult is BadRequestObjectResult);
            Assert.IsTrue(baderequest.Value.Equals($"Doposit must be positiv. Deposit: {-10}"));
        }
        [Test]
        public void UserNull()
        {
            MockDatabase.Setup(x => x.FindCustomer(It.IsAny<string>())).Returns(Customer);

            var actionResult = BankManager.CreateAccount("TestUser", 10, null);
            var baderequest = actionResult as BadRequestObjectResult;
            Assert.IsTrue(actionResult is BadRequestObjectResult);
            Assert.IsTrue(baderequest.Value.Equals($"User is null"));
        }
        [Test]
        public void Ok()
        {
            MockDatabase.Setup(x => x.FindCustomer(It.IsAny<string>())).Returns(Customer);

            var actionResult = BankManager.CreateAccount("TestUser", 10, User);
            var baderequest = actionResult as BadRequestObjectResult;
            Assert.IsTrue(actionResult is OkResult);
        }
    }
}