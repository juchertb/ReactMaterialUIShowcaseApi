namespace ReactMaterialUIShowcaseApi.Dtos
{
    public class OrderDto
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public float total_ex_taxes { get; set; }
        public float delivery_fees { get; set; }
        public float TaxRate { get; set; }
        public float Taxes { get; set; }
        public float Total { get; set; }

        public int customer_id { get; set; }

        // ⭐ Nested DTO
        public CustomerDto Customer { get; set; } = new();

        public string? command_id { get; set; }
        public InvoiceDto? command { get; set; }

        public List<int> ReviewIds { get; set; } = new();
    }
}