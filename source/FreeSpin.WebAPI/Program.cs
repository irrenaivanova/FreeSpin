using FreeSpin.Application;
using FreeSpin.Infrastructure;
using FreeSpin.Infrastructure.Persistence;
using FreeSpin.Infrastructure.Persistence.Seeding.Common;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<FreeSpinDbContext>();
	await dbContext.Database.MigrateAsync();

	var seeder = scope.ServiceProvider.GetRequiredService<FreeSpinDbContextSeeder>();
	await seeder.SeedAsync(dbContext, scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
