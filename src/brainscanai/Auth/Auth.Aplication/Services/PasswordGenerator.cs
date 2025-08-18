using Auth.Application.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
    public class PasswordGenerator:IPasswordGenerator
    {
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+";

        public string Generate(int length = 12)
        {
            if (length < 8)
                throw new ArgumentException("Password length should be at least 8 characters.");

            var bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            var chars = bytes.Select(b => AllowedChars[b % AllowedChars.Length]).ToArray();

            return new string(chars);
        }
    }
}

