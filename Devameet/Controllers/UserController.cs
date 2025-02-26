using Devameet.Dtos;
using Devameet.Models;
using Devameet.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Devameet.Controllers
{
    [ApiController]    
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;        


        public UserController(ILogger<UserController> logger, IUserRepository userRepository) : base(userRepository)
        {
            _logger = logger;            
        }

        [HttpGet]
        [Route("api/[controller]")]
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

        [HttpPut]
        [Route("api/[controller]")]
        public IActionResult UpdateUser([FromBody]UserRequestDto userdto)
        {
            try
            {
                User user = GetToken();
                if(user != null)
                {
                    if(!string.IsNullOrEmpty(user.Name) && !string.IsNullOrWhiteSpace(user.Name) &&
                        !string.IsNullOrEmpty(user.Avatar) && !string.IsNullOrWhiteSpace(user.Avatar))
                    {
                        user.Avatar = userdto.Avatar;
                        user.Name = userdto.Name;

                        _userRepository.UpdateUser(user);

                        return Ok("Dados atualizados com sucesso.");
                    }
                    else
                    {
                        _logger.LogError("Ocampos dever ser preenchidos corretamente.");
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponseDto()
                        {
                            Description = "Ocampos dever ser preenchidos corretamente.",
                            Status = StatusCodes.Status400BadRequest
                        });
                    }
                    
                }
                else
                {
                    _logger.LogError("Usuário inválido.");
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                    {
                        Description = "suário inválido.",
                        Status = StatusCodes.Status500InternalServerError
                    });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro ao atualizar usuário: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro ao atualizar usuário: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

    }
}
