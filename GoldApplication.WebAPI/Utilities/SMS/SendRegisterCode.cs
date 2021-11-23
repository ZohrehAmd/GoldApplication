using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Utilities.SMS
{
    public static class SendRegisterCode
    {
        public static string TextMessage(this string code)
        {
            return $" کد فعال سازی حساب کاربری شما : {int.Parse(code)}";
        }
    }
}
