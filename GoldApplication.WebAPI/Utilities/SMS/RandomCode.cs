using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Utilities.SMS
{
    public static class RandomCode
    {
        public static string GetNewRandom()
        {
            Random random = new();
            return random.Next(00000, 99999).ToString();
        }
    }
}
