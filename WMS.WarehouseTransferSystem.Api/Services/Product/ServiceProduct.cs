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
        #region CRUD Operations
        //Create Product
        public async Task<GetProductDto> CreateProductAsync(CreateProductDto dto)
        {
            //Validate: input string
            ValidateString(dto.Sku);
            ValidateString(dto.ProductName);
            //validate: Duplicate product
            await EnsureProductNotExistForCreate(dto.Sku);
            //mutate
            var product = new ProductModel
            {
                Sku = dto.Sku,
                ProductName = dto.ProductName
            };
            _context.Products.Add(product);
            //Persist
            await _context.SaveChangesAsync();

            return MapToProductDto(product);
        }
        //Get Product
        public async Task<List<GetProductDto>> GetProductAsync()
        {
            //Retrive and validate
            return await _context.Products
                .AsNoTracking()
                .Select(p => new GetProductDto
                {
                    Id = p.Id,
                    Sku = p.Sku,
                    ProductName = p.ProductName
                })
                .ToListAsync();
        }
        //Get Product by Id
        public async Task<GetProductDto?> GetProductByIdAsync(int id)
        {
            //Retrieve
            var product = await EnsureProductExists(id);
            //Validate
            return MapToProductDto(product);
        }
        //Update Product
        public async Task<GetProductDto?> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            //Validate input string
            ValidateString(dto.Sku);
            ValidateString(dto.ProductName);
            //validate: Duplicate product
            await EnsureProductNotExistForUpdate(id, dto.Sku);
            //validate: if product exist
            var product = await EnsureProductExists(id);
            //Mutate
            product.Sku = dto.Sku;
            product.ProductName = dto.ProductName;
            //Persist
            await _context.SaveChangesAsync();
            //return
            return MapToProductDto(product);
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            //Retrieve and validate
            var product = await EnsureProductExists(id);
            //Mutate
            _context.Products.Remove(product);
            //Persist
            await _context.SaveChangesAsync();
            //return
            return true;

        }
        #endregion CRUD Operations
        #region Validation Helper
        //Ensure string input correctly
        private static void ValidateString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.");
        }
        //Ensure product does not exist for create
        private async Task EnsureProductNotExistForCreate(string sku)
        {
            //retrieve
            var product = await _context.Products
                .AnyAsync(p => p.Sku == sku);
            //validate
            if(product)
                throw new ArgumentException("Product already exists.");   
        }
        //Ensure product does not exist for update
        private async Task EnsureProductNotExistForUpdate(int id, string sku)
        {
            //retrieve
            var product = await _context.Products
                .AnyAsync(p => p.Id != id && p.Sku == sku);
            //validate
            if(product)
                throw new ArgumentException("Product already exists.");
        }
        //Ensure Product exists (filter by id)
        private async Task<ProductModel> EnsureProductExists(int id)
        {
            //retrieve
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
            //validate
            if(product == null)
                throw new KeyNotFoundException("Product does not exist.");
            //return
            return product;
        }
        #endregion Validation Helper
        #region Mapper
        //Mapper
        private static GetProductDto MapToProductDto(ProductModel p) => new()
        {
            Id = p.Id,
            Sku = p.Sku,
            ProductName = p.ProductName
        };
        #endregion Mapper
    }
}