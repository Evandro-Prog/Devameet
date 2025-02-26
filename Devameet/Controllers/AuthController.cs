using Devameet.Dtos;
using Devameet.Models;
using Devameet.Repository;
using Devameet.Service;
using Devameet.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Devameet.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, IUserRepository userRepository, IConfiguration configuration) //Construtor que recebe o logger e utiliza em todos os metodos do controller
        {
            _logger = logger;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous] // Permite que seja chamado sem autenticação
        [Route("api/[controller]/login")]
        public IActionResult ExecuteLogin([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                if(!string.IsNullOrEmpty(loginRequestDto.Login) || !string.IsNullOrEmpty(loginRequestDto.Password) && 
                    !string.IsNullOrWhiteSpace(loginRequestDto.Login) || !string.IsNullOrWhiteSpace(loginRequestDto.Password))
                {
                    User user = _userRepository.GetUserByLoginPassword(loginRequestDto.Login.ToLower(), MD5Utils.GenerateHashMd5(loginRequestDto.Password)); 

                    if (user != null) 
                    {
                        return Ok(new LoginResponseDto()
                        {
                            Email = user.Email,
                            Name = user.Name,
                            Token = TokenService.CreateToken(user, _configuration["JWT:SecretKey"])
                        });
                    }
                    else
                    {
                        return BadRequest(new ErrorResponseDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Description = "Usuario e/ou senha inválidos."
                        });
                    }
                }
                else
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Description = "Preencha os campos de LOGIN e SENHA corretamente."                       
                    });
                }
            }catch (Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro ao efetuar login: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro  ao efetuar login: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost]
        [AllowAnonymous] // Permite que seja chamado sem autenticação
        [Route("api/[controller]/register")]
        public IActionResult SaveUser([FromBody] UserRegisterDto userdto) // Pega as informações do Body da requisição que foram transacionadas pelo DTO
        {
            try
            {
                if(userdto != null)
                {
                    var errors = new List<string>(); // Lista os erros e retorna para o usuário

                    if(string.IsNullOrEmpty(userdto.Name) || string.IsNullOrWhiteSpace(userdto.Name))
                    {
                        errors.Add("Campo nome inválicdo.");
                    }
                    if(string.IsNullOrEmpty(userdto.Email) || string.IsNullOrWhiteSpace(userdto.Email) || !userdto.Email.Contains("@"))
                    {
                        errors.Add("Campo email inválido.");
                    }
                    if(string.IsNullOrEmpty(userdto.Password) || string.IsNullOrWhiteSpace(userdto.Password))
                    {
                        errors.Add("Campo senha inválido");
                    }

                    if(errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            _logger.LogError(error);
                            
                        }

                        return BadRequest(new ErrorResponseDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Description = "Erros encontrados na requisição: ",
                            Errors = errors
                        });                        
                    }

                    User user = new User()
                    {
                        Name = userdto.Name.ToLower(),
                        Email = userdto.Email,
                        Password = MD5Utils.GenerateHashMd5(userdto.Password),
                        Avatar = userdto.Avatar,
                    };

                    if (!_userRepository.VerifyEmail(user.Email))
                    {
                        _userRepository.Save(user);
                    }
                    else
                    {
                        _logger.LogError("Já existe uma conta associada ao email informado.");
                        return BadRequest("Já existe uma conta associada ao email informado.");
                    }                   

                }
                else
                {
                    _logger.LogError("Preencha os dados de cadastro corretamente.");
                    return BadRequest("Os campos de cadastro de usuário não podem estar vazios/ em branco.");
                }

                return Ok("Usuário cadastrado com sucesso");

            }
            catch(Exception ex)
            {
                _logger.LogError("Ocorreu o seguinte erro: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }            
        }

    }
}
