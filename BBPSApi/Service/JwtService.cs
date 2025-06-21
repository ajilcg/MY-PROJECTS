using BBPSApi.Data;
using BBPSApi.Handlers;
using BBPSApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NGLMoSy.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace BBPSApi.Service
{
    public class JwtService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient;

        public JwtService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

        }
        public async Task<LoginResponseModel?> Authenticate(LoginRequestModel request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                    return null;



                var userNameDecrypted = "";
                var passwordDecrypted = "";
                var secret = "8080808080808080";

                var key1 = Encoding.UTF8.GetBytes("kljsdkkdlo4454GG00155sajuklmbkdl");
                var iv = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");

                //var usrEncrypt = CommonEncryptDecrypt.Encrypt("369145");
                //var pswdEncrypt = CommonEncryptDecrypt.Encrypt("MAFILNGL369145");


                using (Aes myAes = Aes.Create())
                {


                    userNameDecrypted = CommonEncryptDecrypt.DecryptStringFromBytes_Aes(Convert.FromBase64String(request.Username), key1, iv);
                    passwordDecrypted = CommonEncryptDecrypt.DecryptStringFromBytes_Aes(Convert.FromBase64String(request.Password), key1, iv);
                }

                if (userNameDecrypted != "369145" || passwordDecrypted != "MAFILNGL@#110912") return null;

                //var issuer = _configuration["JwtConfig:Issuer"];
                //var audience = _configuration["JwtConfig:Audience"];
                var key = _configuration["JwtConfig:Key"];
                var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
                var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

                var permClaims = new List<Claim>();
                permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                permClaims.Add(new Claim("valid", "1"));
                permClaims.Add(new Claim("Company", "MAFIL"));
                permClaims.Add(new Claim(JwtRegisteredClaimNames.Name, request.Username));
                var claimsIdentity = new ClaimsIdentity(permClaims);

                var tokenDiscriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = tokenExpiryTimeStamp,
                    //Issuer = issuer,
                    //Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha512Signature),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDiscriptor);
                var accessToken = tokenHandler.WriteToken(securityToken);
              

                return new LoginResponseModel
                {
                    AccessToken = accessToken,
                    UserName = request.Username,
                    ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
                };

            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        private void VerifyPassword(string password, string v)
        {
            throw new NotImplementedException();
        }
    }
}
