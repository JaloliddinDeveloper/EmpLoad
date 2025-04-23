using EmpLoad.Brokers.Storages;
using EmpLoad.Services.Foundations.Employees;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddTransient<IStorageBroker, StorageBroker>();
        builder.Services.AddTransient<IEmployeeServce, EmployeeServce>();

        var app = builder.Build();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}