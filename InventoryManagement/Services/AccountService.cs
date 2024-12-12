using InventoryManagement.DTOs;
using InventoryManagement.Models;
using InventoryManagement.Repositories;
using Serilog;
using System;
using System.Data;

namespace InventoryManagement.Services
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;
        private readonly ILogger _logger;
        public AccountService(AccountRepository accountRepository, ILogger logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public bool VerifyLogin(AccountDto accountDto)
        {
            try
            {
                var account = _accountRepository.GetAccountByCredentials(accountDto.UserName, accountDto.Password);

                if(account != null)
                {
                    accountDto.Role = account.Role;
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred during login verification", ex);
                throw;
            }
        }
    }
}