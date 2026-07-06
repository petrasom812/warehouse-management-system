using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Product;
using WMS.WarehouseTransferSystem.Api.Interfaces.Product;
using WMS.WarehouseTransferSystem.Api.Models.Product;

namespace WMS.WarehouseTransferSystem.Api.Services.Product
{
    public class ServiceProduct : IServiceProduct
    {
        private readonly AppDbContext _context;
        public ServiceProduct(AppDbContext context)
        {
            _context = context;
        }
        //Create Product
        public async Task<GetProductDto> CreateProductAsync(CreateProductDto dto)
        {
            //Validate String
            ValidateString(dto.Sku);
            ValidateString(dto.ProductName);
            //Retrieve
            var createProduct = new ProductModel
            {
                Sku = dto.Sku,
                ProductName = dto.ProductName
            };
            //Mutate
            _context.Products.Add(createProduct);
            //Persist
            await _context.SaveChangesAsync();

            return MapToProductDto(createProduct);
        }
        //Get Product
        public async Task<List<GetProductDto>> GetProductAsync()
        {
            //Retrive
            var getProducts = await _context.Products
                .AsNoTracking()
                .ToListAsync();
            //Mutate
            return getProducts
                .Select(MapToProductDto)
                .ToList();
        }
        //Get Product by Id
        public async Task<GetProductDto?> GetProductByIdAsync(int id)
        {
            //Retrieve
            var getProductById = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            //Validate
            return getProductById == null ? null : MapToProductDto(getProductById);
        }
        //Update Product
        public async Task<UpdateProductDto?> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            //Retrieve
            var updateProduct = await _context.Products.FindAsync(id);
            //Validate
            ValidateString(dto.Sku);
            ValidateString(dto.ProductName);
            if(updateProduct == null)
                return null;
            //Mutate
            updateProduct.Sku = dto.Sku;
            updateProduct.ProductName = dto.ProductName;
            //Persist
            await _context.SaveChangesAsync();
            return new UpdateProductDto
            {
                Sku = updateProduct.Sku,
                ProductName = updateProduct.ProductName
            };
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            //Retrieve
            var deleteProduct = await _context.Products.FindAsync(id);
            //Validate
            if(deleteProduct == null)
                return false;
            //Mutate
            _context.Products.Remove(deleteProduct);
            //Persist
            await _context.SaveChangesAsync();

            return true;

        }
        //Mapper
        private static GetProductDto MapToProductDto(ProductModel p) => new()
        {
            Id = p.Id,
            Sku = p.Sku,
            ProductName = p.ProductName
        };
        //Validate String
        public string ValidateString(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.");
            return value;
        }
    }
}