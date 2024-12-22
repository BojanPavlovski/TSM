using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DataAccess.Interfaces;
using TaskManagementSystem.Domain.Domain;
using TaskManagementSystem.Dto.Users;
using TaskManagementSystem.Services.Interfaces;
using XSystem.Security.Cryptography;

namespace TaskManagementSystem.Services.Implementations
{
    public class UserService : IUserService
    {
        public IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string LogIn(LogInDto logInDto)
        {
            if (string.IsNullOrEmpty(logInDto.Username) || string.IsNullOrEmpty(logInDto.Password))
            {
                throw new Exception("Username and password are required fields.");
            }
            //generating hash
            string hash = GenerateHash(logInDto.Password);
            User user = _userRepository.GetUserByUserNameAndPassword(logInDto.Username, hash);
            if (user == null)
            {
                throw new Exception("Invalid user login.");
            }

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes("Our very veryyyyyyyyyyyyyyy secret secret key");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
        new[]
        {
                        new Claim(ClaimTypes.Name, $"{user.Firstname} {user.Lastname}"),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        }
        ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            //generate token
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            //convert to string
            string resultToken = jwtSecurityTokenHandler.WriteToken(token);
            return resultToken;
        }

        private static string GenerateHash(string password)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            byte[] hashedBytes = mD5CryptoServiceProvider.ComputeHash(passwordBytes);
            return Encoding.ASCII.GetString(hashedBytes);
        }

        public void Register(AddUserDto addUserDto)
        {
            if (string.IsNullOrEmpty(addUserDto.Firstname))
                throw new Exception("First name is required");
            if (string.IsNullOrEmpty(addUserDto.Lastname))
                throw new Exception("Last name is required");
            if (string.IsNullOrEmpty(addUserDto.UserName))
                throw new Exception("User name is required.");
            if (string.IsNullOrEmpty(addUserDto.Email))
                throw new Exception("Email is required");
            if (addUserDto.Password != addUserDto.ConfirmPassword)
                throw new Exception("Passwords did not match!");

            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(addUserDto.Password));
            var hashedPassword = Encoding.ASCII.GetString(md5data);

            var user = new User()
            {
                Firstname = addUserDto.Firstname,
                Lastname = addUserDto.Lastname,
                Username = addUserDto.UserName,
                Email = addUserDto.Email,
                Password = hashedPassword
            };

            _userRepository.AddUser(user);
        }
    }
}
