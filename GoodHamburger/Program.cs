using GoodHamburger.Data;
using GoodHamburger.Models;
using GoodHamburger.Repositories;
using GoodHamburger.Repositories.interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<GoodHamburgerDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase"))
    );

builder.Services.AddScoped<ISandwichRepository, SandwichRepository>();
builder.Services.AddScoped<IExtraRepository, ExtraRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GoodHamburgerDbContext>();

    if (!dbContext.Sandwiches.Any())
    {
        dbContext.Sandwiches.Add(new SandwichModel { Name = "X Burger", Price = 5.00M });
        dbContext.Sandwiches.Add(new SandwichModel { Name = "X Egg", Price = 4.50M });
        dbContext.Sandwiches.Add(new SandwichModel { Name = "X Bacon", Price = 7.00M });
        
        dbContext.SaveChanges();
    }

    if (!dbContext.Extras.Any())
    {
        dbContext.Extras.Add(new ExtraModel { Name = "Fries", Price = 2.00M });
        dbContext.Extras.Add(new ExtraModel { Name = "Soft drink", Price = 2.50M });

        dbContext.SaveChanges();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
