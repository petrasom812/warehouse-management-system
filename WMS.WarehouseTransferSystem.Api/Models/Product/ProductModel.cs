using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.WarehouseTransferSystem.Api.Models.Product
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
    }
}