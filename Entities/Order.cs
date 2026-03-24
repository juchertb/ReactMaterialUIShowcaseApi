using ReactMaterialUIShowcaseApi.Entities;
using System;
using System.Collections.Generic;

namespace ReactMaterialUIShowcaseApi.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public float TotalExTaxes { get; set; }
        public float DeliveryFees { get; set; }
        public float TaxRate { get; set; }
        public float Taxes { get; set; }
        public float Total { get; set; }

        // Foreign Key
        public int CustomerId { get; set; }

        // Navigation
        public Customer Customer { get; set; } = null!;
        public Invoice? Invoice { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}