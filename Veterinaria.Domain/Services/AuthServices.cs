using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore.Encrypt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Veterina.Core.UnitOfWork.Interface;
using Veterinaria.Common.Exceptions;
using Veterinaria.Common.Helpers;
using Veterinaria.Domain.Services.Interfaces;
using Veterinaria.Entity.DTO.Tokens;
using Veterinaria.Entity.Models.Secure;
using static Veterinaria.Common.Constant.Const;

namespace Veterinaria.Domain.Services
{
    public class AuthServices : IAuthServices
    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserServices _userServices;
        private readonly IConfiguration _configuration;
        #endregion

        #region Builder
        public AuthServices(IUnitOfWork unitOfWork, IUserServices userServices, IConfiguration configuration)
        {
            this._unitOfWork = unitOfWork;
            this._userServices = userServices;
            this._configuration = configuration;
        }
        #endregion


        #region Methods
        public async Task<TokenDto> Login(LoginDto login)
        {
            UserEntity? user = await _userServices.GetUserByEmail(login.Email);
            if (user == null || user.Password != EncryptProvider.Sha256(login.Password))
            {
                throw new BusinessException("Credenciales incorrectas");
            }

            return GenerateTokenJWT(user);

        }
        #endregion

        #region Privates


        private TokenDto GenerateTokenJWT(UserEntity userEntity)
        {
            IConfigurationSection tokenAppSetting = _configuration.GetSection("Tokens");
            // Clave secreta para firmar. La validación usa la misma Tokens:Key.
            string? key = tokenAppSetting.GetSection("Key").Value;
            // Quién emite el token. La validación lo compara con ValidIssuer.
            string? issuer = tokenAppSetting.GetSection("Issuer").Value;
            // Para quién es el token. La validación lo compara con ValidAudience.
            string? audience = tokenAppSetting.GetSection("Audience").Value;
            // Minutos de vida del token, desde el login. Viene de Tokens:ExpireMinutes.
            string? expireMinutesValue = tokenAppSetting.GetSection("ExpireMinutes").Value;

            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException("La configuración Tokens debe incluir Key, Issuer y Audience.");

            if (!int.TryParse(expireMinutesValue, out int expireMinutes) || expireMinutes <= 0)
                throw new InvalidOperationException("La configuración Tokens:ExpireMinutes debe ser un número mayor que cero.");

            // La misma clave y el mismo algoritmo (HMAC-SHA256) deben usarse al validar la firma.
            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _header = new JwtHeader(_signingCredentials);

            // Datos que viajan dentro del token. No poner la contraseña ni otros secretos.
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(TypeClaims.IdUser,userEntity.IdUser.ToString()),
                new Claim(TypeClaims.FullName,userEntity.FullName),
                new Claim(TypeClaims.Email,userEntity.Email),
            };
            var _payload = new JwtPayload(
                    issuer: issuer,
                    audience: audience,
                    claims: _Claims,
                    // nbf y exp van en UTC. JWT guarda un instante, no la hora local del servidor.
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(expireMinutes)
                );
            var _token = new JwtSecurityToken(
                    _header,
                    _payload
                );
            TokenDto token = new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(_token),
                // ValidTo es UTC. En el JSON sale con Z; no es la hora de Colombia.
                DateExpiration= _token.ValidTo
            };

            return token;
        }

        #endregion



    }
}
