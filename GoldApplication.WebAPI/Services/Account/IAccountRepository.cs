using GoldApplication.WebAPI.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Services.Account
{
    public interface IAccountRepository
    {
        Models.User LoginUser(string mobile, string pass);
        Models.User RegisterUser(RegisterUserDto register);
        void SensSMS(string mobile, string message);
        bool IsPasswordTrue(LoginUserDto model);
    }
}
