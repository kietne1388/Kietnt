namespace FastFood.Models.Common
{
    public class UploadFileModel
    {
        public IFormFile? File { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public long FileSize { get; set; }
    }
}
