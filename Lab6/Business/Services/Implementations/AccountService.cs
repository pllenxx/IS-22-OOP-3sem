using System.Text;
using System.Security.Cryptography;
using Business.Dto;
using DataAccess.Enums;
using Business.Exceptions;
using Business.Extensions;
using Business.Mapping;
using DataAccess.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Implementations;

internal class AccountService : IAccountService
{
    private readonly DatabaseContext _context;

    public AccountService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<AccountDto> CreateAccountForEmployee(string name, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new MessageProcessingSystemException("Invalid login");
        if (string.IsNullOrWhiteSpace(password))
            throw new MessageProcessingSystemException("Invalid password");
        var account = new Account(Guid.NewGuid(), name, GetPasswordHash(password), AccountType.Personal);
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync(cancellationToken);

        return account.AsDto();
    }

    public async Task<AccountDto> CreateAccountForSource(string info, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(info))
            throw new MessageProcessingSystemException("Invalid info for email/telephone/username");
        if (string.IsNullOrWhiteSpace(password))
            throw new MessageProcessingSystemException("Invalid password");
        var account = new Account(Guid.NewGuid(), info, GetPasswordHash(password), AccountType.MessageSource);
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync(cancellationToken);

        return account.AsDto();
    }

    public async Task<AccountDto> FindAccount(string mainInfo, string password)
    {
        if (string.IsNullOrWhiteSpace(mainInfo))
            throw new MessageProcessingSystemException("Invalid login");
        if (string.IsNullOrWhiteSpace(password))
            throw new MessageProcessingSystemException("Invalid password");

        var account = await _context.Accounts
            .SingleOrDefaultAsync(x => x.MainInfo == mainInfo && x.PasswordHash == GetPasswordHash(password));

        return account.AsDto();
    }

    public async Task<EmployeeDto> FindAccountBeholder(Guid id)
    {
        var employee = await _context.Employees.SingleOrDefaultAsync(x => x.Account.Id == id);
        return employee.AsDto();
    }

    public async Task<MessageSourceDto> FindAccountSourceBeholder(Guid id)
    {
        var source = await _context.MessageSources.SingleOrDefaultAsync(x => x.Account.Id == id);
        return source.AsDto();
    }

    public async Task GrantSupervisorRights(Guid id, CancellationToken cancellationToken)
    {
        Account account = await _context.Accounts.GetEntityAsync(id, cancellationToken);
        account.AllowReportCreation = true;
        var person = await _context.Employees.SingleOrDefaultAsync(x => x.Account == account);
        person.Type = EmployeeType.Supervisor;
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    private static string GetPasswordHash(string password)
    {
        using var hashingAlgorithm = SHA256.Create();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        return BitConverter.ToString(hashingAlgorithm.ComputeHash(passwordBytes));
    }
}