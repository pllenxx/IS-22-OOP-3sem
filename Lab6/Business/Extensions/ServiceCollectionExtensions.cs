using Business.Services;
using Business.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
namespace Business.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IEmployeeService, EmployeeService>();
        collection.AddScoped<IAccountService, AccountService>();
        collection.AddScoped<IReportService, ReportService>();

        return collection;
    }
}