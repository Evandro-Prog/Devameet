using System.Security.Cryptography;
using System.Text;

namespace Devameet.Utils
{
    public class MD5Utils // classe que vai criar a criptografia de senha para que a senha seja salva criptografada no banco, ou seja descriptografada sempre que necessatio
    {
        public static string GenerateHashMd5(string text)
        {
            MD5 md5hash = MD5.Create();

            var bytes = md5hash.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder sb = new StringBuilder();

            foreach( var b in bytes )
            {
                sb = sb.Append(b);
            }

            return sb.ToString();
        }
    }
}
