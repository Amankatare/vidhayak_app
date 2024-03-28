
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;

namespace VidhayakApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _connectionstring;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
      
            _connectionstring = configuration.GetConnectionString("VidhayakAppConnection");
        }

        
        public async Task<bool> RegisterUserAsync(User user)
        {
            // Implement registration logic, including password hashing
            // Check if user already exists, etc.

            var existingUser =await _userRepository.GetByUsernameAsync(user.UserName);

            // if User is already present 
            if (existingUser == null)
            {
               

                string hashPassword = HashPassword(user.PasswordHash);

                user.PasswordHash = hashPassword;

                await _userRepository.AddAsync(user);
              
                return true;
            }
            return false;
        }

        public string HashPassword(string password)
        {
           var hashedPass = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
           return hashedPass;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
       {
            // Implement authentication logic, including password verification
            // Return user if successful


            User record = await _userRepository.GetByUsernameAsync(username);
          
            if (record != null) { 
                var passwordHash = record.PasswordHash;
                var Matched = BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);

                    if (Matched) {
                    User user = new User
                    {

                        UserName = username,
                        PasswordHash = passwordHash
                        
                    };

                    if (user != null) return user;
                    else return null;
                }
            }

            return null;
            
        }


        /*

        */

        /*

        if (isAuthentic)
        {
            Console.WriteLine($"{username} or {password} matched");
            return record;
        }
        else
        {
            Console.WriteLine($"{username} or {password} doesn't match");
        }


        return null;
    }

    public async Task<bool> Validate(string username, string password,string hashpassword)
    {
       using (MySqlConnection connection = new MySqlConnection(_connectionstring))
        {
            connection.Open();

            string query = $"SELECT * FROM Users WHERE username='{username}' AND password='{hashPassword}'";
            using (MySqlCommand sqlCommand = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader read = sqlCommand.ExecuteReader())
                {
                    Console.WriteLine($"{} or {password} matched");
                   // return read.HasRows;
                }
            }
        }
    }
*/
        /*
         
        not taking email by user  so not implementing 
        public async Task<User> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        */


    }
}