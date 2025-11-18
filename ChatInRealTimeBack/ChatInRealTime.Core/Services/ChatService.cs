using AutoMapper;
using ChatInRealTime.Core.Dtos.Message;
using ChatInRealTime.Core.Entites;
using ChatInRealTime.Core.Interfaces;
using ChatInRealTime.Core.Specififcation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<AppMessage> _messageRepo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILogger<ChatService> _logger;
        public ChatService(IRepository<AppMessage> messageRepo, IMapper mapper, IUserService userService,
            ILogger<ChatService> logger)
        {
            _messageRepo = messageRepo;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }
        public async Task<IEnumerable<MessageDto>> GetMessages(int count)
        {
           var result = await _messageRepo.GetListBySpec(new MessageSpecification.TakeFewMessages(count));
           return result.Select(m => _mapper.Map<MessageDto>(m));
        }

        public async Task SaveMessage(MessageDto message)
        {
            AppMessage newMessage = _mapper.Map<AppMessage>(message);
            var user = await _userService.GetByEmail(message.UserName);
            try
            {
                newMessage.UserName = message.UserName;
                await _messageRepo.Insert(newMessage);
                await _messageRepo.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to insert message to database");
            }
        }
    }
}
