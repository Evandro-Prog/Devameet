namespace Devameet.Dtos
{
    public class UserRegisterDto // Dtos são objetos de transisão temporaria de dados para trafego de informações.
    {
        public string Name {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Avatar {  get; set; }
    }
}
