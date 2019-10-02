using RivestCipher.Helper;
using RivestCipher.Interface;
using RivestCipher.Model;
using RivestCipher.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivestCipher.Service
{
    class UserService : IUserService
    {

        UserRepository userRepository;
        public UserService(string connectionString)
        {
            userRepository = new UserRepository(connectionString);
        }
        public bool Activate(Guid Id)
        {
            throw new NotImplementedException();
        }

        public bool Create(string userName, string password)
        {
            if (userRepository.IsUserExists(userName))
            {
                throw new Exception("User Exists");
            }
            if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Required Fields Are Blank");
            }
            return userRepository.Create(userName, password);
        }

        public bool Deactivate(Guid Id)
        {
            throw new NotImplementedException();
        }

        public List<UserModel> GetAll()
        {
            return userRepository.GetAll();
        }

        public UserModel GetById(Guid Id)
        {
            return userRepository.Get(Id);
        }

        public UserModel Login(string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Required Fields Are Blank");
            }
            var user = userRepository.GetByUserName(userName);
            if (user == null || !SecurePasswordHasher.Verify(password, user.Password))
            {
                throw new Exception("Wrong User Name Or Password");
            }
            var logUserIn = userRepository.UpdateUserLogin(user.Id, true);
            if(!logUserIn)
            {
                throw new Exception("User Has Not Log In");
            }
            return user;
        }

        public bool Logout(Guid userId)
        {
            var logUserIn = userRepository.UpdateUserLogin(userId, false);
            if (!logUserIn)
            {
                throw new Exception("User Has Not Log Out");
            }
            return true;
        }
    }
}
