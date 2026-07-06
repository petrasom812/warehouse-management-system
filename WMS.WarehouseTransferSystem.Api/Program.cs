using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.Interfaces;
using WMS.WarehouseTransferSystem.Api.Interfaces.Product;
using WMS.WarehouseTransferSystem.Api.Interfaces.Transfer;
using WMS.WarehouseTransferSystem.Api.Services.Inventory;
using WMS.WarehouseTransferSystem.Api.Services.Product;
using WMS.WarehouseTransferSystem.Api.Services.Transfer;
using WMS.WarehouseTransferSystem.Api.Services.Warehouse;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddScoped<IServiecWarehouse, ServiceWarehouse>();
builder.Services.AddScoped<IServiceProduct, ServiceProduct>();
builder.Services.AddScoped<IServiceInventory, ServiceInventory>();
builder.Services.AddScoped<IServiceTransfer, ServiceTransfer>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=warehouseTransfer.db"));
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.MapControllers();
app.Run();
