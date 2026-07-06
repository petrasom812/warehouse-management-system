

namespace WMS.WarehouseTransferSystem.Api.Models.Transfer
{
    public class TransferNumberGenerator
    {
        public static string GenerateTransferNumber()
        {
            // Date part (easy to read)
            string date = DateTime.UtcNow.ToString("yyMMdd");

            // Short unique part (safe + simple)
            string unique = Guid.NewGuid().ToString("N")[..6].ToUpper();

            // Final format
            return $"ORD-{date}-{unique}";
        }
    }
}