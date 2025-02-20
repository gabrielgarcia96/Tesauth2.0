using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using teste.App.Components;
using teste.App.Security;
using teste.Application.Interfaces;
using teste.Application.Services;
using teste.Domain.Interfaces;
using teste.Infrastructure.Data;
using teste.Infrastructure.Interfaces;
using teste.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthorizationCore();


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddRadzenComponents();

// Services and Repositorys
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

// add authentication: 
builder.Services.AddAuthentication();


// Important line, for authentication don't remove
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthProvider>());
builder.Services.AddScoped<CustomAuthProvider>();

// LocalStorage
builder.Services.AddBlazoredLocalStorage();

//Config Database
builder.Services.AddSingleton<ConfigMongoDb>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration["ConnectionString:MongoDb"];
    var databaseName = configuration["MongoDbSettings:DatabaseName"];
    return new ConfigMongoDb(connectionString, databaseName);
});

// Start Don't remove
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
