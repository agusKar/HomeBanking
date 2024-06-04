﻿using HomeBanking.Models;

namespace HomeBanking.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        Account GetAccountById(long id);
        void SaveAccount(Account account);
        IEnumerable<Account> GetAllAccountsByCliente(long clientId);
        IEnumerable<Account> GetAccountsByClient(long clientId);
    }
}
