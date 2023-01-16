using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace MayiBeerCollection.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly string secretKey;
        private MayiBeerCollectionContext _contexto;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UsuarioController(IConfiguration config, MayiBeerCollectionContext context, IConfiguration configuration, IMapper mapper)
        {
            secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
            _contexto = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UsuarioLoginDTO request)
        {
            if(Authenticate(request))
            {
                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Login));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfiguration = tokenHandler.CreateToken(tokenDescriptor);

                string tokenCreated = tokenHandler.WriteToken(tokenConfiguration);

                return StatusCode(StatusCodes.Status200OK, new { token = tokenCreated });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }

        }

        private bool Authenticate(UsuarioLoginDTO userLogin)
        {
            var _user = (from tbl in _contexto.Usuarios where tbl.Login == userLogin.Login && tbl.Password == userLogin.Password select tbl).FirstOrDefault();
            if (_user != null)
            {
                return true;
            }
            else 
            { 
                return false; 
            }  
        }

    }
}
