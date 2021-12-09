using System;
using System.Net.Http;
using System.Threading.Tasks;
using Boards.MessageService.Core.Dto.Thread;
using Newtonsoft.Json;

namespace Boards.MessageService.Core.Services.Thread
{
    public class ThreadService : IThreadService
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public ThreadService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ThreadResponseDto> GetById(Guid id)
        {
            using var client = _clientFactory.CreateClient("BoardService");
            var response = await client.GetAsync($"{id}");
            var responseMessage = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ThreadResponseDto>(responseMessage);
            return result;
        }
    }
}