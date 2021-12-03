using System.Collections.Generic;

namespace Core.Dto.File.Upload
{
    public class UploadFilesResponseDto
    {
        public ICollection<FileResponseDto> Files { get; set; }
    }
}