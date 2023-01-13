using Business.Dto;

namespace Business.Services.Implementations;

public interface IAccountService
{
    Task<AccountDto> CreateAccountForEmployee(string name, string password, CancellationToken cancellationToken);
    Task<AccountDto> CreateAccountForSource(string info, string password, CancellationToken cancellationToken);
    Task<AccountDto> FindAccount(string mainInfo, string password);
    Task<EmployeeDto> FindAccountBeholder(Guid id);
    Task<MessageSourceDto> FindAccountSourceBeholder(Guid id);
    Task GrantSupervisorRights(Guid id, CancellationToken cancellationToken);
}