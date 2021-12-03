using System.Collections.Generic;

namespace Boards.MessageService.Core.Dto.File.Upload
{
    public class UploadFilesResponseDto
    {
        public ICollection<FileResponseDto> Files { get; set; }
    }
}