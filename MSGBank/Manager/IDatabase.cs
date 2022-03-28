using MSGBank.Data;
using System;

namespace MSGBank.Manager
{
    public interface IDatabase
    {
        User FindUser(string username);
        void AddAccount(Account account);
        Customer FindCustomer(string customername);
        Account FindAccount(Guid accountId);
    }
}