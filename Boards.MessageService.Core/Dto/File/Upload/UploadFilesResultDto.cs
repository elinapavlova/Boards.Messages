using System.Collections.Generic;

namespace Boards.MessageService.Core.Dto.File.Upload
{
    public class UploadFilesResultDto
    {
        public ICollection<FileResponseDto> Data { get; set; }
        public int? ErrorType { get; set; }
    }
}