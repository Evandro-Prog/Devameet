using Devameet.Dtos;
using Devameet.Models;
using Devameet.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Devameet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetController : BaseController
    {
        private readonly ILogger<MeetController> _logger;
        private readonly IMeetRepository _meetRepository;

        public MeetController(ILogger<MeetController> logger, IUserRepository userRepository, IMeetRepository meetRepository) : base(userRepository)
        {
            _logger = logger;
            _meetRepository = meetRepository;
        }

        [HttpGet]
        public IActionResult GetMeet()
        {
            try
            {
                User user = GetToken();

                List<Meet> meets = _meetRepository.GetMeetsByUser(user.Id);

                return Ok(meets);
            }
            catch(Exception ex) 
            {
                _logger.LogError("Ocorreu o seguinte erro ao buscar salas de reunião: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro ao buscar salas de reunião: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
            
            
        }
        
    }
}
