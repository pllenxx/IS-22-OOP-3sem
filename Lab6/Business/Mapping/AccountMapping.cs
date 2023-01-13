using Business.Dto;
using DataAccess.Entities;

namespace Business.Mapping;

public static class AccountMapping
{
    public static AccountDto AsDto(this Account account)
            => new AccountDto(account.Id, account.MainInfo, account.Type.ToString());
}