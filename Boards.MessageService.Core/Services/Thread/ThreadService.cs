using System;
using System.Threading.Tasks;
using AutoMapper;
using Boards.MessageService.Core.Dto.Thread;
using Boards.MessageService.Database.Repositories.Thread;

namespace Boards.MessageService.Core.Services.Thread
{
    public class ThreadService : IThreadService
    {
        private readonly IThreadRepository _threadRepository;
        private readonly IMapper _mapper;
        
        public ThreadService
        (
            IThreadRepository threadRepository, 
            IMapper mapper
        )
        {
            _threadRepository = threadRepository;
            _mapper = mapper;
        }

        public async Task<ThreadResponseDto> GetById(Guid id)
        {
            var thread = await _threadRepository.GetById(id);
            var result = _mapper.Map<ThreadResponseDto>(thread);
            return result;
        }
    }
}