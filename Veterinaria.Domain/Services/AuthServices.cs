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

            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenAppSetting.GetSection("Key").Value));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _header = new JwtHeader(_signingCredentials);

            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(TypeClaims.IdUser,userEntity.IdUser.ToString()),
                new Claim(TypeClaims.FullName,userEntity.FullName),
                new Claim(TypeClaims.Email,userEntity.Email),
            };
            var _payload = new JwtPayload(
                    issuer: tokenAppSetting.GetSection("Issuer").Value,
                    audience: tokenAppSetting.GetSection("Audience").Value,
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(2)
                );
            var _token = new JwtSecurityToken(
                    _header,
                    _payload
                );
            TokenDto token = new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(_token),
                //Expiration = Helper.ConvertToUnixTimestamp(_token.ValidTo),
                DateExpiration= _token.ValidTo
            };

            return token;
        }

        #endregion



    }
}
