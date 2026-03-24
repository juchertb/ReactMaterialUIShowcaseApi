using System;

namespace ReactMaterialUIShowcaseApi.Dtos
{
    public class InvoiceDto
    {
        public string Id { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int CommandId { get; set; }
        public int CustomerId { get; set; }

        public float total_ex_taxes { get; set; }
        public float delivery_fees { get; set; }
        public float TaxRate { get; set; }
        public float Taxes { get; set; }
        public float Total { get; set; }
    }
}