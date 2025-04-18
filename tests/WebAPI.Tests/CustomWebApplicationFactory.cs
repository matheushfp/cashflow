﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Tests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private User _user;
    private string _password;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<CashFlowDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();

                StartDatabase(dbContext, passwordEncrypter);
            });
    }

    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password; 

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncrypter passwordEncrypter)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;

        _user.Password = passwordEncrypter.Encrypt(_user.Password);

        dbContext.Users.Add(_user);
        dbContext.SaveChanges();
    }
}
