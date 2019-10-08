using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using RivestCipher.Model;
using System.Collections.Generic;
using RivestCipher.Helper;
using System.Linq;

namespace RivestCipher.Repository
{
    public class UserRepository
    {
        private string _connectionString;
        XmlSerializer _serializer;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
            _serializer = new XmlSerializer(typeof(List<UserModel>));
        }

        public bool Create(string userName, string passWord)
        {
            try
            {
                var users = GetAll();
                if(users == null || users.Count < 1)
                {
                    users = new List<UserModel>();
                }
                var hashedPass = SecurePasswordHasher.Hash(passWord);
                var writer = new StreamWriter(_connectionString);
                var user = new UserModel
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Password = hashedPass
                };
                users.Add(user);
                _serializer.Serialize(writer, users);
                writer.Close();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Update(List<UserModel> editedUsers)
        {
            var users = GetAll();
            if(users == null)
            {
                return false;
            }
            var writer = new StreamWriter(_connectionString);
            users.ToList().ForEach(user =>
            {
                var editedUsersIndex = editedUsers.FindIndex(_user => Guid.Equals(user.Id, _user.Id));
                if (editedUsersIndex > -1)
                {
                    user = editedUsers[editedUsersIndex];
                }
            });
            _serializer.Serialize(writer, users);
            writer.Close();
            return true;
        }

        public bool UpdateUserLogin(Guid Id, bool isLogIn)
        {
            var users = GetAll();
            if (users == null)
            {
                return false;
            }
            var writer = new StreamWriter(_connectionString);
            users.ToList().ForEach(user =>
            {
                user.IsLoggedIn = isLogIn && Guid.Equals(Id, user.Id);
            });
            _serializer.Serialize(writer, users);
            writer.Close();
            return true;
        }


        public bool UpdateUserDocument(Guid userId, Guid documentId)
        {
            try
            {
                var users = GetAll();
                if (users == null)
                {
                    return false;
                }
                var writer = new StreamWriter(_connectionString);
                users.ToList().ForEach(user =>
                {
                    if(Guid.Equals(userId, user.Id) && !user.Documents.Contains(documentId))
                    {
                        user.Documents.Add(documentId);
                    }
                });
                _serializer.Serialize(writer, users);
                writer.Close();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public UserModel GetLoggedInUser()
        {
            try
            {
                using (FileStream fs = new FileStream(_connectionString, FileMode.Open))
                {
                    var users = (List<UserModel>)_serializer.Deserialize(fs);

                    return users.ToList().Where(user => user.IsLoggedIn == true).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UserModel Get(Guid Id)
        {
            try
            {
                using (FileStream fs = new FileStream(_connectionString, FileMode.Open))
                {
                    var users = (List<UserModel>)_serializer.Deserialize(fs);

                    return users.ToList().Where(user => (Guid.Equals(Id, user.Id))).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<UserModel> GetAll()
        {
            try
            {
                using (FileStream fs = new FileStream(_connectionString, FileMode.Open))
                {
                    return (List<UserModel>)_serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UserModel GetByUserName(string userName)
        {
            try
            {
                using (FileStream fs = new FileStream(_connectionString, FileMode.Open))
                {
                    var users = (List<UserModel>)_serializer.Deserialize(fs);
                    return users.ToList().Where(user => String.Equals(user.UserName, userName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool IsUserExists(string userName)
        {
            try
            {
                using(FileStream fs = new FileStream(_connectionString, FileMode.Open))
                {
                    var users = (List<UserModel>)_serializer.Deserialize(fs);
                    return users.ToList().Where(user => String.Equals(user.UserName, userName, StringComparison.OrdinalIgnoreCase)).Count() > 0;
                }
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
