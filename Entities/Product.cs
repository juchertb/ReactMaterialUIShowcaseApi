using System.Collections.Generic;

namespace ReactMaterialUIShowcaseApi.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Reference { get; set; } = string.Empty;

        public float Width { get; set; }
        public float Height { get; set; }
        public float Price { get; set; }

        public string Thumbnail { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Stock { get; set; }
        public int Sales { get; set; }
        public float Weight { get; set; }

        public string ShopifyHandle { get; set; } = string.Empty;

        public string FacebookAccount { get; set; } = string.Empty;
        public string InstagramAccount { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;

        // Foreign Keys
        public int CategoryId { get; set; }
        public int CollectionId { get; set; }
        public int ColorId { get; set; }

        // Navigation
        public Category Category { get; set; } = null!;
        public Collection Collection { get; set; } = null!;
        public Color Color { get; set; } = null!;

        public ICollection<ProductTag> Tags { get; set; } = new List<ProductTag>();
    }
}