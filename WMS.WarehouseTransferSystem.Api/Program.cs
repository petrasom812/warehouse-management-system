using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.Interfaces;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;
using WMS.WarehouseTransferSystem.Api.Interfaces.Product;
using WMS.WarehouseTransferSystem.Api.Interfaces.Transfer;
using WMS.WarehouseTransferSystem.Api.Services.Auth;
using WMS.WarehouseTransferSystem.Api.Services.Inventory;
using WMS.WarehouseTransferSystem.Api.Services.Product;
using WMS.WarehouseTransferSystem.Api.Services.Transfer;
using WMS.WarehouseTransferSystem.Api.Services.Warehouse;
using System.Text;
using WMS.WarehouseTransferSystem.Api.Settings;
using WMS.WarehouseTransferSystem.Api.Interfaces.Warehouse;
using WMS.WarehouseTransferSystem.Api.Interfaces.Inventory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddScoped<IServiceWarehouse, ServiceWarehouse>();
builder.Services.AddScoped<IServiceProduct, ServiceProduct>();
builder.Services.AddScoped<IServiceInventory, ServiceInventory>();
builder.Services.AddScoped<IServiceTransfer, ServiceTransfer>();
builder.Services.AddScoped<IServiceUser, ServiceUser>();
builder.Services.AddScoped<IServiceRole, ServiceRole>();
builder.Services.AddScoped<IServiceUserRole, ServiceUserRole>();
builder.Services.AddScoped<IServiceAuth, ServiceAuth>();
builder.Services.AddScoped<IServiceJwt, JwtService>();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);
var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings!.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Key
                        )
                    )
            };
    });
builder.Services.AddAuthorization();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
