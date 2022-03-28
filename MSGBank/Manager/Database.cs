using MSGBank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSGBank.Manager
{
    public class Database : IDatabase
    {
        private List<Account> Accounts { get; set; }
        private List<User> Users { get; set; }
        private List<Customer> Customers { get; set; }
        public Database()
        {
            Init();
        }

        private void Init()
        {
            InitCustomer();
            InitUser();
            InitAccounts();
        }

        private void InitAccounts()
        {
            Accounts = new();
            var customer = FindCustomer("Dimitri Bojarowv");
            Account account = new Account(1000, customer);
            account.Id = new Guid("173757d8-a226-4698-94ae-1a68cfe52106");
            Accounts.Add(account);
            customer.Accounts.Add(account);

            customer = FindCustomer("Dimitri Bojarowv");
            account = new Account(2000, customer);
            account.Id = new Guid("234e4293-43e4-45d5-91a5-7c0e917d5fdb");
            Accounts.Add(account);
            customer.Accounts.Add(account);
        }

        private void InitUser()
        {
            Users = new();
            Users.Add(new User(1, "Müller"));
            Users.Add(new User(2, "Maier"));
            Users.Add(new User(3, "Schmidt"));
        }

        private void InitCustomer()
        {
            Customers = new();
            Customers.Add(new Customer(1, "Dimitri Bojarowv"));
            Customers.Add(new Customer(2, "John Schmidt"));
            Customers.Add(new Customer(3, "Volker Deutschmann"));
            Customers.Add(new Customer(4, "Dominik Kolumbus"));
        }

        public User FindUser(string name)
        {
            return Users.FirstOrDefault<User>(x => x.Name == name);
        }
        public Customer FindCustomer(string name)
        {
            return Customers.FirstOrDefault<Customer>(x => x.Name == name);
        }
        public void AddCustomer(Customer customer)
        {
            //could check if exists
            Customers.Add(customer);
        }

        public void AddAccount(Account account)
        {
            Accounts.Add(account);
        }

        public Account FindAccount(Guid accountId)
        {
            return Accounts.FirstOrDefault<Account>(x => x.Id == accountId);
        }
    }
}
