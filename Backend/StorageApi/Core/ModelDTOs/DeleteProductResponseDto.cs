

namespace StorageApi.Core.ModelsDTO
{
    public class DeleteProductResponseDto
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public Guid DeletedProductId { get; set; }
    }
}