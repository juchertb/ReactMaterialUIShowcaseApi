namespace ReactMaterialUIShowcaseApi.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public int category_id { get; set; }
        public string category_name { get; set; } = string.Empty;

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

        public int collection_id { get; set; }
        public string collection_name { get; set; } = string.Empty;

        public int color_id { get; set; }
        public string color_name { get; set; } = string.Empty;

        public string ShopifyHandle { get; set; } = string.Empty;

        public string FacebookAccount { get; set; } = string.Empty;
        public string InstagramAccount { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new();

        public string Currency { get; set; } = string.Empty;

        public string Sku { get; set; } = string.Empty;
    }
}