using Microsoft.EntityFrameworkCore;
using TechnicalTest.Authorization;
using TechnicalTest.Helpers;
using TechnicalTest.Services;

var builder = WebApplication.CreateBuilder(args);

{
    var services = builder.Services;
    var env = builder.Environment;

    if (env.IsProduction()) services.AddDbContext<DataContext>();
    else services.AddDbContext<DataContext>();

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddCors();
    services.AddControllers();
    services.AddAutoMapper(typeof(Program));
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IUserService, UserService>();
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
