using ReactMaterialUIShowcaseApi.Dtos;
using System;

namespace ReactMaterialUIShowcaseApi.Entities
{
    public class Review
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public float TotalExTaxes { get; set; }
        public float DeliveryFees { get; set; }
        public float TaxRate { get; set; }
        public float Taxes { get; set; }
        public float Total { get; set; }

        public string Text { get; set; } = string.Empty;

        // Foreign Keys
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        //public int InvoiceId { get; set; }

        // Navigation
        public Order Order { get; set; } = null!;
        public Customer Customer { get; set; } = null!;

        //public Invoice? Invoice { get; set; }
    }
}