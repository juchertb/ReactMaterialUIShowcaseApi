using ReactMaterialUIShowcaseApi.Dtos;
using System;

namespace ReactMaterialUIShowcaseApi
{
    public class ReviewDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string? command_id { get; set; }

        public InvoiceDto? command { get; set; }

        public int CustomerId { get; set; }

        public decimal TotalExTaxes { get; set; }

        public decimal DeliveryFees { get; set; }

        public decimal TaxRate { get; set; }

        public decimal Taxes { get; set; }

        public decimal Total { get; set; }

        public string Text { get; set; } = string.Empty;
    }
}
