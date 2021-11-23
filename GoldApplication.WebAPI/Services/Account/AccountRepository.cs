using GoldApplication.WebAPI.Contexts;
using Microsoft.Extensions.Configuration;
using System;
using SmsIrRestful;
using System.Linq;
using System.Collections.Generic;
using GoldApplication.WebAPI.Utilities.Setting;
using Microsoft.Extensions.Options;
using GoldApplication.WebAPI.Utilities.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using GoldApplication.WebAPI.Dtos.User;
using GoldApplication.WebAPI.Utilities.SMS;
using Microsoft.EntityFrameworkCore;

namespace GoldApplication.WebAPI.Services.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly GoldApplicationContext _context;
        private readonly IConfiguration _configuration;
        public readonly AppSettings appSettings;
        public AccountRepository(GoldApplicationContext context,IConfiguration configuration,IOptions<AppSettings> appSettings)
        {
            _context = context;
            _configuration = configuration;
            this.appSettings = appSettings.Value;
        }

        public Models.User LoginUser(string mobile, string pass)
        {
            var hashPass = PasswordHelper.EncodeProSecurity(pass.Trim());
            var user = _context.Users.SingleOrDefault
                (u => u.Mobile == mobile.Trim().ToLower() && u.Password==hashPass);
            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var toKenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name , user.Mobile),
            new Claim(ClaimTypes.Role , user.Role)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
            new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
            )
            };

            var token = tokenHandler.CreateToken(toKenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        public Models.User RegisterUser(RegisterUserDto register)
        {
            Models.User user = new()
            {
                Avatar="Default.jpg",IsActive=false,Description="",FirstName=register.FirstName,
                LastName=register.LastName,Mobile=register.Mobile.Trim().ToLower(),Password=PasswordHelper.EncodeProSecurity(register.Password.Trim()),
                RegisterCode=RandomCode.GetNewRandom(),RegisterDate=DateTime.Now,
                Role="User"
            };
            try
            {
                SensSMS(user.Mobile, SendRegisterCode.TextMessage(user.RegisterCode));
                _context.Users.Add(user);
                _context.SaveChanges();
                return user;
            }
            catch
            {
                return null;
            } 
            
           

        }

        public void SensSMS(string mobile, string message)
        {
            var token = GetToken();
            var lines = new SmsLine().GetSmsLines(token);
            if (lines == null) return ;

            var line = lines.SMSLines.Last().LineNumber.ToString();
            var data = new MessageSendObject()
            {
                Messages=new List<string> { message}.ToArray(),
                MobileNumbers = new List<string> { mobile}.ToArray(),
                LineNumber=line,
                SendDateTime = DateTime.Now,
                CanContinueInCaseOfError=true
            };
            var messageSendResponseObject = new MessageSend().Send(token, data);

            if (messageSendResponseObject.IsSuccessful) return;

            line = lines.SMSLines.First().LineNumber.ToString();
            data.LineNumber = line;
            new MessageSend().Send(token, data);
        }

        private string GetToken()
        {
            var smsSecret = _configuration.GetSection("SmsSecrets");
            var tokenService = new Token();
            return tokenService.GetToken(smsSecret["ApiKey"], smsSecret["SecretKey"]);
        }

        public bool IsPasswordTrue(LoginUserDto model)
        {
            var user = _context.Users.Single(u => u.Mobile == model.Mobile.Trim().ToLower());
            var hashPass = PasswordHelper.EncodeProSecurity(model.Password);
            if (hashPass != user.Password)
                return false;

            return true;
        }
    }
}
