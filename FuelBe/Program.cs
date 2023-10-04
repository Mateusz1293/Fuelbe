using FuelBe.Database;
using FuelBe.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserResolver, UserResolver>();

builder.Services.AddDbContext<ReservationDbContext>(options => {
    options.UseSqlServer("Data Source=DESKTOP-J4TA6EL\\SQLEXPRESS;Initial Catalog=mati;Integrated Security=True",
        configuration => {
            options.EnableSensitiveDataLogging();
        });
}, ServiceLifetime.Transient);

var app = builder.Build();

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

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
