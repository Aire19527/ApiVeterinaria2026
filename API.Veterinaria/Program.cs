using Microsoft.EntityFrameworkCore;
using Veterina.Core.Data;
using Veterina.Core.Repository;
using Veterina.Core.Repository.Interface;
using Veterina.Core.UnitOfWork;
using Veterina.Core.UnitOfWork.Interface;
using Veterinaria.Domain.Services;
using Veterinaria.Domain.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


#region SQL SERVER
builder
    .Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringSQLServer"));
});
#endregion

// Infrastructure
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRolServices, RolServices>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
