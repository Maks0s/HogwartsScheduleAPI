using HogwartsScheduleAPI.Data;
using HogwartsScheduleAPI.Data.Seeder;
using HogwartsScheduleAPI.Mapper;
using Microsoft.EntityFrameworkCore;
using Serilog;

try
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateBootstrapLogger();

    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Started app building");

    builder.Host.UseSerilog((_, loggerConfig) =>
    {
        var jsonConfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true, reloadOnChange: true)
            .Build();

        loggerConfig
            .ReadFrom.Configuration(jsonConfig);
    });

    // Add services to the container.
    builder.Services.AddDbContext<HogwartsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddSingleton<IMapper, MapperProfile>();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    #region Data seeding
    //app.UseMiddleware<HogwartsDataSeederMiddleware>();
    #endregion

    app.UseSerilogRequestLogging();

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
}
catch(Exception ex)
{
    Log.Error(ex, "Crushed during the building");
}
finally
{
    Log.CloseAndFlush();
}
