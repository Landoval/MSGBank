using Microsoft.AspNetCore.Mvc;
using Moq;
using MSGBank.Data;
using MSGBank.Manager;
using NSubstitute;
using NUnit.Framework;
using System;

namespace MSGBankTests
{
    public class TransferTest
    {
        private Mock<IDatabase> MockDatabase;
        private IBankManager BankManager;
        private User User;
        private Customer Customer;

        private Account TargetAccount;
        private Account SourceAccount;

        private Guid TargetGuid;
        private Guid SourceGuid;

        [SetUp]
        public void Setup()
        {
            MockDatabase = new Mock<IDatabase>();
            BankManager = new BankManager(MockDatabase.Object);
            User = new User(1, "TestUser");

            Customer = new Customer(1, "TestCustomer1");
            Customer Customer2 = new Customer(2, "TestCustomer2");

            TargetGuid = new Guid("00000000-0000-0000-0000-000000000001");
            TargetAccount = new Account(1000, Customer);

            SourceGuid = new Guid("00000000-0000-0000-0000-000000000002");
            SourceAccount = new Account(2000, Customer);

            MockDatabase.Setup(x => x.FindAccount(TargetGuid)).Returns(TargetAccount);
            MockDatabase.Setup(x => x.FindAccount(SourceGuid)).Returns(SourceAccount);
        }
        [Test]
        public void Ok()
        {
            double amount = 500;

            var actionResult = BankManager.Transfer(SourceGuid, TargetGuid, amount, User);

            Assert.IsTrue(actionResult is OkResult);
            Assert.IsTrue(TargetAccount.Balance == 1500);
            Assert.IsTrue(SourceAccount.Balance == 1500);
        }

        [Test]
        public void SourceEqualsTargetAccount()
        {
            double amount = 500;

            var actionResult = BankManager.Transfer(TargetGuid, TargetGuid, amount, User);

            var baderequest = actionResult as BadRequestObjectResult;

            Assert.IsTrue(actionResult is BadRequestObjectResult);
            Assert.IsTrue(baderequest.Value.Equals($"Source and Target are equal"));
        }

        [Test]
        public void TargetInvalid()
        {
            double amount = 500;

            Guid TargetInvalidGuid = new("00000000-0000-0000-0000-000000000009");

            var actionResult = BankManager.Transfer(SourceGuid, TargetInvalidGuid, amount, User);

            var baderequest = actionResult as BadRequestObjectResult;

            Assert.IsTrue(actionResult is BadRequestObjectResult);
            Assert.IsTrue(baderequest.Value.Equals($"Invalid target {TargetInvalidGuid}"));
        }
        [Test]
        public void SourceInvalid()
        {
            double amount = 500;


            Guid SourceInvalidGuid = new("00000000-0000-0000-0000-000000000009");

            var actionResult = BankManager.Transfer(SourceInvalidGuid, TargetGuid, amount, User);

            var baderequest = actionResult as BadRequestObjectResult;

            Assert.IsTrue(actionResult is BadRequestObjectResult);
            Assert.IsTrue(baderequest.Value.Equals($"Invalid source {SourceInvalidGuid}"));
        }

        [Test]
        public void AmmountNegativInvalid()
        {
            double amount = -1000;

            var actionResult = BankManager.Transfer(SourceGuid, TargetGuid, amount, User);

            var baderequest = actionResult as BadRequestObjectResult;

            Assert.IsTrue(actionResult is BadRequestObjectResult);
            Assert.IsTrue(baderequest.Value.Equals($"Bad ammount {amount}"));
        }

        [Test]
        public void AmmountZeroInvalid()
        {
            double amount = 0;

            var actionResult = BankManager.Transfer(SourceGuid, TargetGuid, amount, User);

            var baderequest = actionResult as BadRequestObjectResult;

            Assert.IsTrue(actionResult is BadRequestObjectResult);
            Assert.IsTrue(baderequest.Value.Equals($"Bad ammount {amount}"));
        }

        [Test]
        public void AmountGreaterBalanceInvalid()
        {
            double amount = 50000;

            var actionResult = BankManager.Transfer(SourceGuid, TargetGuid, amount, User);

            var baderequest = actionResult as BadRequestObjectResult;

            Assert.IsTrue(actionResult is BadRequestObjectResult);
            Assert.IsTrue(baderequest.Value.Equals($"Not enough ammount {amount}"));
        }

        [Test]
        public void MultipleOKTransfersOk()
        {
            double amount = 100;

            var actionResult = BankManager.Transfer(SourceGuid, TargetGuid, amount, User);

            Assert.IsTrue(actionResult is OkResult);
            Assert.IsTrue(TargetAccount.Balance == 1100);
            Assert.IsTrue(SourceAccount.Balance == 1900);

            actionResult = BankManager.Transfer(SourceGuid, TargetGuid, amount, User);

            Assert.IsTrue(actionResult is OkResult);
            Assert.IsTrue(TargetAccount.Balance == 1200);
            Assert.IsTrue(SourceAccount.Balance == 1800);

            actionResult = BankManager.Transfer(SourceGuid, TargetGuid, amount, User);

            Assert.IsTrue(actionResult is OkResult);
            Assert.IsTrue(TargetAccount.Balance == 1300);
            Assert.IsTrue(SourceAccount.Balance == 1700);

            actionResult = BankManager.Transfer(SourceGuid, TargetGuid, amount, User);

            Assert.IsTrue(actionResult is OkResult);
            Assert.IsTrue(TargetAccount.Balance == 1400);
            Assert.IsTrue(SourceAccount.Balance == 1600);

            actionResult = BankManager.Transfer(SourceGuid, TargetGuid, amount, User);

            Assert.IsTrue(actionResult is OkResult);
            Assert.IsTrue(TargetAccount.Balance == 1500);
            Assert.IsTrue(SourceAccount.Balance == 1500);

            actionResult = BankManager.Transfer(TargetGuid, SourceGuid, 500, User);

            Assert.IsTrue(actionResult is OkResult);
            Assert.IsTrue(TargetAccount.Balance == 1000);
            Assert.IsTrue(SourceAccount.Balance == 2000);
        }
    }
}