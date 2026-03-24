using System;

namespace ReactMaterialUIShowcaseApi.Entities
{
    public class Invoice
    {
        public string Id { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public float TotalExTaxes { get; set; }
        public float DeliveryFees { get; set; }
        public float TaxRate { get; set; }
        public float Taxes { get; set; }
        public float Total { get; set; }

        // Foreign Key
        public int OrderId { get; set; }

        // Navigation
        public Order Order { get; set; } = null!;
    }
}