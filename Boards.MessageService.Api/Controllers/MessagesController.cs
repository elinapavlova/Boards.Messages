using System;
using System.Threading.Tasks;
using Boards.Auth.Common.Result;
using Boards.MessageService.Core.Dto.Message;
using Boards.MessageService.Core.Dto.Message.Create;
using Boards.MessageService.Core.Services.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boards.MessageService.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class MessagesController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        
        /// <summary>
        /// Create a message
        /// </summary>
        /// <response code="200">Return created message</response>
        /// <response code="400">If the message text is empty or files not loaded correctly</response>
        /// <response code="404">If thread doesn't exist</response>
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CreateMessageResponseDto>> Create([FromForm] CreateMessageRequestDto data)
            => await ReturnResult<ResultContainer<CreateMessageResponseDto>, CreateMessageResponseDto>
                (_messageService.Create(data));

        /// <summary>
        /// Get message by Id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Return message</response>
        /// <response code="404">If the message or reference to message doesn't exists</response>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageModelDto>> GetById(Guid id)
            => await ReturnResult<ResultContainer<MessageModelDto>, MessageModelDto>(_messageService.GetById(id));

        /// <summary>
        /// Delete message
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Return deleted message</response>
        /// <response code="404">If the message doesn't exists</response>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageModelResponseDto>> Delete(Guid id)
            => await ReturnResult<ResultContainer<MessageModelResponseDto>, MessageModelResponseDto>
                (_messageService.Delete(id));
    }
}