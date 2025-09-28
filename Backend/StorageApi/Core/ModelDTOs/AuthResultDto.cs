

namespace StorageApi.Core.ModelsDTO
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
