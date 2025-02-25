using Devameet.Dtos;
using Devameet.Models;
using Devameet.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Devameet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;


        public UserController(ILogger<UserController> logger, IUserRepository userRepository) : base(userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            try
            {
                User user = GetToken();
                return Ok(new UserResponseDto
                {
                    Name = user.Name,
                    Email = user.Email,
                    Avatar = user.Avatar,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro ao buscar usuário: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro ao buscar usuário: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }            
        }

        [HttpGet]
        [Route("api/[controller]/getuserbyid")]
        public IActionResult GetUserById(int iduser)
        {
            try
            {
                User user = _userRepository.GetUserByLogin(iduser);
                return Ok(new UserResponseDto
                {
                    Name = user.Name,
                    Email = user.Email,
                    Avatar = user.Avatar,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro ao buscar usuário: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro ao buscar usuário: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }            
        }

    }
}
