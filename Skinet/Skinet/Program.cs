using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Skinet.Middleware;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository , ProductRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddCors();

/* To avoid authentication cookie is being blocked by the browser due to cross-site cookie restrictions (SameSite=Lax)
 * (usually when the request is between different domains or schemes — like http(Frontend) ↔ https(Backend),
 * or localhost:5001 ↔ localhost:4200).*/
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None; // Allow cross-site cookies
});

//builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
//{
//    var connString = builder.Configuration.GetConnectionString("Redis")
//        ?? throw new Exception("Cannot get redis connection string");

//    var cofiguration=ConfigurationOptions.Parse(connString,true);
//    return ConnectionMultiplexer.Connect(cofiguration);

//});

//builder.Services.AddSingleton<ICartService,CartService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<StoreContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();


//for handling errors

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x=> 
         x.AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()
         .WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.MapControllers();

app.MapGroup("api").MapIdentityApi<AppUser>(); //api/login

//Migrate and seed Data to DB

try
{
    using var scope=app.Services.CreateScope();
    var services=scope.ServiceProvider;
    var context=services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
	throw;
}

app.Run();
