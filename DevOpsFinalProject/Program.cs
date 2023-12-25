using DevOpsFinalProject.Config;
using DevOpsFinalProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICacheService, CacheService>(); // registration of cache service in container
builder.Services.AddScoped<IHouseRentDBService, HouseRentDBService>();

builder.Services.Configure<MongoDbConfig>(
    builder.Configuration.GetSection("MongoDb")
);

var redisConfig = new RedisConfig();
builder.Configuration.GetSection("RedisConfig").Bind(redisConfig);

builder.Services.AddStackExchangeRedisCache(op =>
{
    op.Configuration = redisConfig.Configuration;
    op.InstanceName = redisConfig.InstanceName;
});

var app = builder.Build();

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
